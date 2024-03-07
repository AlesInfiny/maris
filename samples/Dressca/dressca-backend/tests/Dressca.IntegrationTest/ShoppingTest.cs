using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Dressca.Web.Dto.Baskets;
using Dressca.Web.Dto.Ordering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Dressca.IntegrationTest;

public class ShoppingTest(IntegrationTestWebApplicationFactory<Program> factory)
    : IClassFixture<IntegrationTestWebApplicationFactory<Program>>
{
    private readonly IntegrationTestWebApplicationFactory<Program> factory = factory;

    [Fact]
    public async Task 買い物かごに入れた商品を注文できる()
    {
        // Arrange
        using (var scope = this.factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
            DatabaseHelper.ClearTransactionTable(dbContext);
        }

        var client = this.factory.CreateClient();
        var postBasketItemsRequest = this.CreateBasketItemsRequest();
        var postOrderRequestJson = JsonSerializer.Serialize(this.CreateDefaultPostOrderRequest());

        // Act
        var postBasketItemResponse = await client.PostAsJsonAsync("api/basket-items", postBasketItemsRequest);
        postBasketItemResponse.EnsureSuccessStatusCode();
        var cookies = postBasketItemResponse.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

        var httpRequestMessage = new HttpRequestMessage
        {
            Content = new StringContent(postOrderRequestJson, Encoding.UTF8, "application/json"),
            Method = HttpMethod.Post,
            RequestUri = new Uri("api/orders", UriKind.Relative),
        };
        httpRequestMessage.Headers.Add("Cookie", cookies);
        var orderResponse = await client.SendAsync(httpRequestMessage);

        // Assert
        orderResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, orderResponse.StatusCode);
    }

    private PostBasketItemsRequest CreateBasketItemsRequest()
    {
        var postBasketItemsRequest = new PostBasketItemsRequest()
        {
            CatalogItemId = 1,
            AddedQuantity = 2,
        };
        return postBasketItemsRequest;
    }

    private PostOrderRequest CreateDefaultPostOrderRequest()
    {
        var postOrderRequest = new PostOrderRequest()
        {
            FullName = "国会　太郎",
            PostalCode = "100-8924",
            Todofuken = "東京都",
            Shikuchoson = "千代田区",
            AzanaAndOthers = "永田町1-10-1",
        };

        return postOrderRequest;
    }
}
