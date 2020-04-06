﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;

namespace YS.Knife
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class OptionsValidateAttribute : KnifeAttribute
    {
        public OptionsValidateAttribute() : base(typeof(IValidateOptions<>))
        {
        }
        public override void RegisteService(IServiceCollection services, IRegisteContext context, Type declareType)
        {
            _ = declareType ?? throw new ArgumentNullException(nameof(declareType));
            var optionsType = FindOptionsType(declareType);
            services.AddSingleton(typeof(IValidateOptions<>).MakeGenericType(optionsType), declareType);
        }
        private Type FindOptionsType(Type declareType)
        {
            return declareType.GetInterfaces()
                 .Where(p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(IValidateOptions<>))
                 .Select(p => p.GetGenericArguments().First())
                 .FirstOrDefault();
        }
    }
}
