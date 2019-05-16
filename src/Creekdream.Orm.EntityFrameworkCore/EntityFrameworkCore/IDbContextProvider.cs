namespace Creekdream.Orm.EntityFrameworkCore
{
    /// <summary>
    /// DbContext provider
    /// </summary>
    public interface IDbContextProvider
    {
        /// <summary>
        /// Get data context operation object
        /// </summary>
        DbContextBase GetDbContext();
    }
}

