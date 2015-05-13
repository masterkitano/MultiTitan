using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MultiTitan.Config
{
	public class MTConfigurationLoader
	{
		private string _configurationPath;

		private const string REGISTER_FEATURE = "registerFeature";
		private const string EXCLUDE_FEATURE = "excludeFeature";

		private const string REGISTER_PATH = "registerPath";
		private const string EXCLUDE_PATH = "excludePath";

		private const string ID_ATTRIBUTE = "id";
		private const string PATH_ATTRIBUTE = "path";
		private const string ROUTE_ATTRIBUTE = "route";
		private const string FEATURE_ATTRIBUTE = "feature";
		private const string INHERIT_ATTRIBUTE = "inherit";

		private const string GLOBAL_CONFIGURATION_FILENAME = "global.xml";
		public MTConfigurationLoader(string path)
		{
			_configurationPath = path;
		}

		public MTFeaturesBundle loadGlobalBundle() 
		{
			return appendBundleFromFile(GLOBAL_CONFIGURATION_FILENAME);
		}

		public MTFeaturesBundle loadBundle(string configurationFilename) 
		{
			MTFeaturesBundle bundle = appendBundleFromFile(GLOBAL_CONFIGURATION_FILENAME);
			bundle = appendBundleFromFile(configurationFilename,bundle);
			return bundle;
		}

		private MTFeaturesBundle appendBundleFromFile(string configurationFilename, MTFeaturesBundle bundle = null) 
		{
			if(bundle == null)
				bundle = new MTFeaturesBundle();

			XDocument document = XDocument.Load(_configurationPath+configurationFilename);
			var registerFeatureKeys = from r in document.Descendants(REGISTER_FEATURE) 
									  where r.Attribute(ID_ATTRIBUTE) != null
									  select r;

			addFeatureKeys(registerFeatureKeys, bundle, REGISTER_FEATURE);

			var excludeFeaturesKeys = from e in document.Descendants(EXCLUDE_FEATURE)
									  where e.Attribute(ID_ATTRIBUTE) != null
									  select e;

			addFeatureKeys(excludeFeaturesKeys, bundle, EXCLUDE_FEATURE);

			var registerPathsKeys = from p in document.Descendants(REGISTER_PATH)
									where p.Attribute(ROUTE_ATTRIBUTE) != null
									select p;

			addPathKeys(registerPathsKeys, bundle, REGISTER_PATH);

			var excludePathsKeys = from p in document.Descendants(EXCLUDE_PATH)
								   where p.Attribute(ROUTE_ATTRIBUTE) != null
								   select p;

			addPathKeys(excludePathsKeys, bundle, EXCLUDE_PATH);

			return bundle;
		}

		private void addFeatureKeys(IEnumerable<XElement> keys, MTFeaturesBundle bundle, string keyType)
		{
			foreach (XElement feature in keys)
			{
				string id = feature.Attribute(ID_ATTRIBUTE).Value;
				string path = null;
				bool inherit = true;
				string inheritString = safeGetAttributeValue(feature, INHERIT_ATTRIBUTE);
				if (inheritString != null)
					inherit = bool.Parse(inheritString);
				

				MTConfigurationKey key = null;

				if (feature.HasElements)
				{
					var paths = from p in feature.Elements(PATH_ATTRIBUTE) 
								where p.Attribute(ROUTE_ATTRIBUTE) != null
								select p;
					foreach (XElement pathElement in paths)
					{
						path = pathElement.Attribute(ROUTE_ATTRIBUTE).Value;
						inheritString = safeGetAttributeValue(pathElement, INHERIT_ATTRIBUTE);
						bool internalInherit = inherit;
						if (inheritString != null)
							internalInherit = bool.Parse(inheritString);

						key = new MTConfigurationKey(id, path, internalInherit);
						addKey(key, keyType, bundle);
					}
				}
				else
				{
					path = safeGetAttributeValue(feature, PATH_ATTRIBUTE);
					if(path == null)
						path = MTConfig.GLOBAL_PATH;
					key = new MTConfigurationKey(id, path, inherit);
					addKey(key, keyType, bundle);
				}
			}
		}

		private void addPathKeys(IEnumerable<XElement> keys, MTFeaturesBundle bundle, string keyType)
		{
			foreach (XElement pathElement in keys)
			{
				string id = null;
				string route = pathElement.Attribute(ROUTE_ATTRIBUTE).Value;
				bool inherit = true;
				string inheritString = safeGetAttributeValue(pathElement, INHERIT_ATTRIBUTE);
				if (inheritString != null)
					inherit = bool.Parse(inheritString);

				MTConfigurationKey key = null;
				if (pathElement.HasElements)
				{
					var features = from f in pathElement.Elements(FEATURE_ATTRIBUTE)
								   where f.Attribute(ID_ATTRIBUTE) != null
								   select f;

					foreach (XElement feature in features)
					{
						id = feature.Attribute(ID_ATTRIBUTE).Value;

						inheritString = safeGetAttributeValue(feature, INHERIT_ATTRIBUTE);
						bool internalInherith = inherit;
						if (inheritString != null)
							internalInherith = bool.Parse(inheritString);

						key = new MTConfigurationKey(id, route, internalInherith);
						addKey(key, keyType, bundle);
					}
				}
			}
		}

		private void addKey(MTConfigurationKey key, string type, MTFeaturesBundle bundle)
		{
			if(type == REGISTER_FEATURE || type == REGISTER_PATH)
			{
				bundle.RegisterFeature(key.featureID, key.featurePath, key.inherit);
			}
			else if(type == EXCLUDE_FEATURE || type == EXCLUDE_PATH)
			{
				bundle.excludeFeature(key.featureID, key.featurePath, key.inherit);
			}
		}

		private string safeGetAttributeValue(XElement element, string attributeName)
		{
			string value = (element.Attribute(attributeName) == null)
							? null
							: element.Attribute(attributeName).Value;
			return value;
		}
	}
}
