using Mazzatech.Domain.EntitiesModels;
using Mazzatech.Domain.Interfaces.Repositories;
using Mazzatech.Domain.Interfaces.Services;

namespace Mazzatech.Domain.Services
{
    public class DbErrorLoggerService : IDbErrorLoggerService
    {
        private readonly IDbErrorLoggerRepository _dbErrorLoggerRepository;
        public DbErrorLoggerService(IDbErrorLoggerRepository dbErrorLoggerRepository)
        {
            _dbErrorLoggerRepository = dbErrorLoggerRepository;
        }

        public Task<IEnumerable<DbErrorLoggerEntityModel>> Add(DbErrorLoggerEntityModel entity)
        {
            try
            {
                return _dbErrorLoggerRepository.Add(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<DbErrorLoggerEntityModel>> Delete(DbErrorLoggerEntityModel entity)
        {
            try
            {
                return _dbErrorLoggerRepository.Delete(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<DbErrorLoggerEntityModel>> DeleteById(int id)
        {
            try
            {
                return _dbErrorLoggerRepository.DeleteById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DbErrorLoggerEntityModel>> GetAll()
        {
            try
            {
                return _dbErrorLoggerRepository.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<DbErrorLoggerEntityModel> GetById(int id)
        {
            try
            {
                return _dbErrorLoggerRepository.GetById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<DbErrorLoggerEntityModel>> Update(DbErrorLoggerEntityModel entity)
        {
            try
            {
                return _dbErrorLoggerRepository.Update(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
