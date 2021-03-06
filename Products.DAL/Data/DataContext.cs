﻿using Microsoft.EntityFrameworkCore;
using Products.DAL.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.DAL.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPayload> Payloads { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>()
                            .HasKey(k => k.Code);
            
            builder.Entity<Vendor>().Property(n => n.Id).ValueGeneratedOnAdd();
            builder.Entity<Vendor>()
                .HasKey(k => k.Code);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var configuration = new ConfigurationBuilder()
            //      .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            //      .AddJsonFile("appsettings.json")
            //      .Build();

            optionsBuilder.UseSqlServer("Server=localhost;Database=ProductsDatabase;Trusted_Connection=True;user=DESKTOP-EB31SD2\\btsan");
        }
    }
}
