using MultiTitan.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTitan.Models
{
	public class MTTenant : MTModel
	{
		public string ID;
		public string Name;
		public MTFeaturesBundle featuresBundle;

		public MTTenant() 
		{

		}
	}
}
