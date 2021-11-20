namespace Boba.Shop
{
    public interface ICreditable<TSelf> where TSelf : ICreditable<TSelf>
    {
        decimal Cash { get; }
        TSelf WithCreditted(decimal amount);
    }
}