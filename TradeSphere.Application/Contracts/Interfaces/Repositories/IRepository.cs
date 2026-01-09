using TradeSphere.Application.Contracts.Interfaces.Specification;

namespace TradeSphere.Application.Contracts.Interfaces.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<T> GetByIdSpec(ISpecification<T> spec);
        Task<IEnumerable<T>> GetAllWithSpec(ISpecification<T> spec);
        public Task<T> GetByIdTrackedAsync(int id);
        public Task<T> GetByIdSpecTracked(ISpecification<T> spec);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate);
    }
}
