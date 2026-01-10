namespace TradeSphere.Infrastructure.Repositories.FeedBackRepository
{
    public class FeedBackRepository(IUnitOfWork unitOfWork) : IFeedBackRepository
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<FeedBack> AddFeedBack(FeedBack feedBack)
        {
            await _unitOfWork.Repository<FeedBack>().AddAsync(feedBack);
            return await _unitOfWork.CommitAsync() > 0 ? feedBack : null!;
        }
        public async Task<bool> DeleteFeedBack(FeedBack feedBack)
        {
            _unitOfWork.Repository<FeedBack>().Delete(feedBack);
            return await _unitOfWork.CommitAsync() > 0;
        }
        public async Task<IEnumerable<FeedBack>> GetFeedBacksByProductId(int productId)
        {
            var feedBack = await _unitOfWork.Repository<FeedBack>().GetAllWithSpec(new FeedBackSpecification(f => f.ProductId == productId));
            if (feedBack is null) return null;
            return feedBack;
        }
        public Task<FeedBack> GetFeedBacksId(int id)
        {
            var feedBack = _unitOfWork.Repository<FeedBack>().GetByIdAsync(id);
            return feedBack;
        }
        public async Task<FeedBack> UpdateFeedBack(FeedBack feedBack)
        {
            _unitOfWork.Repository<FeedBack>().Update(feedBack);
            return await _unitOfWork.CommitAsync() > 0 ? feedBack : null!;
        }
    }
}