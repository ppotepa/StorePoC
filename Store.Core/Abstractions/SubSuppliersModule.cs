using System;
using System.Reflection;

namespace Store.Utilities.Abstractions
{
    public abstract class StoreModule : Autofac.Module
    {
        protected override Assembly ThisAssembly => Assembly.GetCallingAssembly();
    }
}
