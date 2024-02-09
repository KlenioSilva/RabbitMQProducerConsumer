using Mazzatech.Domain.EntitiesModels;

namespace Mazzatech.Domain.Interfaces.Repositories
{
    public interface IIssuerSecretKeyRepository : IRepositoryBase<IssuerSecretKeyEntityModel>
    {
        Task<IssuerSecretKeyEntityModel> GetSecretKeyByIssuer(string issuer);
    }
}
