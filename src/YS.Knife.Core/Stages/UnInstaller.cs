﻿using System.Threading;
using System.Threading.Tasks;

namespace YS.Knife.Stages
{
    public abstract class UnInstaller : IStageService
    {
        public string StageName => "install";
        public virtual string EnvironmentName => "*";
        public abstract Task Run(CancellationToken cancellationToken = default);
    }
}