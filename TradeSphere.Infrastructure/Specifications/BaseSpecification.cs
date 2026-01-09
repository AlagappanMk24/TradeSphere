using System.Linq.Expressions;
using TradeSphere.Application.Contracts.Interfaces.Specification;
using TradeSphere.Domain.Entities.Common;

namespace TradeSphere.Infrastructure.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public BaseSpecification()
        {

        }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<T, bool>>? Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = [];
        public Expression<Func<T, object>>? OrderBy { get; set; }
        public Expression<Func<T, object>>? OrderByDesc { get; set; }
        public List<string> IncludeStrings { get; set; } = [];
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool HasPagination { get; set; }
        protected void AddOrderBy(Expression<Func<T, object>> expression)
            => OrderBy = expression;
        protected void AddOrderByDesc(Expression<Func<T, object>> expression)
            => OrderByDesc = expression;
        protected void AddInclude(string includeString)
            => IncludeStrings.Add(includeString);
        protected void AddPagination(int skip, int take)
        {
            HasPagination = true;
            Skip = skip;
            Take = take;
        }
    }
}