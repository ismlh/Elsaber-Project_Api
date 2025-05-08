







using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAccessLayer.Data
{
    public class ApplicationDbContext :IdentityDbContext<ApplicationUser>
    {
       

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<Images> Images { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Services> Services { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<ClientOrders> ClientOrders { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
    }
  
}
