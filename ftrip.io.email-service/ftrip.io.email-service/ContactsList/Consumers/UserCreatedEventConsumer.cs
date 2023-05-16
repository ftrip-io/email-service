using ftrip.io.email_service.ContactsList.Domain;
using ftrip.io.user_service.contracts.Users.Events;
using MassTransit;
using Serilog;
using System.Threading.Tasks;

namespace ftrip.io.email_service.ContactsList.Consumers
{
    public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly IContactRepository _contactRepository;
        private readonly ILogger _logger;

        public UserCreatedEventConsumer(
            IContactRepository contactRepository,
            ILogger logger)
        {
            _contactRepository = contactRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var userCreatedEvent = context.Message;

            var createdContact = await _contactRepository.Create(new Contact()
            {
                UserId = userCreatedEvent.UserId,
                Email = userCreatedEvent.Email,
                Name = userCreatedEvent.FirstName + " " + userCreatedEvent.LastName
            });

            _logger.Information("Created contact - UserId[{UserId}], Email[{Email}]", createdContact.UserId, createdContact.Email);
        }
    }
}