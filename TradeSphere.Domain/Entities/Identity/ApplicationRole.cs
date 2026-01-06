namespace TradeSphere.Domain.Entities.Identity
{
    public class ApplicationRole : IdentityRole<int>
    {
        public string Description { get; set; }
    }
}