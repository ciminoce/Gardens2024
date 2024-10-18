using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Repositories
{
    public class ApplicationUsersRepository : GenericRepository<ApplicationUser>, IApplicationUsersRepository
    {
        private readonly Gardens2024DbContext _db;

        public ApplicationUsersRepository(Gardens2024DbContext db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public void Update(ApplicationUser applicationUser)
        {

            _db.ApplicationUsers.Update(applicationUser);

        }
    }
}
