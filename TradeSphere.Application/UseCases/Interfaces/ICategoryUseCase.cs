namespace TradeSphere.Application.UseCases.Interfaces
{
    public interface ICategoryUseCase
    {
        Task<List<CategoryListDto>> GetAllCategory();
        Task<CategoryListDto> GetById(int id);
        Task<CategoryListDto> GetByName(string name);
        Task<bool> DeleteCategory(int id);
        Task<CategoryListDto> AddCategory(CategoryAddDto categoryAddDto);
        Task<CategoryListDto> UpdateCategory(int id, CategoryAddDto categoryAddDto);
    }
}