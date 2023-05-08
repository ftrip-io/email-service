using ftrip.io.email_service.ContactsList.Domain;
using ftrip.io.user_service.contracts.Users.Events;
using MassTransit;
using System.Threading.Tasks;

namespace ftrip.io.email_service.ContactsList.Consumers
{
    public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly IContactRepository _contactRepository;

        public UserCreatedEventConsumer(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var userCreatedEvent = context.Message;

            await _contactRepository.Create(new Contact()
            {
                UserId = userCreatedEvent.UserId,
                Email = userCreatedEvent.Email,
                Name = userCreatedEvent.FirstName + " " + userCreatedEvent.LastName
            });
        }
    }
}