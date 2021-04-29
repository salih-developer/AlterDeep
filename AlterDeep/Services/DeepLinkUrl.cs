using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using AlterDeep.DBOperations;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;

namespace AlterDeep.Services
{
    public abstract class DeepLinkUrl : IDeepLinkUrl
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly Uri Uri;
        protected readonly NameValueCollection QueryString;

        public DeepLinkUrl(IUnitOfWork unitOfWork, string link)
        {
            this.UnitOfWork = unitOfWork;
            this.Uri = new Uri(link);
            this.QueryString = HttpUtility.ParseQueryString(this.Uri.Query);
        }

        public List<string> Validations { get; set; } = new List<string>();

        protected virtual bool Valid()
        {
            if (!Uri.IsWellFormedOriginalString())
                Validations.Add("uri is not well formed");
            return !Validations.Any();
        }
        public virtual string GetUrl()
        {
            if (!this.Valid())
            {
                return string.Empty;
            }

            return string.Empty;
        }

        public static RouteValueDictionary GetDefaults(RouteTemplate parsedTemplate)
        {
            var result = new RouteValueDictionary();

            foreach (var parameter in parsedTemplate.Parameters)
            {
                if (parameter.DefaultValue != null)
                {
                    result.Add(parameter.Name, parameter.DefaultValue);
                }
            }

            return result;
        }
    }
}