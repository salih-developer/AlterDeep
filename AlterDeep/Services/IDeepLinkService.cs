using System.Threading.Tasks;
using AlterDeep.Model;

namespace AlterDeep.Services
{
    public interface IDeepLinkService
    {
        Task<ApiResponseModel<string>> GetLinkUrlAsync(string link);
    }
}