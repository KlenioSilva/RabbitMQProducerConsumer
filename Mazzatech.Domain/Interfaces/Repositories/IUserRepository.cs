using Mazzatech.Domain.EntitiesModels;

namespace Mazzatech.Domain.Interfaces.Repositories
{

    public interface IUserRepository : IRepositoryBase<UserEntityModel>
    {
        bool IsValidUser(UserEntityModel userEntityModel);
    }
}
