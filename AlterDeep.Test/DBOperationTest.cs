using Xunit;

namespace AlterDeep.Test
{
    public class DBOperationTest : IClassFixture<DeepLinkDataFixture>
    {
        DeepLinkDataFixture fixture;

        public DBOperationTest(DeepLinkDataFixture fixture)
        {
            this.fixture = fixture;
        }
        [Fact]
        public async void DBoperation_Mobil_Link_Data_Select_Test()
        {
            var f = fixture._flowRepository.Get(x => x.Name == "TestEftFlow");
            Assert.NotNull(f);
        }
    }
}
