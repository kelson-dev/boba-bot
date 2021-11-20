namespace Boba.Shop
{
    public interface IDebitable<TSelf> where TSelf : IDebitable<TSelf>
    {
        decimal Cash { get; }
        bool TryDebit<TCredit>(decimal amount, TCredit to, out TSelf updated_debit, out TCredit credited) where TCredit : ICreditable<TCredit>;
    }
}