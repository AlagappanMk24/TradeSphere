namespace TradeSphere.Application.Contracts.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : BaseEntity;
        IAuthRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        Task<int> CommitAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}