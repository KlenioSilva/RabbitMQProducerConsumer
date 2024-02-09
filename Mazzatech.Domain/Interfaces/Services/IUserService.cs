using Mazzatech.Domain.EntitiesModels;

namespace Mazzatech.Domain.Interfaces.Services
{
    public interface IUserService : IServiceBase<UserEntityModel>
    {
        bool IsValidUser(UserEntityModel userEntityModel);
    }
}
