using AlterDeep.Controllers;
using AlterDeep.DBOperations;
using AlterDeep.Model;
using AlterDeep.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using UriTemplate.Core;
using Xunit;

namespace AlterDeep.Test
{


    public class DeepLinkServiceTest : IClassFixture<DeepLinkDataFixture>
    {
        DeepLinkDataFixture fixture;

        public DeepLinkServiceTest(DeepLinkDataFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void MobileDeepLinkUrl_Get_Url_Method_Test()
        {
            string link = "ty://?Page=Transaction&ContentId=123&flowId=352";
            IDeepLinkUrl deepLinkUrl = new MobileDeepLinkUrl(fixture._uow, link);
            var weburl = deepLinkUrl.GetUrl();
            Assert.Equal("http://xyz.com/5632/eft-t-123?flowName=TestEftFlow", weburl);
        }

        [Fact]
        public void WebDeepLinkUrl_Get_Url_Method_Test()
        {
            string link = "http://xyz.com/5632/eft-t-123?flowName=TestEftFlow";
            IDeepLinkUrl deepLinkUrl = new WebDeepLinkUrl(fixture._uow, link);
            var weburl = deepLinkUrl.GetUrl();
            Assert.Equal("ty://?Page=Transaction&ContentId=123&flowId=352", weburl);
        }

        [Fact]
        public void DeeplinkHelper_WebDeepLinkUrl_Get_Url_Method_Test()
        {
            string link = "http://xyz.com/5632/eft-t-123?flowName=TestEftFlow";
            DeepLinkHelper deepLinkHelper = new DeepLinkHelper();
            IDeepLinkUrl deepLinkUrl = deepLinkHelper.Create(fixture._uow, link);
            string weburl = deepLinkUrl.GetUrl();
            Assert.Equal("ty://?Page=Transaction&ContentId=123&flowId=352", weburl);
        }

        [Fact]
        public async void DeepLinkService_WebDeepLinkUrl_Get_Url_Method_Test()
        {
            string link = "http://xyz.com/5632/eft-t-123?flowName=TestEftFlow";
            var mq = new Mock<ILogger<DeepLinkService>>();
            DeepLinkService deepLinkService = new DeepLinkService(fixture._uow);
            var deeplink = await deepLinkService.GetLinkUrlAsync(link);
            Assert.True(deeplink.IsSucces);
        }




    }
}
