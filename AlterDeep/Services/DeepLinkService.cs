using AlterDeep.DBOperations;
using AlterDeep.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AlterDeep.Services
{
    public class DeepLinkService : IDeepLinkService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeepLinkService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public  Task<ApiResponseModel<string>> GetLinkUrlAsync(string link)
        {
            ApiResponseModel<string> apiResponseModel = new ApiResponseModel<string> { IsSucces = false };

            if (!Uri.IsWellFormedUriString(link, UriKind.Absolute) || string.IsNullOrEmpty(link))
            {
                apiResponseModel.IsSucces = false;
                apiResponseModel.Messages.Add("link must not empty or The format of the URI could not be determined.");
                return Task.FromResult(apiResponseModel); 
            }

            DeepLinkHelper deepLinkHelper = new DeepLinkHelper();
            IDeepLinkUrl deepLink = deepLinkHelper.Create(_unitOfWork, link);
            apiResponseModel.Data = deepLink.GetUrl();
            apiResponseModel.IsSucces = !deepLink.Validations.Any();
            apiResponseModel.Messages = deepLink.Validations;
            return Task.FromResult(apiResponseModel);
        }
    }
}
