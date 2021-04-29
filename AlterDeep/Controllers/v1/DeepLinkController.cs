using System.Threading.Tasks;
using AlterDeep.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlterDeep.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DeepLinkController : ControllerBase
    {
        private readonly IDeepLinkService _deepLinkService;

        public DeepLinkController(IDeepLinkService deepLinkService)
        {
            this._deepLinkService = deepLinkService;
        }

        [HttpGet]
        public async Task<ActionResult> Get(string link)
        {
            //TODO buraya redis cache eklenecek
            var model = await _deepLinkService.GetLinkUrlAsync(link);
            return Ok(model);
        }
        
    }
}
