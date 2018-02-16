﻿using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using AutoMapper;
using Lykke.Service.Affiliate.AutoMapper;

namespace Lykke.Service.Affiliate.Modules
{
    internal sealed class AutoMapperModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DomainAutoMapperProfile>()
                .As<Profile>();
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>()
                    .CreateMapper(c.Resolve))
                .As<IMapper>()
                .SingleInstance();
        }
    }
}
