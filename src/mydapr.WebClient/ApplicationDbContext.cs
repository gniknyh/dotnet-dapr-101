using Microsoft.EntityFrameworkCore;

namespace Link.Mydapr.WebClient;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }
}
