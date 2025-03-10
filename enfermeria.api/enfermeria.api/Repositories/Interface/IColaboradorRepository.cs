using enfermeria.api.Models;
using enfermeria.api.Models.DTO.Colaborador;

namespace enfermeria.api.Repositories.Interface
{
    public interface IColaboradorRepository
    {
        Task<ResponseModel> Get();
        Task<ResponseModel> Get(Guid id);
        Task<ResponseModel> Create(CreateColaborador_Request model, string usuarioId);
        Task<ResponseModel> Update(UpdateColaborador_Request model, Guid id, string usuarioId);
        Task<ResponseModel> Activar(Guid id);
        Task<ResponseModel> Desactivar(Guid id);
    }
}
