using MultiTitan.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Optimization;
using System.Web.Routing;

namespace MultiTitan.Helpers
{
	public static class MTRenderHelper
	{
		public static MvcHtmlString MTRenderFeature(this HtmlHelper helper,
														  MTFeature feature,
														  object containerHTMLAttributes = null,
														  string unavailableFeatureHTML = MTConfig.DEFAULT_UNAVAILABLE_FEATURE_HTML)
		{
			if (feature == null)
				return new MvcHtmlString("");

			if (feature.context.featuresBundle.HasFeature(feature.ID, feature.localPath))
			{
				String featureHTML = 
					helper.Partial(feature.context.config.viewsPath + feature.view + ".cshtml", feature).ToString();
				TagBuilder featureContainer = new TagBuilder("div");

				if (containerHTMLAttributes == null) 
				{
					containerHTMLAttributes = new { @class="container-fluid"};
				}

				featureContainer.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(containerHTMLAttributes), true);
				featureContainer.MergeAttribute("featureID", feature.ID, true);

				featureContainer.InnerHtml = featureHTML;

				return new MvcHtmlString(featureContainer.ToString());
			}
			else
			{
				return new MvcHtmlString(unavailableFeatureHTML);
			}
		}

		public static MvcHtmlString MTRenderScripts(this HtmlHelper helper,
														  List<string> scripts,
															MTContext context)
		{
			string scriptsIncludes = Scripts.Render(context.config.scriptsPath+"PalicWebCore.js").ToString();
			foreach (string script in scripts)
			{
				scriptsIncludes += Scripts.Render(context.config.scriptsPath + script + ".js").ToString();
			}
			return MvcHtmlString.Create(scriptsIncludes);
		}

		public static string MTFeatureAction(this UrlHelper helper,
												MTFeature feature,
												string featureAction,
												bool onlyBoundValues = true)
		{
			RouteValueDictionary finalRouteValues =
				 new RouteValueDictionary(feature.model);

			if (onlyBoundValues)
			{
				RemoveUnboundValues(finalRouteValues, feature.model);
			}

			string featurePath = feature.context.action + "/" + feature.localPath + feature.ID;
			// Internally, MVC calls an overload of GenerateUrl with
			// hard-coded defaults.  Since we shouldn't know what these
			// defaults are, we call the non-extension equivalents.
			return helper.Action(featurePath, feature.context.controller, finalRouteValues)
								+ "&featureAction=" + featureAction;
		}

		private static void RemoveUnboundValues(RouteValueDictionary routeValues
											, object source)
		{
			if (source == null)
			{
				return;
			}

			var type = source.GetType();

			BindAttribute b = null;

			foreach (var attribute in type.GetCustomAttributes(true))
			{
				if (attribute is BindAttribute)
				{
					b = (BindAttribute)attribute;
					break;
				}
			}

			if (b == null)
			{
				return;
			}

			foreach (var property in type.GetProperties())
			{
				var propertyName = property.Name;
				if (!b.IsPropertyAllowed(propertyName))
				{
					routeValues.Remove(propertyName);
				}
			}
		}
	}
}
