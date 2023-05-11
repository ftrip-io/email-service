using EmailService.Constracts;
using EmailService.Models;
using ftrip.io.email_service.ContactsList;
using ftrip.io.notification_service.contracts.Notifications.Events;
using MassTransit;
using System.Threading.Tasks;

namespace ftrip.io.email_service.Emailing
{
    public class NotificationEmailSender : IConsumer<NotificationSavedEvent>
    {
        private readonly IContactRepository _contactRepository;
        private readonly ITemplatesProvider _templatesProvider;
        private readonly IEmailDispatcher _emailDispatcher;

        public NotificationEmailSender(
            IContactRepository contactRepository,
            ITemplatesProvider templatesProvider,
            IEmailDispatcher emailDispatcher)
        {
            _contactRepository = contactRepository;
            _templatesProvider = templatesProvider;
            _emailDispatcher = emailDispatcher;
        }

        public async Task Consume(ConsumeContext<NotificationSavedEvent> context)
        {
            var notificationSaved = context.Message;

            var contact = await _contactRepository.ReadByUserId(notificationSaved.UserId);
            if (contact == null)
            {
                return;
            }

            var email = _templatesProvider.FromTemplate<Email>("NewNotification", new
            {
                To = contact.Email,
                contact.Name
            });

            _emailDispatcher.Dispatch(email);
        }
    }
}