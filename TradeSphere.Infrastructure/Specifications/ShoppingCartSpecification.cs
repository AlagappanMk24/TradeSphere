namespace TradeSphere.Infrastructure.Specifications
{
    public class ShoppingCartSpecification : BaseSpecification<ShoppingCart>
    {
        public ShoppingCartSpecification()
        {
            AddInculdes();
        }
        public ShoppingCartSpecification(int id) : base(r => r.Id == id)
        {
            AddInculdes();
        }
        public ShoppingCartSpecification(Expression<Func<ShoppingCart, bool>> criteria) : base(criteria)
        {
            AddInculdes();
        }

        void AddInculdes()
        {
            Includes.Add(o => o.ApplicationUser);
            Includes.Add(o => o.CartItems);
            AddInclude("CartItems.Product");
        }
    }
}