namespace TradeSphere.Infrastructure.Repositories.ShoppingCartRepository
{
    public class ShoppingCartRepository(IUnitOfWork unitOfWork) : IShoppingCartRepository
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ShoppingCart> AddShoppingCart(ShoppingCart shoppingCart)
        {
            await _unitOfWork.Repository<ShoppingCart>().AddAsync(shoppingCart);
            return await _unitOfWork.CommitAsync() > 0 ? shoppingCart : null!;
        }
        public async Task<bool> DeleteShoppingCart(ShoppingCart shoppingCart)
        {
            _unitOfWork.Repository<ShoppingCart>().Delete(shoppingCart);
            return await _unitOfWork.CommitAsync() > 0 ? true : false;
        }
        public async Task<ShoppingCart> GetById(int id)
        {
            var shoppingCart = await _unitOfWork.Repository<ShoppingCart>().GetByIdAsync(id);
            return shoppingCart;
        }
        public async Task<ShoppingCart> GetByIdTracked(int id)
        {
            var shoppingCart = await _unitOfWork.Repository<ShoppingCart>().GetByIdSpecTracked(new ShoppingCartSpecification(id));
            return shoppingCart;
        }
        public Task<ShoppingCart> GetShoppingCartByUser(int userId)
        {
            var shoppingCart = _unitOfWork.Repository<ShoppingCart>().GetByIdSpecTracked(new ShoppingCartSpecification(s => s.ApplicationUserId == userId));
            return shoppingCart;
        }
        public async Task<ShoppingCart> UpdateShoppingCart(ShoppingCart shoppingCart)
        {
            _unitOfWork.Repository<ShoppingCart>().Update(shoppingCart);
            return await _unitOfWork.CommitAsync() > 0 ? shoppingCart : null!;
        }
    }
}
