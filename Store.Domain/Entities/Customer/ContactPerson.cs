using Store.Domain.Entities;

namespace Store.Domain
{
    public class ContactPerson : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }
}
