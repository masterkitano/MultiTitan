using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MultiTitan.Models;
using MultiTitan.Controllers;

namespace MultiTitan
{
	public class MTFeature
	{
		protected string _id;
		protected string _name;
		protected string _view;
		protected List<string> _scripts;
		protected IMTModel _model;
		protected MTController _controller;
		protected MTContext _context;

		public string ID { get { return _id; } }
		public string name { get { return _name; } }
		public string view { get { return _view; } }

		public List<string> scripts
		{
			get
			{
				return new List<string>(_scripts);
			}
		}

		public MTContext context { get { return _context; } }
		public IMTModel model
		{
			get
			{ return _model; }
			set
			{ _model = value; }

		}
		public MTController controller
		{
			get
			{ return _controller; }
			set
			{ _controller = value; }

		}
		public MTFeaturesGroupModel parentGroup;

		public string localPath
		{
			get
			{
				MTFeaturesGroupModel parent = this.parentGroup;
				string path = null;
				while (parent != null)
				{
					path = parent.feature.ID + "-" + path;
					parent = parent.feature.parentGroup;
				}

				return path;
			}
		}

		public string actionPath
		{
			get
			{
				string path = context.action + "/" + localPath + ID;
				return path;
			}
		}

		public MTFeature(string ID,
						string name,
						MTContext context,
						string view = null,
						IMTModel model = null,
						MTController controller = null,
						List<string> scripts = null)
		{
			_id = ID;
			_name = name;
			_view = view;
			_model = model;
			_controller = controller;
			_context = context;
			_scripts = scripts;
			if (_model != null)
				_model.feature = this;
			_context.addScripts(_scripts);
		}

		public void updateModel(Controller MVCController, IModelBinder binder)
		{
			if (_model != null)
			{
				_model.update(MVCController, binder);
			}
		}
		public void executeAction(string action, Dictionary<string, string> parameters = null)
		{
			if (_controller != null)
			{
				_controller.executeAction(action, parameters);
			}
		}
	}
}
