﻿using System.Threading;
using System.Threading.Tasks;

namespace YS.Knife.Stage
{
    public abstract class Installer : IStageService
    {
        public string StageName => "install";

        public abstract Task Run(CancellationToken cancellationToken = default);
    }
}