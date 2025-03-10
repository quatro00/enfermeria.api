using enfermeria.api.Models;
using enfermeria.api.Models.DTO.TipoEnfermera;

namespace enfermeria.api.Repositories.Interface
{
    public interface ITipoEnfermeraRepository
    {
        Task<ResponseModel> Get();
        Task<ResponseModel> GetActivos();
        Task<ResponseModel> Get(Guid id);
        Task<ResponseModel> Create(CreateTipoEnfermera_Request model, string usuarioId);
        Task<ResponseModel> Update(UpdateTipoEnfermera_Request model, Guid id, string usuarioId);
        Task<ResponseModel> Activar(Guid id);
        Task<ResponseModel> Desactivar(Guid id);
    }
}
