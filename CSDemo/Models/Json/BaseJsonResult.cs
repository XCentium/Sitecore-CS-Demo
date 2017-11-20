using Sitecore.Commerce.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSDemo.Models.Json
{
    public class BaseJsonResult : JsonResult
    {
        private readonly List<string> _errors;

        public BaseJsonResult()
        {
            this._errors = new List<string>();
            this.Success = true;
        }

        public BaseJsonResult(ServiceProviderResult result)
        {
            this._errors = new List<string>();
            this.Success = true;
            if (result != null)
            {
                this.SetErrors(result);
            }
        }

        public BaseJsonResult(string url)
        {
            this._errors = new List<string>();
            this.Success = false;
            this.Url = url;
        }

        public BaseJsonResult(string area, Exception exception)
        {
            this._errors = new List<string>();
            this.Success = false;
            this.SetErrors(area, exception);
        }

        public void SetErrors(ServiceProviderResult result)
        {
            this.Success = result.Success;
            if (result.SystemMessages.Count > 0)
            {
                foreach (SystemMessage message in result.SystemMessages)
                {
                    string systemMessage = string.Empty;
                    this.Errors.Add(string.IsNullOrEmpty(systemMessage) ? message.Message : systemMessage);
                }
            }
        }

        public void SetErrors(List<string> errors)
        {
            if (errors.Any<string>())
            {
                this.Success = false;
                this._errors.AddRange(errors);
            }
        }

        public void SetErrors(string area, Exception exception)
        {
            object[] args = new object[] { area, exception.Message };
            this._errors.Add(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", args));
            this.Success = false;
        }

        public List<string> Errors =>
            this._errors;

        public bool HasErrors =>
            ((this._errors != null) && this._errors.Any<string>());

        public bool Success { get; set; }

        public string Url { get; set; }
    }
}