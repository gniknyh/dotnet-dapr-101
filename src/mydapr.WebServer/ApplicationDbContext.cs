using Microsoft.EntityFrameworkCore;

namespace Link.Mydapr.WebServer;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }
}
