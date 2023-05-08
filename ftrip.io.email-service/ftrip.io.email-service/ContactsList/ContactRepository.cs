using ftrip.io.email_service.ContactsList.Domain;
using ftrip.io.framework.Persistence.Contracts;
using ftrip.io.framework.Persistence.NoSql.Mongodb.Repository;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.email_service.ContactsList
{
    public interface IContactRepository : IRepository<Contact, string>
    {
        Task<Contact> ReadByUserId(string userId, CancellationToken cancellationToken = default);
    }

    public class ContactRepository : Repository<Contact, string>, IContactRepository
    {
        public ContactRepository(IMongoDatabase mongoDatabase) :
            base(mongoDatabase)
        {
        }

        public async Task<Contact> ReadByUserId(string userId, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(contact => contact.UserId == userId).FirstOrDefaultAsync(cancellationToken);
        }
    }
}