﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Employees;

namespace WebStore.DAL.Context
{
    public class WebStoreDB : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public WebStoreDB(DbContextOptions<WebStoreDB> Options) : base(Options) { }


    }
}
