using ftrip.io.user_service.contracts.Users.Events;
using MassTransit;
using System.Threading.Tasks;

namespace ftrip.io.email_service.ContactsList.Consumers
{
    public class UserUpdatedEventConsumer : IConsumer<UserUpdatedEvent>
    {
        private readonly IContactRepository _contactRepository;

        public UserUpdatedEventConsumer(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            var userUpdatedEvent = context.Message;

            var contact = await _contactRepository.ReadByUserId(userUpdatedEvent.UserId);
            if (contact == null)
            {
                return;
            }

            contact.Email = userUpdatedEvent.Email;
            contact.Name = userUpdatedEvent.FirstName + " " + userUpdatedEvent.LastName;

            await _contactRepository.Update(contact);
        }
    }
}