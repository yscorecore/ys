﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using YS.Knife.Extensions.Configuration.Api;

namespace Microsoft.Extensions.Configuration
{
    public static class ApiConfigurationExtensions
    {
        public static IConfigurationBuilder AddApiConfiguration(
            this IConfigurationBuilder builder, Action<ApiConfigurationSource> action)
        {
            var source = new ApiConfigurationSource();

            action?.Invoke(source);
            return builder.Add(source);
        }
        public static IConfigurationBuilder AddApiConfiguration(this IConfigurationBuilder builder, string apiUrl)
        {
            return AddApiConfiguration(builder, (apiSource) =>
            {
                apiSource.ApiUrl = apiUrl;
            });
        }
    }
}