namespace RPS.Domain.Data
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
