using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTitan.Config
{
	public class MTFeaturesBundle
	{
		 private List<MTConfigurationKey> _registeredKeys;
		private List<MTConfigurationKey> _excludedKeys;

        public ReadOnlyCollection<MTConfigurationKey> registeredKeys 
        {
            get 
            {
				return _registeredKeys.AsReadOnly();
            }
        }

		public ReadOnlyCollection<MTConfigurationKey> excludedKeys
		{
			get
			{
				return _excludedKeys.AsReadOnly();
			}
		}

		public MTFeaturesBundle() 
        {
            _registeredKeys = new List<MTConfigurationKey>();
			_excludedKeys = new List<MTConfigurationKey>();
        }

		public MTFeaturesBundle RegisterFeature(string featureID, 
												string path = MTConfig.GLOBAL_PATH, 
												  bool inherit = true)
        {
			path = normalizePath(path);
            if (findKey(featureID, path, _registeredKeys) == null)
            {
				_registeredKeys.Add(new MTConfigurationKey(featureID, path, inherit));
            }

			return this;
        }

		public MTFeaturesBundle UnregisterFeature(string featureID,
												  string path = MTConfig.GLOBAL_PATH)
        {
			path = normalizePath(path);
			removeFeatureKeys(featureID, path);
			return this;
        }

		public MTFeaturesBundle UnregisterAll(string featureID) 
		{
			removeFeatureKeys(featureID);
			return this;
		}

		public MTFeaturesBundle excludeFeature(string featureID, string path, bool inherit = true) 
		{
			path = normalizePath(path);
			
			if (findKey(featureID, path, _excludedKeys) == null)
			{
				_excludedKeys.Add(new MTConfigurationKey(featureID, path, inherit));
			}

			return this;
		}

		private MTConfigurationKey findKey(string featureID, 
											string path,
											List<MTConfigurationKey> keys,
											bool searchInherit = false) 
		{
			MTConfigurationKey key = null;
			if (searchInherit)
			{
				key = keys.FirstOrDefault(
											f => f.featureID == featureID
												&& f.featurePath == path
												&& f.inherit == true
											);
			}
			else 
			{
				
				key = keys.FirstOrDefault(
											f => f.featureID == featureID
												&& f.featurePath == path
											);
			}
			return key;
		}

		private void removeFeatureKeys(string featureID, string path = null)
		{
			Predicate<MTConfigurationKey> predicate = null;
			if (path == null)
			{
				predicate = new Predicate<MTConfigurationKey>(f => f.featureID == featureID);
			}
			else 
			{
				predicate = new Predicate<MTConfigurationKey>(
											f => f.featureID == featureID
											&& f.featurePath == path);
			}

			_registeredKeys.RemoveAll(predicate);
		}

		public bool HasFeature(string featureID, string path = null) 
		{
			path = normalizePath(path);

			MTConfigurationKey registeredKey = null;
			MTConfigurationKey excludedKey = null;

			List<string> features = new List<string>(path.Substring(0, path.Length - 1).Split('-'));

			//check key in global and then parent features for inherit
			int searchStartIndex = 0;
			for (int i = searchStartIndex; i < features.Count(); i++) 
			{
				string topFeatureID = features[i];
				List<string> featuresCopy = new List<string>(features);
				featuresCopy.RemoveRange(i, featuresCopy.Count-i);
				string topFeaturePath = normalizePath(String.Join("-", featuresCopy));

				//search for key in global
				if (i == searchStartIndex)
				{
					if (registeredKey == null)
						registeredKey = findKey(featureID, topFeaturePath, _registeredKeys);

					if (excludedKey == null)
						excludedKey = findKey(featureID, topFeaturePath, _excludedKeys);
				}
				// else, search for parent features with inherit
				else
				{
					if (registeredKey == null)
						registeredKey = findKey(topFeatureID, topFeaturePath, _registeredKeys, true);
					
					if (excludedKey == null)
						excludedKey = findKey(topFeatureID, topFeaturePath, _excludedKeys, true);
				}
			}

			//after parent checks, check for feature registry key
			if (registeredKey == null)
				registeredKey = findKey(featureID, path, _registeredKeys);

			if (excludedKey == null)
				excludedKey = findKey(featureID, path, _excludedKeys);

			return (registeredKey != null && excludedKey == null) ? true : false;
		}

		private string normalizePath(string path)
		{
			if (path == null)
				path = "";

			if (path != "" && path.Last<char>() != '-')
				path += "-";

			if (!path.StartsWith(MTConfig.GLOBAL_PATH))
				path = MTConfig.GLOBAL_PATH + path;

			return path;
		}
	}
}
