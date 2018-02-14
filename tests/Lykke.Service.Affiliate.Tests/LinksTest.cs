using System;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services;
using Lykke.Service.Affiliate.Services;
using Moq;
using Xunit;

namespace Lykke.Service.Affiliate.Tests
{
    public class LinksTest
    {
        [Fact]
        public async Task Create_Link()
        {
            const string baseUrl = "https://lykke.com";

            var href = Guid.NewGuid().ToString("n");

            Mock<ILinkRepository> linkRepoMock = new Mock<ILinkRepository>();
            linkRepoMock.Setup(x => x.CreateAsync("client", "redirect")).Returns(() =>
            {
                var item = new Mock<ILink>();
                item.Setup(x => x.Key).Returns(href);

                return Task.FromResult(item.Object);
            });

            var subject = new LinkService(linkRepoMock.Object, baseUrl);
            var result = await subject.CreateNewLink("client", "redirect");
            
            var subject2 = new LinkService(linkRepoMock.Object, baseUrl + "/");
            var result2 = await subject2.CreateNewLink("client", "redirect");

            Assert.Equal($"{baseUrl}/{href}", result);
            Assert.Equal($"{baseUrl}/{href}", result2);

        }
    }

}
