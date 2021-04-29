using System;
using System.Linq;
using AlterDeep.DBOperations;
using AlterDeep.DBOperations.Model;

namespace AlterDeep.Services
{
    public class MobileDeepLinkUrl : DeepLinkUrl
    {
        public MobileDeepLinkUrl(IUnitOfWork unitOfWork, string link) : base(unitOfWork, link) { }
        protected override bool Valid()
        {
            string page = this.QueryString.Get("Page");
            if (string.IsNullOrEmpty(page))
                Validations.Add("Page :  must not empty");

            string contentId = this.QueryString.Get("ContentId");
            if (string.IsNullOrEmpty(contentId))
                Validations.Add("ContentId :  must not empty");

            return !Validations.Any();
        }

        public override string GetUrl()
        {
            if (this.Valid())
            {
                string pageParam = this.QueryString.Get("Page");
                int contentId = Convert.ToInt32(this.QueryString.Get("ContentId"));
                string flowId = this.QueryString.Get("flowId");
                var page = this.UnitOfWork.GetRepository<TransactionPage>().Get(x => x.Name == pageParam && x.TransactionPageContents.Any(v => v.ContentId == contentId), r => r.TransactionPageContents);
                if (page == null)
                {
                    this.Validations.Add("Content not found !");
                    return string.Empty;
                }
                string CustomerNumberORCitizenshipNumber = "5632";
                string pattern = $"http://xyz.com/{CustomerNumberORCitizenshipNumber}/{page.FriendlyName}-t-{page.TransactionPageContents.First().ContentId}";

                if (!string.IsNullOrEmpty(flowId))
                {
                    var flow = this.UnitOfWork.GetRepository<Flow>().Get(x => x.Id == int.Parse(flowId));
                    pattern += $"?flowName={flow.Name}";
                }
                return pattern;
            }

            return string.Empty;
        }
    }
}