using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MultiTitan.Filters
{
	public class HttpPostedFileExtensionsAttribute : ValidationAttribute, IClientValidatable
	{
		private readonly FileExtensionsAttribute _fileExtensionsAttribute = new FileExtensionsAttribute();

		public HttpPostedFileExtensionsAttribute()
		{

			ErrorMessage = _fileExtensionsAttribute.ErrorMessage;
		}

		public string Extensions
		{
			get { return _fileExtensionsAttribute.Extensions; }
			set { _fileExtensionsAttribute.Extensions = value; }
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			if (ErrorMessage != _fileExtensionsAttribute.ErrorMessage)
			{
				_fileExtensionsAttribute.ErrorMessage = ErrorMessage;
			}
			var rule = new ModelClientValidationRule
			{
				ValidationType = "extension",
				ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
			};

			rule.ValidationParameters["extension"] =
				_fileExtensionsAttribute.Extensions
					.Replace(" ", string.Empty).Replace(".", string.Empty)
					.ToLowerInvariant();

			yield return rule;
		}

		public override string FormatErrorMessage(string name)
		{
			return _fileExtensionsAttribute.FormatErrorMessage(name);
		}

		public override bool IsValid(object value)
		{
			var file = value as HttpPostedFileBase;
			return _fileExtensionsAttribute.IsValid(file != null ? file.FileName : value);
		}
	}
}
