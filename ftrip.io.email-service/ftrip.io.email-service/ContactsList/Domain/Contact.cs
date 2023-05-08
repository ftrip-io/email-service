using ftrip.io.framework.Domain;

namespace ftrip.io.email_service.ContactsList.Domain
{
    public class Contact : Record
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}