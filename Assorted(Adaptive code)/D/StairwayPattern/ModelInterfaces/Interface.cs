namespace ModelInterfaces
{
    /// <summary>
    /// Interface and concrete class are in separate assemblies.
    /// Interface does not depend on any implementation class.
    /// </summary>
    public interface IDataProvider
    {
        int GetSingleNumger();
    }
}
