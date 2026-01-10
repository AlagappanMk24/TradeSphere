namespace TradeSphere.Application.UseCases
{
    public class FeedBackUseCase(IFeedBackRepository feedBackRepository, IMapper mapper) : IFeedBackUseCase
    {
        private readonly IFeedBackRepository _feedBackRepository = feedBackRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<FeedBack> AddFeedBack(FeedBackAddDto inputDto)
        {
            var feedBackEntity = _mapper.Map<FeedBack>(inputDto);
            var createdFeedBack = await _feedBackRepository.AddFeedBack(feedBackEntity) ?? throw new Exception("Failed to create feedback.");
            return createdFeedBack;
        }
        public async Task<List<FeedBackReadDto>> GetProductFeedBackById(int ProductId)
        {
            var feedBack = await _feedBackRepository.GetFeedBacksByProductId(ProductId) ?? throw new Exception("Feedback not found.");
            return _mapper.Map<List<FeedBackReadDto>>(feedBack);
        }
        public async Task<FeedBackReadDto> UpdateFeedBack(int userId, int id, FeedBackUpdateDto updateDto)
        {
            var existingFeedBack = await _feedBackRepository.GetFeedBacksId(id) ?? throw new Exception("Feedback not found.");
            if (existingFeedBack.ApplicationUserId != userId)
                throw new Exception("You not Allowed");

            if (updateDto.Rating < 1 || updateDto.Rating > 5)
                throw new Exception("Rating must be between 1 and 5");
            _mapper.Map(updateDto, existingFeedBack);
            var updatedFeedBack = await _feedBackRepository.UpdateFeedBack(existingFeedBack);
            return _mapper.Map<FeedBackReadDto>(updatedFeedBack);
        }
    }
}