using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Partner.WorldTel.Did.Api.Models;

namespace Partner.WorldTel.Did.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Partner.WorldTel.Did.Api.Models.InternationalDid> InternationalDid { get; set; } = default!;
    }
}
