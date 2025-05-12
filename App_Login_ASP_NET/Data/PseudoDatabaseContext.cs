using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App_Login_ASP_NET.Models;

namespace App_Login_ASP_NET.Data
{

    public class PseudoDatabaseContext : DbContext
    {

        public PseudoDatabaseContext(DbContextOptions<PseudoDatabaseContext> options) : base(options) {  }

        public DbSet<App_Login_ASP_NET.Models.User> Users { get; set; } = default!;

        public DbSet<App_Login_ASP_NET.Models.Role> Roles { get; set; } = default!;

    }

}