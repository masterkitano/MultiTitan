using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MultiTitan.Models;
using MultiTitan.Config;
using System.Web;

namespace MultiTitan.Filters
{
	public class MTTenantContext : ActionFilterAttribute
	{
		public string viewsPath { get; set; }
		public string scriptsPath { get; set; }
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			MTTenant tenant = (MTTenant)filterContext.HttpContext.Session["currentTenant"];

			MTConfig config = new MTConfig(viewsPath, scriptsPath);
			MTContext context = new MTContext(filterContext.ActionParameters["controller"].ToString(), 
												filterContext.ActionParameters["action"].ToString(), 
												config, 
												tenant.featuresBundle, 
												(Controller)filterContext.Controller);

			filterContext.ActionParameters["context"] = context;
			filterContext.ActionParameters["tenant"] = tenant;
		}
	}
}
