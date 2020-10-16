using System.Data.Entity;


// assert Cars.CarDb Table exists
namespace Cars
{
    public class CarDb : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public CarDb(string connectionSring) : base(connectionSring)
        {}
    }
}