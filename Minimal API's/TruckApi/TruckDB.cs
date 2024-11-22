using Microsoft.EntityFrameworkCore;

class TruckDB : DbContext
{
    public TruckDB(DbContextOptions<TruckDB> options)
        : base(options) { }

    public DbSet<Truck> Trucks => Set<Truck>();
}