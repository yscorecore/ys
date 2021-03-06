﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
namespace YS.Knife.Mongo
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class MongoContextAttribute : KnifeAttribute
    {
        public MongoContextAttribute(string dataBaseName) : this(dataBaseName, dataBaseName)
        {
        }
        public MongoContextAttribute(string dataBaseName, string connectionStringKey) : base(typeof(MongoContext))
        {
            if (string.IsNullOrEmpty(dataBaseName) || !Regex.IsMatch(dataBaseName, @"^\w+$"))
            {
                throw new ArgumentException($"Invalid mongo data base name '{dataBaseName}'.");
            }
            this.DataBaseName = dataBaseName;
            this.ConnectionStringKey = connectionStringKey ?? dataBaseName;
        }
        public string DataBaseName { get; }
        public string ConnectionStringKey { get; }

        public bool RegisterEntityStore { get; set; } = true;
        public override void RegisterService(IServiceCollection services, IRegisterContext context, Type declareType)
        {
            _ = declareType ?? throw new ArgumentNullException(nameof(declareType));
            services.AddScoped(declareType, (sp) =>
             {
                 var ctor = declareType.GetConstructor(new Type[] { typeof(IMongoDatabase) });
                 if (ctor == null)
                 {
                     throw new ArgumentException($"Can not find constructor with '{typeof(IMongoDatabase).FullName}' argument in [{declareType.FullName}].");
                 }
                 var clientFactory = sp.GetRequiredService<IMongoClientFactory>();
                 var client = clientFactory.Create(ConnectionStringKey);
                 var database = client.GetDatabase(DataBaseName);
                 return ctor.Invoke(new object[] { database });
             });
            // add mongo context
            services.AddScoped(sp => sp.GetService(declareType) as MongoContext);
            if (RegisterEntityStore)
            {
                AddEntityStoresInternal(services, declareType);
            }
        }


        private static void AddEntityStoresInternal(IServiceCollection services, Type contextType)
        {
            var entityTypes = from p in contextType.GetProperties()
                              let pType = p.PropertyType
                              where pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(IMongoCollection<>)
                              select pType.GetGenericArguments().First();
            foreach (var entityType in entityTypes)
            {
                var storeType = typeof(IEntityStore<>).MakeGenericType(entityType);
                var implType = typeof(MongoEntityStore<,>).MakeGenericType(entityType, contextType);
                services.AddScoped(storeType, implType);
            }
        }
    }
}
