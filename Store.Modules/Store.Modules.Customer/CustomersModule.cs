﻿using Autofac;
using FluentValidation;
using MediatR.Pipeline;
using Store.Event;
using Store.Utilities.Abstractions;
using System;
using System.Linq;

namespace Store.Modules.Customers
{
    /// <summary>
    /// Autofac UsersModule
    /// </summary>
    public class CustomersModule : StoreModule
    {
        
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomersController>().PropertiesAutowired();

            Type[] Validators = ThisAssembly.GetTypes().Where(type => type.IsClosedTypeOf(typeof(IValidator<>))).ToArray();
            Type[] Events = ThisAssembly.GetTypes().Where(type => type.IsClosedTypeOf(typeof(IEvent<>))).ToArray();
            Type[] PreProcessors = ThisAssembly.GetTypes().Where(type => type.IsClosedTypeOf(typeof(IRequestPreProcessor<>))).ToArray();
            Type[] PostProcessors = ThisAssembly.GetTypes().Where(type => type.IsClosedTypeOf(typeof(IRequestPostProcessor<,>))).ToArray();

            builder.RegisterTypes(Validators).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterTypes(Events).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterTypes(PreProcessors).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterTypes(PostProcessors).AsImplementedInterfaces().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
