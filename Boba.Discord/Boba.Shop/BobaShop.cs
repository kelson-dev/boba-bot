using System.Collections.Immutable;

namespace Boba.Shop
{
    public record BobaShop(string Name, ImmutableArray<MenuItem> Items, decimal Cash) : IDebitable<BobaShop>, ICreditable<BobaShop>
    {
        public (BobaShop, T) Purchase<T>(string itemName, int count, T debitable)
            where T : IDebitable<T>
        {
            if (Items.SingleOrDefault(item => item.Name == itemName) is MenuItem found)
            {
                if (found.Inventory >= count)
                {
                    decimal price = found.Price * count;
                    if (debitable.TryDebit(price, this, out debitable, out var shop))
                    {
                        shop = shop with
                        {
                            Items = Items.Replace(found, found with { Inventory = found.Inventory - count })
                        };
                        return (shop, debitable);
                    }
                }
            }

            return (this, debitable);
        }

        public bool TryDebit<TTo>(decimal amount, TTo to, out BobaShop debitted, out TTo credited)
            where TTo : ICreditable<TTo>
        {
            if (amount <= Cash)
            {
                (debitted, credited) = (this with { Cash = Cash - amount }, to.WithCreditted(amount));
                return true;
            }
            (debitted, credited) = (this, to);
            return false;
        }

        public BobaShop WithCreditted(decimal amount) => this with { Cash = Cash + amount };
    }
}