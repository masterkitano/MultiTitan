using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTitan.Models
{
	public class MTFeaturesGroupModel : MTModel
	{
		private List<MTFeature> _features;
        public MTFeature _activeFeature;

		public MTFeature activeFeature
		{
			get 
			{
				if (_activeFeature == null && _features.Count > 0)
				{
					return _features[0];
				}
				else 
				{
					return _activeFeature;
				}
			}
			set { _activeFeature = value; }
		}

        public ReadOnlyCollection<MTFeature> features
        {
            get
            {
                return _features.AsReadOnly();
            }
        }

		public MTFeaturesGroupModel(List<MTFeature> features = null) 
        {
			_features = new List<MTFeature>();

            if (features != null)
            {
				foreach (MTFeature feature in features) 
				{
					addFeature(feature);
				}
            }
        }

        public MTFeature GetFeature(string featureID) 
        {
            MTFeature feature = null;
            if (features != null)
            {
               feature = _features.FirstOrDefault(f => f.ID == featureID);
            }
            return feature;
        }

		public void addFeature(MTFeature feature)
        {
            if (feature == this.feature) 
            {
                throw new Exception("PalicWebCore Error: You can't add a feature to its own group model");
            }
            feature.parentGroup = this;
            _features.Add(feature);
            
        }

		public void removeFeature(MTFeature feature)
        {
            _features.Remove(feature);
        }
	}
}
