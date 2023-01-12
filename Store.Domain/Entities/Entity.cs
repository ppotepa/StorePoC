using Store.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Store.Domain.Entities
{
    public class Entity
    {
        protected static Assembly CurrentAssembly => Assembly.LoadFrom("Store.Domain.dll");
        protected const string EntitiesAssemblyNamespace = "Store.Domain.Entities";
        protected bool IsProxyType => this.GetType().Name.EndsWith("Proxy");
        private Type proxyUnderlyingType = null;
        private Type ProxyUnderlyingType
        {
            get
            {
                if (IsProxyType is false) return null;
                if (proxyUnderlyingType is null)
                {
                    proxyUnderlyingType = this.GetType().BaseType;
                }
                return proxyUnderlyingType;
            }
        }
        public object this[string propertyName] => _properties[propertyName];
        public bool HasRootEntity => this.GetType().BaseType != null && this.GetType().BaseType != typeof(object);
        private void UpdateProperty(string propertyName, object @value)
        {
            this.Properties[propertyName].SetMethod.Invoke(this, new object[] { @value });
        }

        private Dictionary<string, PropertyInfo> _properties = default;

        [NotMapped]
        internal Dictionary<string, PropertyInfo> Properties
        {
            get
            {
                if (_properties is null)
                {
                    this._properties = this.GetType()
                                                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                    .Where(prop => prop.CanWrite && prop.Name != "Properties")
                                                    .ToDictionary(x => x.Name, x => x);

                }
                return _properties;
            }

            set => this._properties = value;
        }

        public Entity() { }
        [JsonIgnore]
        public Guid CreatedBy { get; set; }        
        [JsonIgnore]
        public DateTime CreatedOn { get; set; }
        [JsonIgnore]
        public bool Deleted { get; set; }
        public Guid Id { get; set; }
        [JsonIgnore]
        public Guid? ModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime? ModifiedOn { get; set; }
        [JsonIgnore]
        public DateTime? DeletedOn { get; set; }

        public virtual void Update(Entity entity)
            => entity.Properties
                .Where(kvp => kvp.Value.GetValue(entity) != null && !ShadowProperties.Contains(kvp.Value.Name))
                .ForEach(x => this.UpdateProperty(x.Key, x.Value.GetValue(entity)));

        private static readonly string[] ShadowProperties = typeof(Entity).GetProperties().Select(p => p.Name).ToArray();

        public void MarkAsDeleted() => this.Deleted = true;
    }
}
