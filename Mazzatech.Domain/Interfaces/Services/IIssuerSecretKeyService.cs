using Mazzatech.Domain.EntitiesModels;

namespace Mazzatech.Domain.Interfaces.Services
{
    public interface IIssuerSecretKeyService : IServiceBase<IssuerSecretKeyEntityModel>
    {
        Task<IssuerSecretKeyEntityModel> GetSecretKeyByIssuer(string issuer);
    }
}
