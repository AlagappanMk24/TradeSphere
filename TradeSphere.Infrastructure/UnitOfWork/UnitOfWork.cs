namespace TradeSphere.Infrastructure.UnitOfWork
{
    public class UnitOfWork(TradeSphereDbContext context, UserManager<ApplicationUser> userManger, IAuthRepository user) : IUnitOfWork
    {
        private readonly Dictionary<string, object> repositories = new();
        private IRefreshTokenRepository? _refreshTokenRepository;
        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            var key = typeof(T).Name;
            if (!repositories.TryGetValue(key, out object? value))
            {
                var repo = new GenericRepository<T>(context);
                value = repo;
                repositories.Add(key, value);
            }

            return (IRepository<T>)value;
        }
        public IAuthRepository Users
        {
            get
            {
                if (user == null)
                    user = new AuthRepository(userManger, null, null);

                return user;
            }
        }
        public IRefreshTokenRepository RefreshTokens
        {
            get
            {
                _refreshTokenRepository ??= new RefreshTokenRepository(this);
                return _refreshTokenRepository;
            }
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync() => await context.Database.BeginTransactionAsync();
        public async Task<int> CommitAsync() => await context.SaveChangesAsync();
        public void Dispose() => context.Dispose();
    }
}