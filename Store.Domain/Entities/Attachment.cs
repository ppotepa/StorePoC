﻿using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Store.Domain.Entities
{
    public class Attachment : Entity
    {
        public virtual RegardingObject RegardingObject { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public string Extension => FileName.Split('.').Last();
    }

    public class RegardingObject : Entity
    {
        public RegardingObject()
        {
        }

        public string EntityName { get; set; }
        public Guid RegardingObjectId { get; set; }

        [NotMapped]
        public Type EntityType
            => Type.GetType($"{EntitiesAssemblyNamespace}.{this.EntityName}", true);
    
        public static TEntity Find<TEntity>(DbContext context, Guid Id) where TEntity : Entity
        {
            TEntity @object = context.Find(typeof(TEntity), Id) as TEntity;
            return @object;
        }
    }

    [NotMapped]
    public class RegardingObject<TEntity> : Entity where TEntity : Entity
    {
        public readonly TEntity Entity;

        public RegardingObject(TEntity Entity)
        {
            this.Entity = Entity;
        }
    }
}
