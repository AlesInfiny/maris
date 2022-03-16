using Dressca.ApplicationCore.Accounting;

namespace Dressca.UnitTests.ApplicationCore.Accounting;

public class AccountTest
{
    [Fact]
    public void 会計アイテムがないとき会計アイムの合計金額は0円()
    {
        // Arrange
        var account = new Account(Array.Empty<AccountItem>());

        // Act
        var itemsTotal = account.GetItemsTotalPrice();

        // Assert
        Assert.Equal(0m, itemsTotal);
    }

    [Fact]
    public void 会計アイテムが1件あるとき会計アイムの合計金額は送料と消費税をのぞく合計金額になる()
    {
        // Arrange
        var accountItems = new AccountItem[] { new(5, 300m) };
        var account = new Account(accountItems);

        // Act
        var itemsTotal = account.GetItemsTotalPrice();

        // Assert
        Assert.Equal(1500m, itemsTotal);
    }

    [Fact]
    public void 会計アイテムが2件あるとき会計アイムの合計金額は送料と消費税をのぞく合計金額になる()
    {
        // Arrange
        var accountItems = new AccountItem[]
        {
            new(5, 300m),
            new(2, 400m),
        };
        var account = new Account(accountItems);

        // Act
        var itemsTotal = account.GetItemsTotalPrice();

        // Assert
        Assert.Equal(2300m, itemsTotal);
    }

    [Fact]
    public void 会計アイムの合計金額が5000円未満の場合は送料が500円になる()
    {
        // Arrange
        var accountItems = new AccountItem[] { new(1, 4999m) };
        var account = new Account(accountItems);

        // Act
        var deliveryCharge = account.GetDeliveryCharge();

        // Assert
        Assert.Equal(500m, deliveryCharge);
    }

    [Fact]
    public void 会計アイムの合計金額が5000円以上の場合は送料が0円になる()
    {
        // Arrange
        var accountItems = new AccountItem[] { new(1, 5000m) };
        var account = new Account(accountItems);

        // Act
        var deliveryCharge = account.GetDeliveryCharge();

        // Assert
        Assert.Equal(0m, deliveryCharge);
    }

    [Fact]
    public void 消費税は会計アイテムの合計金額と送料に対してかかる()
    {
        // Arrange
        var accountItems = new AccountItem[] { new(1, 4500m) };
        var account = new Account(accountItems);

        // Act
        var tax = account.GetConsumptionTax();

        // Assert
        Assert.Equal(500m, tax);
    }

    [Fact]
    public void 消費税は0円未満の端数が切り捨てられる()
    {
        // Arrange
        var accountItems = new AccountItem[] { new(1, 19m) };
        var account = new Account(accountItems);

        // Act
        var tax = account.GetConsumptionTax();

        // Assert
        Assert.Equal(51m, tax);
    }

    [Fact]
    public void 税込み合計は会計アイテムの合計金額と送料と消費税額の合計となる()
    {
        // Arrange
        var accountItems = new AccountItem[] { new(1, 4500m) };
        var account = new Account(accountItems);

        // Act
        var totalPrice = account.GetTotalPrice();

        // Assert
        Assert.Equal(5500m, totalPrice);
    }
}
