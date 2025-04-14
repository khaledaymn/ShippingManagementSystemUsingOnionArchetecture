

using System.Linq.Expressions;

namespace ShippingManagementSystem.Domain.Interfaces.IGenaricRepository
{
    public interface IGenaricRepo<T> where T : class
    {
        public Task<List<T>> GetAll();
        public Task<T> GetById<T2>(T2 id);
        public Task Add(T obj);
        public void Delete(int id);
        public void Update(T obj);
        Task AddRange(IEnumerable<T> entities);
        Task DeleteRange(IEnumerable<T> entities);
        Task<List<T>> Find(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
        Task<T?> GetBySpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAllBySpecAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
    }
}
