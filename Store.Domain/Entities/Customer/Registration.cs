using System;

namespace Store.Domain.Entities
{
    public class Registration : Entity
    {
        public virtual Customer Customer { get; set; }
        public virtual Guid CustomerId { get; set; }
        public string ActivationCode { get; set; }        
        
    }
}
