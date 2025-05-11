using enfermeria.api.Data;
using enfermeria.api.Models.Domain;
using enfermeria.api.Repositories.Interface;

namespace enfermeria.api.Repositories.Implementation
{
    public class AvisoRepository : GenericRepository<Aviso>, IAvisoRepository
    {
        public AvisoRepository(DbAb1c8aEnfermeriaContext context) : base(context)
        {
        }
    }
}
