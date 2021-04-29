using System.Collections.Generic;

namespace AlterDeep.Services
{
    public interface IDeepLinkUrl
    {
        List<string> Validations { get; set; }
        string GetUrl();
    }
}