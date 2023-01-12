using Store.Domain.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Entities
{
    public partial class Customer : User
    {
    

        public Customer() { }
        public Customer
        (
             string companyName,
             CompanySizeType companySizeType,
             List<Address> addresses,    
             string vatNumber,
             string website,
             string registrationNumber             
        )
        {
            CompanyName = companyName;
            CompanySizeType = companySizeType;
            Addresses = addresses;            
            VATNumber = vatNumber;
            Website = website;
            RegistrationNumber = registrationNumber;            
        }

        public virtual ICollection<Address> Addresses { get; set; }        
        public string CompanyName { get; set; }
        public virtual CompanySizeType CompanySizeType { get; set; }
        public bool IsActive { get; set; }
        public override string Name
        {
            get => CompanyName;
            set => Name = value;
        }

        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public string PasswordConfirmation { get; set; }

        public string PasswordHash { get; set; }
        public virtual Registration Registration { get; set; }
        public string RegistrationNumber { get; set; }
        public string VATNumber { get; set; }
        public string Website { get; set; }

        #region FOREIGN_KEYS
        public Guid CompanySizeTypeId { get; set; }
        #endregion
    }
}
