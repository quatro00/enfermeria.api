using enfermeria.api.Models;
using enfermeria.api.Models.DTO.Estado;

namespace enfermeria.api.Repositories.Interface
{
    public interface IEstadoRepository
    {
        Task<ResponseModel> Get();
        Task<ResponseModel> Get(Guid id);
        Task<ResponseModel> Create(CreateEstado_Request model, string usuarioId);
        Task<ResponseModel> Update(UpdateEstado_Request model, Guid id, string usuarioId);
        Task<ResponseModel> Activar(Guid id);
        Task<ResponseModel> Desactivar(Guid id);
    }
}
