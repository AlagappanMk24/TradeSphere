namespace TradeSphere.Application.UseCases
{
    public class CategoryUseCase(ICategoryRepository categoryRepository, ILogger<CategoryUseCase> logger) : ICategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly ILogger<CategoryUseCase> _logger = logger;
        public async Task<List<CategoryListDto>> GetAllCategory()
        {
            var categories = await _categoryRepository.GetAllCategory();
            return categories;
        }
        public async Task<CategoryListDto> GetById(int id)
        {
            var category = await _categoryRepository.GetById(id);
            return category;
        }
        public async Task<CategoryListDto> GetByName(string name)
        {
            try
            {
                var category = await _categoryRepository.GetByName(name);
                if (category == null)
                    _logger.LogInformation("Category with name {Name} not found in UseCase", name);

                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UseCase while getting category with name {Name}", name);
                throw;
            }
        }
        public async Task<bool> DeleteCategory(int id)
        {
            var deleteCategory = await _categoryRepository.DeleteCategory(id);
            if (!deleteCategory)
            {
                _logger.LogInformation($"There is No Category With This Id {id}");
                return false;
            }
            return deleteCategory;
        }
        public async Task<CategoryListDto> AddCategory(CategoryAddDto categoryAddDto)
        {
            var addCategory = await _categoryRepository.AddCategory(categoryAddDto);
            if (addCategory is null)
            {
                _logger.LogInformation($"Cant Add Category");
                return null;
            }
            return addCategory;
        }
        public async Task<CategoryListDto> UpdateCategory(int id, CategoryAddDto categoryAddDto)
        {
            var updatedCategory = await _categoryRepository.UpdateCategory(id, categoryAddDto);
            if (updatedCategory is null)
            {
                _logger.LogInformation($"Cant updatedCategory");
                return null;
            }
            return updatedCategory;
        }
    }
}
