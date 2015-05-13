using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiTitan.Models;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;


namespace MultiTitan.Controllers
{
	public class MTController
	{
		protected IMTModel _model;
		public MTController(IMTModel model) 
		{
			_model = model;
		}

		public virtual object executeAction(string action, Dictionary<string,string> parameters = null)
		{
			if (action != null 
				&& _model.feature.context.featuresBundle.HasFeature(_model.feature.ID, _model.feature.localPath))
			{
				Type thisType = this.GetType();
				MethodInfo theMethod = thisType.GetMethod(action);
				ParameterInfo[] methodParameters = theMethod.GetParameters();
				object[] convertedParameters = null;
				if (parameters != null)
				{
					convertedParameters = new object[methodParameters.Length];
					for (int i = 0; i < methodParameters.Length; i++)
					{
						ParameterInfo parameterInfo = methodParameters[i];
						Type parameterType = parameterInfo.ParameterType;
						TypeConverter typeConverter = TypeDescriptor.GetConverter(parameterType);

						foreach (KeyValuePair<string,string> parameter in parameters) 
						{
							if (parameter.Key == parameterInfo.Name) 
							{
								convertedParameters[i] = 
									typeConverter.ConvertFrom(null, CultureInfo.InvariantCulture, parameter.Value);
							}
						}
					}
				}
				return theMethod.Invoke(this, convertedParameters);
			}
			return null;
		}
	}
}
