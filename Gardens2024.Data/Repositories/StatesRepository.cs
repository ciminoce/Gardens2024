using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Repositories
{
    public class StatesRepository : GenericRepository<State>, IStatesRepository
    {
        private readonly Gardens2024DbContext? _db;

        public StatesRepository(Gardens2024DbContext db):base(db)
        {
            _db = db??throw new ArgumentException("Dependencies not set");
        }

        public bool Exist(State state)
        {
            if(state.StateId == 0)
            {
                return _db!.States.Any(s => s.StateName == state.StateName
                    && s.CountryId == state.CountryId);

            }
            return _db!.States.Any(s => s.StateName == state.StateName
                    && s.CountryId == state.CountryId
                    && s.StateId!=state.StateId);
        }

        public bool ItsRelated(int id)
        {
            return _db!.Cities.Any(c=>c.StateId == id);
        }

        public void Update(State state)
        {
            _db!.States.Update(state);
        }
    }
}
