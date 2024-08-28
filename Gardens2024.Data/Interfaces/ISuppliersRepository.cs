using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Interfaces
{
    public interface ISuppliersRepository:IGenericRepository<Supplier>
    {
        void Update(Supplier supplier);
        bool Exist(Supplier supplier);
        bool ItsRelated(int id);

    }
}
