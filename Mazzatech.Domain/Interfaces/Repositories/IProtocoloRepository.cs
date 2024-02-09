using Mazzatech.Domain.EntitiesModels;

namespace Mazzatech.Domain.Interfaces.Repositories
{
    public interface IProtocoloRepository : IRepositoryBase<ProtocoloEntityModel>
    {
        void AddWithoutReturn(ProtocoloEntityModel entity);
        Task<ProtocoloEntityModel> GetByProtocolo(Guid? protocolo);
        Task<ProtocoloEntityModel> GetByCPF(string? cpf);
        Task<ProtocoloEntityModel> GetByRG(string? rg);
    }
}
