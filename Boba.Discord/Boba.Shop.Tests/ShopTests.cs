using FluentAssertions;
using System.Collections.Immutable;
using Xunit;

namespace Boba.Shop.Tests
{
    public class ShopTests
    {
        [Fact]
        public void BuyingItem_Should_ReduceCustomerCash()
        {
            // assemble
            var customer = new Customer(Cash: 100.00M);
            var egg_item = new MenuItem("Egg", 1.00M, 10);
            var shop = new BobaShop(
                Name: "Test Shop",
                Items: ImmutableArray<MenuItem>.Empty.Add(egg_item), 
                Cash: 100.00M);

            // act
            var (updated_shop, updated_customer) = shop.Purchase("Egg", 1, customer);

            // assert
            updated_customer.Cash.Should().Be(customer.Cash - egg_item.Price);
            updated_shop.Cash.Should().Be(shop.Cash + egg_item.Price);
            updated_shop.Items[0].Inventory.Should().Be(shop.Items[0].Inventory - 1);
        }
    }
}