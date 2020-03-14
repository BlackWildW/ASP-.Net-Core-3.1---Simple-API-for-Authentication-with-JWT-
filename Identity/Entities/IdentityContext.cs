using Identity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity
{
    public class IdentityContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public IdentityContext(DbContextOptions<IdentityContext> dbContext) : base(dbContext) { }
    }
}
