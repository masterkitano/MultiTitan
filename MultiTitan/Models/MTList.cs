using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MultiTitan.Models
{
	public class MTList<T> : List<T>, IMTModel
	{
		MTFeature _feature;
		public MTFeature feature
		{
			get
			{
				return _feature;
			}
			set
			{
				_feature = value;
			}
		}

		public virtual void update(Controller MVCController, IModelBinder binder)
		{
			if (feature.context.featuresBundle.HasFeature(feature.ID, feature.localPath))
			{
				Type modelType = this.GetType();
				ModelBindingContext bindingContext = new ModelBindingContext()
				{
					ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => this, modelType),
					ModelState = MVCController.ModelState,
					ValueProvider = MVCController.ValueProvider
				};
				binder.BindModel(MVCController.ControllerContext, bindingContext);
			}
		}
	}
}
