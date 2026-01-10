namespace TradeSphere.Application.UseCases.Interfaces
{
    public interface IFeedBackUseCase
    {
        Task<FeedBack> AddFeedBack(FeedBackAddDto inputDto);
        Task<List<FeedBackReadDto>> GetProductFeedBackById(int ProductId);
        Task<FeedBackReadDto> UpdateFeedBack(int userId, int id, FeedBackUpdateDto updateDto);
    }
}
