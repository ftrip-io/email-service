using ftrip.io.user_service.contracts.Users.Events;
using MassTransit;
using Serilog;
using System.Threading.Tasks;

namespace ftrip.io.email_service.ContactsList.Consumers
{
    public class UserUpdatedEventConsumer : IConsumer<UserUpdatedEvent>
    {
        private readonly IContactRepository _contactRepository;
        private readonly ILogger _logger;

        public UserUpdatedEventConsumer(
            IContactRepository contactRepository,
            ILogger logger)
        {
            _contactRepository = contactRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            var userUpdatedEvent = context.Message;

            var contact = await _contactRepository.ReadByUserId(userUpdatedEvent.UserId);
            if (contact == null)
            {
                _logger.Warning("Could not update contact because it is not found - UserId[{UserId}]", userUpdatedEvent.UserId);
                return;
            }

            contact.Email = userUpdatedEvent.Email;
            contact.Name = userUpdatedEvent.FirstName + " " + userUpdatedEvent.LastName;

            var updatedContact = await _contactRepository.Update(contact);
            _logger.Information("Updated contact - UserId[{UserId}], Email[{Email}]", updatedContact.UserId, updatedContact.Email);
        }
    }
}