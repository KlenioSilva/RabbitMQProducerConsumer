using Mazzatech.Domain.EntitiesModels;
using Mazzatech.Domain.Interfaces.Repositories;
using Mazzatech.Domain.Interfaces.Services;

namespace Mazzatech.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IEnumerable<UserEntityModel>> Add(UserEntityModel entity)
        {
            try
            {
                return _userRepository.Add(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<UserEntityModel>> Delete(UserEntityModel entity)
        {
            try
            {
                return _userRepository.Delete(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<UserEntityModel>> DeleteById(int id)
        {
            try
            {
                return _userRepository.DeleteById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _userRepository.Dispose();
        }

        public Task<IEnumerable<UserEntityModel>> GetAll()
        {
            try
            {
                return _userRepository.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<UserEntityModel> GetById(int id)
        {
            try
            {
                return _userRepository.GetById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsValidUser(UserEntityModel userEntityModel)
        {
            try
            {
                return _userRepository.IsValidUser(userEntityModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<UserEntityModel>> Update(UserEntityModel entity)
        {
            try
            {
                return _userRepository.Update(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
