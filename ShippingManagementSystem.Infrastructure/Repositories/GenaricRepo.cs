using Microsoft.EntityFrameworkCore;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Interfaces.IGenaricRepository;
using ShippingManagementSystem.Infrastructure.Data;
using System.Linq;
using System.Linq.Expressions;

namespace ShippingManagementSystem.Infrastructure.Repositories
{
    public class GenaricRepo<T> : IGenaricRepo<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public GenaricRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(T obj)
        {
            await _context.Set<T>().AddAsync(obj);
        }

        public async void Delete(int id)
        {
            T obj = await GetById(id);
            _context.Set<T>().Remove(obj);
        }

        public Task<List<T>> GetAll()
        {
            return _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById<T2>(T2 id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Update(T obj)
        {
            _context.Set<T>().Update(obj);
        }

        public async Task AddRange(IEnumerable<T> objs)
        {
            await _context.Set<T>().AddRangeAsync(objs);
        }

        public async Task DeleteRange(IEnumerable<T> objs)
        {
            _context.Set<T>().RemoveRange(objs);
           
        }

        public async Task<List<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Any(predicate);
        }


        #region specification

        public async Task<T?> GetBySpecAsync(ISpecification<T> spec)
        {
            ArgumentNullException.ThrowIfNull(spec);
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllBySpecAsync(ISpecification<T> spec)
        {
            ArgumentNullException.ThrowIfNull(spec);
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            ArgumentNullException.ThrowIfNull(spec);
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            var query = _context.Set<T>().AsQueryable();

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            return query;
        }

        #endregion
    }
}
