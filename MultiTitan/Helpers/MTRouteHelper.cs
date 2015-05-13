using MultiTitan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MultiTitan.Helpers
{
	public class MTRouteHelper
	{
		public static MTFeature getActionFeature(MTFeature mainFeature)
		{
			MTFeature actionFeature = mainFeature;
			MTFeaturesGroupModel groupModel = mainFeature.model as MTFeaturesGroupModel;
			while (groupModel != null)
			{
				MTFeature activeFeature = groupModel.activeFeature;
				groupModel = null;
				if (activeFeature != null)
				{
					actionFeature = activeFeature;
					groupModel = actionFeature.model as MTFeaturesGroupModel;
				}
			}

			return actionFeature;
		}

		public static void solveActionRouting(MTFeature mainFeature, string actionRoute, Controller actionController)
		{
			List<string> activeFeatures = new List<string>(actionRoute.Split('-'));
			MTFeature currentFeature = mainFeature;
			while (activeFeatures.Count > 0 && currentFeature != null)
			{
				string topActiveFeatureID = activeFeatures[0];
				if (currentFeature.ID == topActiveFeatureID)
				{
					MTFeaturesGroupModel groupModel = currentFeature.model as MTFeaturesGroupModel;
					if (groupModel == null)
					{
						currentFeature = null;
					}
					else
					{
						activeFeatures.RemoveAt(0);
						if (activeFeatures.Count > 0)
						{
							topActiveFeatureID = activeFeatures[0];
							currentFeature = groupModel.GetFeature(topActiveFeatureID);
							if (currentFeature != null)
							{
								groupModel.activeFeature = currentFeature;
							}
						}
						else
						{
							if (groupModel.features.Count > 0)
							{
								groupModel.activeFeature = groupModel.features[0];
							}
							currentFeature = null;
						}
					}
				}
				else 
				{
					currentFeature = null;
				}
			}
		}
	}
}
