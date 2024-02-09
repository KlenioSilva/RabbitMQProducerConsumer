using Mazzatech.Domain.EntitiesModels;

namespace Mazzatech.Domain.Interfaces.Repositories
{
    public interface IIssuerUserSecretKeyTokenRepository : IRepositoryBase<IssuerUserSecretKeyTokenEntityModel>
    {
        Task<string> GetTokenById(int id);
    }
}
