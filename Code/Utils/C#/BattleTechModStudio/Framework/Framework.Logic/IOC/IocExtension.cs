﻿using System.Security.Principal;
using Framework.Interfaces.Injection;

namespace Framework.Logic.IOC
{
    public static class IocExtension
    {
        public static IIdentity Identity => Container.Instance.GetInstance<IIdentity>();
    }
}