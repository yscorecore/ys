﻿using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace YS.EntityFrameworkCore.MySql.UnitTest.TestData
{
    [MySqlDbContextClass("TestContext")]
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
    }
}
