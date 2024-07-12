using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Interfaces
{
    public interface IStatesRepository:IGenericRepository<State>
    {
        void Update(State state);
        bool Exist(State state);
        bool ItsRelated(int id);

    }
}
