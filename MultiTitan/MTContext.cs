using MultiTitan.Config;
using MultiTitan.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MultiTitan
{
	public class MTContext
	{
		private Controller _MVCController;
		private MTFeaturesBundle _bundle;
		private MTConfig _config;
		private string _action;
		private string _controllerName;
		private List<string> _scripts;

		public MTFeaturesBundle featuresBundle
		{
			get { return _bundle; }
		}

		public MTConfig config
		{
			get { return _config; }
		}

		public string controller
		{
			get { return _controllerName; }
			set { _controllerName = value; }
		}

		public string action
		{
			get { return _action; }
			set { _action = value; }
		}

		public Controller MVCController
		{
			get { return _MVCController; }
		}

		public List<string> scripts
		{
			get
			{
				return new List<string>(_scripts);
			}
		}

		public MTContext() 
		{
		}

		public MTContext(string controllerName, string action, MTConfig config, MTFeaturesBundle bundle, Controller MVCController) 
		{
			_action = action;
			_controllerName = controllerName;
			_bundle = bundle;
			_MVCController = MVCController;
			_config = config;
			_scripts = new List<string>();
		}

		public void addScript(string script)
		{
			if (!_scripts.Exists(s => s == script)) 
			{
				_scripts.Add(script);
			}
		}

		public void addScripts(List<string> scripts)
		{
			if(scripts != null)
			_scripts.AddRange(scripts.Except(_scripts));
		}

		public void removeScript(string script)
		{
			_scripts.Remove(script);
		}

		public void removeScripts(List<string> scripts)
		{
			_scripts = _scripts.Except(scripts).ToList();
		}

		public MTFeature getActiveFeature(MTFeature mainFeature, string actionRoute, IModelBinder binder) 
		{
			MTRouteHelper.solveActionRouting(mainFeature, actionRoute, MVCController);

			MTFeature activeFeature = MTRouteHelper.getActionFeature(mainFeature);
			if (activeFeature != null)
			{
				return activeFeature;
			}
			return mainFeature;
		}
	}
}
