using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTitan.Config
{
	public class MTConfigurationKey
	{
		private string _featureID;
		private string _featurePath;
		private bool _inherit;
		public string featureID { get { return _featureID; } }
		public string featurePath { get { return _featurePath; } }
		public bool inherit { get { return _inherit; } }

		public MTConfigurationKey(string featureID, string featurePath, bool inherit = true) 
		{
			_featureID = featureID;
			_featurePath = featurePath;
			_inherit = inherit;
		}
	}
}
