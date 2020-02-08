﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
    public abstract class OracleDesignTimeDbContextFactory<T> : IDesignTimeDbContextFactory<T>
        where T : DbContext
    {

        public T CreateDbContext(string[] args)
        {
            string basePath = Directory.GetCurrentDirectory();

            string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", true, false);
              
            if (!string.IsNullOrEmpty(envName))
            {
                configurationBuilder.AddJsonFile($"appsettings.{envName}.json", true, false);
            }

           

            configurationBuilder.AddEnvironmentVariables();

            if (args != null)
            {
                configurationBuilder.AddCommandLine(args);
            }
            
            var configuration = configurationBuilder.Build();

            var configurationKey = GetConnectionStringKey();
            var connectionString = configuration.GetConnectionString(configurationKey);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Can not find connection string by key '{configurationKey}'.");
            }
            var options = new DbContextOptionsBuilder<T>()
                    .UseOracle(connectionString)
                    .Options;
            return OnCreateDbContextInstance(options);
        }
        private string GetConnectionStringKey()
        {
            if (Attribute.IsDefined(typeof(T), typeof(OracleDbContextClassAttribute)))
            {
                var attr = Attribute.GetCustomAttribute(typeof(T), typeof(OracleDbContextClassAttribute)) as OracleDbContextClassAttribute;
                if (!string.IsNullOrEmpty(attr.ConnectionStringKey))
                {
                    return attr.ConnectionStringKey;
                }
            }
            return typeof(T).Name;
        }
        protected virtual T OnCreateDbContextInstance(DbContextOptions<T> options)
        {
           var ctor1= typeof(T).GetConstructor(new Type[] { typeof(DbContextOptions<T>) })
                ?? typeof(T).GetConstructor(new Type[] { typeof(DbContextOptions) });
            if (ctor1 != null)
            {
                return ctor1.Invoke(new object[] { options }) as T;
            }
            var ctor_empty = typeof(T).GetConstructor(Type.EmptyTypes);
            if (ctor_empty != null)
            {
                return ctor1.Invoke(new object[0]) as T;
            }
            throw new Exception("Can not create DbContext instance.");
        }

    }
}