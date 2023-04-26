﻿using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
               
        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Features> Features { get; set; }
    }
}
