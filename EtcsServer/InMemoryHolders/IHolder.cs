namespace EtcsServer.InMemoryHolders
{
    public interface IHolder<T>
    {
        Dictionary<int, T> GetValues();
    }
}
