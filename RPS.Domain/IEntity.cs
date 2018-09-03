namespace RPS.Domain
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
