namespace Creekdream.Uow
{
    /// <summary>
    /// Transaction api container
    /// </summary>
    public interface ITransactionApiContainer
    {
        /// <summary>
        /// Find transaction api
        /// </summary>
        ITransactionApi FindTransactionApi();

        /// <summary>
        /// Add transaction api
        /// </summary>
        void AddTransactionApi(ITransactionApi api);
    }
}
