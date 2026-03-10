using Microsoft.EntityFrameworkCore;
using vlf_4rum.Models;

namespace vlf_4rum.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}