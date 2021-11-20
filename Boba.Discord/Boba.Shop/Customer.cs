namespace Boba.Shop
{
    public record Customer(decimal Cash) : IDebitable<Customer>
    {
        public bool TryDebit<TTo>(decimal amount, TTo to, out Customer debitted, out TTo credited)
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
    }
}