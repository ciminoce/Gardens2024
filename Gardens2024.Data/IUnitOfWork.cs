namespace Gardens2024.Data
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        int SaveChanges();
    }
}
