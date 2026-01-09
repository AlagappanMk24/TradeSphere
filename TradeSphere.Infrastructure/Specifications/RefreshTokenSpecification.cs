using System.Linq.Expressions;

namespace TradeSphere.Infrastructure.Specifications
{
    public class RefreshTokenSpecification : BaseSpecification<RefreshToken>
    {
        public RefreshTokenSpecification()
        {
            AddInculdes();
        }
        public RefreshTokenSpecification(int id) : base(r => r.Id == id)
        {
            AddInculdes();
        }
        public RefreshTokenSpecification(Expression<Func<RefreshToken, bool>> criteria) : base(criteria)
        {
            AddInculdes();
        }
        void AddInculdes()
        {
            Includes.Add(r => r.AppUser);

        }
    }
}