using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Interfaces
{
    public interface IApplicationUsersRepository:IGenericRepository<ApplicationUser>
    {
        void Update(ApplicationUser applicationUser);
    }
}
