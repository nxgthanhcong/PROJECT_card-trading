﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NETCore_CardTrading.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore_CardTrading.Areas.Admin.Data
{
    public class DataProviderContext : IdentityDbContext<User>
    {
        public DataProviderContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
    }
}
