using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Repositories
{
    public class SuppliersRepository : GenericRepository<Supplier>, ISuppliersRepository
    {
        private readonly Gardens2024DbContext _db;

        public SuppliersRepository(Gardens2024DbContext db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public bool Exist(Supplier supplier)
        {

            if (supplier.SupplierId == 0)
            {
                return _db.Suppliers
                    .Any(c => c.SupplierName == supplier.SupplierName);
            }
            return _db.Suppliers
                .Any(c => c.SupplierName == supplier.SupplierName 
                && c.SupplierId != supplier.SupplierId);
        }


        public bool ItsRelated(int id)
        {
            return _db.Products.Any(p => p.SupplierId == id);
        }

        public void Update(Supplier supplier)
        {

            _db.Suppliers.Update(supplier);

        }
    }
}
