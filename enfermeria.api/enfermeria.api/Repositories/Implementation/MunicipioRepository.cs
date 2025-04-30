using enfermeria.api.Data;
using enfermeria.api.Models.Domain;
using enfermeria.api.Repositories.Interface;

namespace enfermeria.api.Repositories.Implementation
{
    public class MunicipioRepository : GenericRepository<CatMunicipio>, IMunicipioRepository
    {
        public MunicipioRepository(DbAb1c8aEnfermeriaContext context) : base(context)
        {
        }
    }
}
