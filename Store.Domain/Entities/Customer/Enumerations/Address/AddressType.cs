namespace Store.Domain.Entities.Enumerations
{
    public enum AddressTypeEnum
    {
        Shipping,
        Billing
    }

    public class AddressType : EnumerationEntity<AddressTypeEnum> { }   
}