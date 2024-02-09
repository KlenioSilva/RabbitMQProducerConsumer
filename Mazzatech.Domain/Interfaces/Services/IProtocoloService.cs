using Mazzatech.Domain.EntitiesModels;

namespace Mazzatech.Domain.Interfaces.Services
{
    public interface IProtocoloService : IServiceBase<ProtocoloEntityModel>
    {
        void AddWithoutReturn(ProtocoloEntityModel entity);
        Task<ProtocoloEntityModel> GetByProtocolo(Guid? protocolo);
        Task<ProtocoloEntityModel> GetByCPF(string? cpf);
        Task<ProtocoloEntityModel> GetByRG(string? rg);
    }
}
