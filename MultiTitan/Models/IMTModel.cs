using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MultiTitan.Models
{
	public interface IMTModel
	{
		MTFeature feature
		{
			get;
			set;
		}
		void update(Controller MVCController, IModelBinder binder);
	}
}
