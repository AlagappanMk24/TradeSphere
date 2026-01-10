namespace TradeSphere.Infrastructure.Specifications
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification()
        {
            AddInculdes();
        }
        public OrderSpecification(int id) : base(r => r.Id == id)
        {
            AddInculdes();
        }
        public OrderSpecification(Expression<Func<Order, bool>> criteria) : base(criteria)
        {
            AddInculdes();
        }

        void AddInculdes()
        {
            Includes.Add(o => o.ApplicationUser);
            Includes.Add(p => p.Payment);
            AddInclude("Payment.ApplicationUser");
            Includes.Add(p => p.OrderItems);
            AddInclude("OrderItems.Product");
        }
    }
}