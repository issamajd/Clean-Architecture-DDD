﻿using Autofac;
using Twinkle.Auditing.AuditLogs;
using Twinkle.SeedWork.Auditing;

namespace Twinkle.Auditing;

public class AuditingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EfCoreAuditLogRepository>()
            .As<IAuditLogRepository>();
        
        builder.RegisterType<AuditLogDataToAuditLogMapper>()
            .AsSelf();
        
        builder.RegisterType<AuditLogStore>()
            .As<IAuditLogStore>()
            .IfNotRegistered(typeof(IAuditLogStore));
        
        builder.RegisterType<AuditingManager>()
            .As<IAuditingManager>()
            .IfNotRegistered(typeof(IAuditingManager));
    }
}