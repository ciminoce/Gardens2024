using Gardens2024.Entities.Entities;
using System.Linq.Expressions;

namespace Gardens2024.Services.Interfaces
{
    public interface IApplicationUsersService
    {
        IEnumerable<ApplicationUser>? GetAll(Expression<Func<ApplicationUser, bool>>? filter = null,
            Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>>? orderBy = null,
            string? propertiesNames = null);
        void Save(ApplicationUser applicationUser);
        void Delete(ApplicationUser applicationUser);
        ApplicationUser? Get(Expression<Func<ApplicationUser, bool>>? filter = null,
            string? propertiesNames = null, 
            bool tracked=true);

    }
}
