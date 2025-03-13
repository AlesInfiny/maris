using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Dressca.Web.Consumer.Dto.Baskets;
using Dressca.Web.Consumer.Dto.Ordering;
using Xunit;

namespace Dressca.IntegrationTest;

public class ShoppingTest(IntegrationTestWebApplicationFactory<Program> factory)
    : IClassFixture<IntegrationTestWebApplicationFactory<Program>>
{
    private static readonly JsonSerializerOptions JsonSerializerWebOptions = new(JsonSerializerDefaults.Web);
    private readonly IntegrationTestWebApplicationFactory<Program> factory = factory;
    private readonly HttpClient client = factory.CreateClient();

    [Fact]
    public async Task 買い物かごに入れた商品を注文できる()
    {
        // Arrange
        await this.factory.InitializeDatabaseAsync();
        var postBasketItemsRequest = CreateBasketItemsRequest();
        var postOrderRequestJson = JsonSerializer.Serialize(CreateDefaultPostOrderRequest());
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var postBasketItemResponse = await this.client.PostAsJsonAsync("api/basket-items", postBasketItemsRequest, cancellationToken);
        postBasketItemResponse.EnsureSuccessStatusCode();

        // API側でBuyerIdを特定できるように、Cookieを付加してリクエストする
        var cookies = postBasketItemResponse.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;
        var postOrderResponse = await this.PostOrderAsync(postOrderRequestJson, cookies);
        var getOrderResponse = await this.GetOrderAsync(cookies, postOrderResponse);
        var orderResponse = await DeserializeOrderResponseAsync(getOrderResponse);

        // Assert
        Assert.NotNull(orderResponse);
        var orderItemResponse = Assert.Single(orderResponse.OrderItems);
        Assert.NotNull(orderItemResponse.ItemOrdered);
        Assert.Equal(postBasketItemsRequest.AddedQuantity, orderItemResponse.Quantity);
        Assert.Equal(postBasketItemsRequest.CatalogItemId, orderItemResponse.ItemOrdered.Id);
    }

    private static async Task<OrderResponse?> DeserializeOrderResponseAsync(HttpResponseMessage getOrderResponse)
    {
        var orderResponseJson = await getOrderResponse.Content.ReadAsStringAsync();
        var orderResponse = JsonSerializer.Deserialize<OrderResponse>(orderResponseJson, JsonSerializerWebOptions);
        return orderResponse;
    }

    private static PostBasketItemsRequest CreateBasketItemsRequest() => new()
    {
        CatalogItemId = 1,
        AddedQuantity = 2,
    };

    private static PostOrderRequest CreateDefaultPostOrderRequest() => new()
    {
        FullName = "国会　太郎",
        PostalCode = "100-8924",
        Todofuken = "東京都",
        Shikuchoson = "千代田区",
        AzanaAndOthers = "永田町1-10-1",
    };

    private async Task<HttpResponseMessage> GetOrderAsync(IEnumerable<string> cookies, HttpResponseMessage postOrderResponse)
    {
        var getOrderRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = postOrderResponse.Headers.Location,
        };

        getOrderRequest.Headers.Add("Cookie", cookies);
        var getOrderResponse = await this.client.SendAsync(getOrderRequest);
        getOrderResponse.EnsureSuccessStatusCode();
        return getOrderResponse;
    }

    private async Task<HttpResponseMessage> PostOrderAsync(string postOrderRequestJson, IEnumerable<string> cookies)
    {
        var postOrderRequest = new HttpRequestMessage
        {
            Content = new StringContent(postOrderRequestJson, Encoding.UTF8, "application/json"),
            Method = HttpMethod.Post,
            RequestUri = new Uri("api/orders", UriKind.Relative),
        };

        postOrderRequest.Headers.Add("Cookie", cookies);
        var postOrderResponse = await this.client.SendAsync(postOrderRequest);
        postOrderResponse.EnsureSuccessStatusCode();
        return postOrderResponse;
    }
}
