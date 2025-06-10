using HopeBox.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace HopeBox.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IHopeBoxDataContext _context;

        public Repository(IHopeBoxDataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<TResult?> GetOneAsyncUntracked<TResult>(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>>? orderBy = null,
            Expression<Func<T, TResult>>? selector = null, Expression<Func<IQueryable<T>,
            IQueryable<T>>>? include = null)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (include != null)
            {
                query = include.Compile()(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy.Compile()(query);
            }

            if (selector != null)
            {
                return await query.Select(selector).FirstOrDefaultAsync();
            }
            else
            {
                return await query.Cast<TResult>().FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<TResult>> GetListAsyncUntracked<TResult>(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>>? orderBy = null,
            Expression<Func<T, TResult>>? selector = null,
            Expression<Func<IQueryable<T>, IQueryable<T>>>? include = null,
            int? pageSize = null,
            int? pageNumber = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include.Compile()(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy.Compile()(query);
            }

            if (pageSize.HasValue && pageNumber.HasValue)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            if (selector != null)
            {
                return await query.Select(selector).ToListAsync();
            }
            else
            {
                return await query.Cast<TResult>().ToListAsync();
            }
        }

        public async Task<T?> GetOneAsync(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>>? orderBy = null,
            Expression<Func<IQueryable<T>, IQueryable<T>>>? include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include.Compile()(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy.Compile()(query);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetListAsync(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>>? orderBy = null,
            Expression<Func<IQueryable<T>, IQueryable<T>>>? include = null,
            int? pageSize = null,
            int? pageNumber = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include.Compile()(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy.Compile()(query);
            }

            if (pageSize.HasValue && pageNumber.HasValue)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.RollbackAsync();
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public async Task<int> GetCount(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }
    }
}
