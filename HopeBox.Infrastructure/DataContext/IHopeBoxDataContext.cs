using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HopeBox.Infrastructure.DataContext
{
    public interface IHopeBoxDataContext
    {
        IModel Model { get; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync();
        DatabaseFacade Database { get; }
    }
}