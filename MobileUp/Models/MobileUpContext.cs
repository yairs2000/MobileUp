using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileUp.Models
{
    public class MobileUpContext : DbContext
    {
        public MobileUpContext(DbContextOptions<MobileUpContext> options)
            : base(options)
        {
        }

        public DbSet<Listings> Listings { get; set; }
    }
}
