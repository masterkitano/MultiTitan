using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTitan.Config
{
	public class MTConfig
	{
		public const string GLOBAL_PATH = "global-";
		public const string DEFAULT_UNAVAILABLE_FEATURE_HTML = "<p class=\"text-center\"><b>Feature not available for current tenant</b></p>"; 

		public string viewsPath;
		public string scriptsPath;

		public MTConfig(string viewsPath, string scriptsPath) 
		{
			this.viewsPath = viewsPath;
			this.scriptsPath = scriptsPath;
		}
	}
}
