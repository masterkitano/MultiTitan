using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using MultiTitan.Config;

namespace MultiTitan.Filters
{
	public class MTGlobalContext : ActionFilterAttribute
	{
		public string viewsPath { get; set; }
		public string scriptsPath { get; set; }
		public string configPath { get; set; }
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			MTConfigurationLoader configurationLoader = new MTConfigurationLoader(((Controller)filterContext.Controller).Server.MapPath(configPath));
			MTFeaturesBundle globalFeaturesBundle = configurationLoader.loadGlobalBundle();

			MTConfig config = new MTConfig(viewsPath, scriptsPath);
			MTContext context = new MTContext(filterContext.ActionParameters["controller"].ToString(),
												filterContext.ActionParameters["action"].ToString(),
												config,
												globalFeaturesBundle,
												(Controller)filterContext.Controller);

			filterContext.ActionParameters["context"] = context;
		}
	}
}
