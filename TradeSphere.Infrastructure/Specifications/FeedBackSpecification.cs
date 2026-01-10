namespace TradeSphere.Infrastructure.Specifications
{
    public class FeedBackSpecification : BaseSpecification<FeedBack>
    {
        public FeedBackSpecification()
        {
            AddInculdes();
        }
        public FeedBackSpecification(int id) : base(r => r.Id == id)
        {
            AddInculdes();
        }
        public FeedBackSpecification(Expression<Func<FeedBack, bool>> criteria) : base(criteria)
        {
            AddInculdes();
        }
        void AddInculdes()
        {
            Includes.Add(o => o.ApplicationUser);
            Includes.Add(o => o.Product);
        }
    }
}