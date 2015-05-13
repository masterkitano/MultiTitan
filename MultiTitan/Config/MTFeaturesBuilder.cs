using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTitan.Config
{
	public class MTFeaturesBuilder
	{
		private Dictionary<string, Func<MTContext, MTFeature>> _builders;

		public MTFeaturesBuilder(Dictionary<string, Func<MTContext, MTFeature>> builders = null) 
		{
			_builders = builders;
			if (_builders == null) 
			{
				_builders = new Dictionary<string, Func<MTContext, MTFeature>>();
			}
		}

		public void addBuilder(string featureID, Func<MTContext, MTFeature> builder)
		{
			_builders.Add(featureID, builder);
			
			if (_builders.ContainsKey(featureID))
			{
				_builders[featureID] = builder;
			}
			else
			{
				_builders.Add(featureID, builder);
			}
		}

		public void removeBuilder(string featureID)
		{
			if (_builders.ContainsKey(featureID))
			{
				_builders.Remove(featureID);
			}
		}

		public MTFeature buildFeature(string featureID, MTContext context) 
		{
			MTFeature feature = null;
			Func<MTContext, MTFeature> builder = null;

			if(_builders.ContainsKey(featureID))
			{
				builder = _builders[featureID];
				feature = builder(context);
			}

			return feature;
		}
	}
}
