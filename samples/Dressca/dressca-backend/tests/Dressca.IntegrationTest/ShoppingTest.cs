using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Dressca.Web.Dto.Baskets;
using Dressca.Web.Dto.Ordering;
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
        await this.factory.InitializeDatabaseAsync();
        var client = this.factory.CreateClient();
        var postBasketItemsRequest = this.CreateBasketItemsRequest();
        var postOrderRequestJson = JsonSerializer.Serialize(this.CreateDefaultPostOrderRequest());

        // Act
        var postBasketItemResponse = await client.PostAsJsonAsync("api/basket-items", postBasketItemsRequest);
        postBasketItemResponse.EnsureSuccessStatusCode();
        var cookies = postBasketItemResponse.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

        var postOrderRequest = new HttpRequestMessage
        {
            Content = new StringContent(postOrderRequestJson, Encoding.UTF8, "application/json"),
            Method = HttpMethod.Post,
            RequestUri = new Uri("api/orders", UriKind.Relative),
        };
        postOrderRequest.Headers.Add("Cookie", cookies);
        var postOrderResponse = await client.SendAsync(postOrderRequest);
        postOrderResponse.EnsureSuccessStatusCode();

        var getOrderRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = postOrderResponse.Headers.Location,
        };
        getOrderRequest.Headers.Add("Cookie", cookies);
        var result = await client.SendAsync(getOrderRequest);
        result.EnsureSuccessStatusCode();

        var orderResponseJson = await result.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var orderResponse = JsonSerializer.Deserialize<OrderResponse>(orderResponseJson, options);

        // Assert
        Assert.NotNull(orderResponse);
        var orderItemResponse = orderResponse.OrderItems.FirstOrDefault();
        Assert.NotNull(orderItemResponse);
        Assert.NotNull(orderItemResponse.ItemOrdered);
        Assert.Equal(postBasketItemsRequest.AddedQuantity, orderItemResponse.Quantity);
        Assert.Equal(postBasketItemsRequest.CatalogItemId, orderItemResponse.ItemOrdered.Id);
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
