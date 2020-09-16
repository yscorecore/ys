﻿using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace YS.Knife
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class OptionsAttribute : KnifeAttribute
    {
        public OptionsAttribute(string configKey = default) : base(null)
        {
            this.ConfigKey = configKey;
        }
        public string ConfigKey { get; set; }

        public override void RegisteService(IServiceCollection services, IRegisteContext context, Type declareType)
        {
            var method = typeof(OptionsAttribute).GetMethod(nameof(RegisteOptions), BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(declareType);
            method.Invoke(this, new object[] { services, context?.Configuration });
        }
        private void RegisteOptions<T>(IServiceCollection services, IConfiguration configuration)
            where T : class, new()
        {
            var optionsConfiguration = configuration.GetOptionsConfiguration<T>(ConfigKey);
            services.AddOptions<T>().Bind(optionsConfiguration).ValidateDataAnnotations();
            services.AddSingleton<T>(sp => sp.GetRequiredService<IOptions<T>>().Value);
        }
    }
}