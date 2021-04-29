using System.Linq;
using System.Web;
using AlterDeep.DBOperations;
using AlterDeep.DBOperations.Model;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;

namespace AlterDeep.Services
{
    public class WebDeepLinkUrl : DeepLinkUrl
    {
        public const string Template = "/{CustomerNumberORCitizenshipNumber}/{TransactionName}-t-{ContentId}"; //configden alınabilir.
        public WebDeepLinkUrl(IUnitOfWork unitOfWork, string link) : base(unitOfWork, link) { }
        public override string GetUrl()
        {
            if (this.Valid())
            {
                var template = TemplateParser.Parse(WebDeepLinkUrl.Template);
                var matcher = new TemplateMatcher(template, GetDefaults(template));
                var values = new RouteValueDictionary();
                matcher.TryMatch(this.Uri.AbsolutePath, values);
                var page = this.UnitOfWork.GetRepository<TransactionPage>().Get(x =>
                    x.FriendlyName == values["TransactionName"].ToString() &&
                    x.TransactionPageContents.Any(v => v.ContentId == int.Parse(values["ContentId"].ToString())), c => c.TransactionPageContents);

                if (page == null)
                {
                    this.Validations.Add("Content not found !");
                    return string.Empty;
                }
                string pattern = $"ty://?Page={page.Name}&ContentId={page.TransactionPageContents.First().ContentId}";
                var query = HttpUtility.ParseQueryString(this.Uri.Query);
                if (query.Count > 0 && !string.IsNullOrEmpty(query.Get("flowName")))
                {
                    var flow = this.UnitOfWork.GetRepository<Flow>().Get(x => x.Name == query.Get("flowName"));
                    if (flow != null)
                        pattern += $"&flowId={flow.Id}";
                }
                return pattern;
            }

            return string.Empty;
        }

    }
}