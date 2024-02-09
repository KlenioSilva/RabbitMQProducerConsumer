using Mazzatech.Domain.EntitiesModels;

namespace Mazzatech.Domain.Interfaces.Services
{
    public interface IIssuerUserSecretKeyTokenService : IServiceBase<IssuerUserSecretKeyTokenEntityModel>
    {
        Task<string> GetTokenById(int id);
    }
}
