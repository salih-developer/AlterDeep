using System;
using AlterDeep.DBOperations;
using Microsoft.AspNetCore.Routing.Template;

namespace AlterDeep.Services
{
    public class DeepLinkHelper
    {
        public IDeepLinkUrl Create(IUnitOfWork unitOfWork, string url)
        {           

            var isWebLink = LinkChecker(url);
            if (isWebLink)
                return new WebDeepLinkUrl(unitOfWork, url);
            else
                return new MobileDeepLinkUrl(unitOfWork, url);
        }

        private bool LinkChecker(string url)
        {
            Uri uri = new Uri(url);
            var template = TemplateParser.Parse(WebDeepLinkUrl.Template);
            var matcher = new TemplateMatcher(template, DeepLinkUrl.GetDefaults(template));
            return matcher.TryMatch(uri.PathAndQuery, DeepLinkUrl.GetDefaults(template));
        }


    }
}