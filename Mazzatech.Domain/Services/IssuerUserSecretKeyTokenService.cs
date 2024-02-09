using Mazzatech.Domain.EntitiesModels;
using Mazzatech.Domain.Interfaces.Repositories;
using Mazzatech.Domain.Interfaces.Services;

namespace Mazzatech.Domain.Services
{
    public class IssuerUserSecretKeyTokenService : IIssuerUserSecretKeyTokenService
    {
        private readonly IIssuerUserSecretKeyTokenRepository _issuerUserSecretKeyTokenRepository;
        public IssuerUserSecretKeyTokenService(IIssuerUserSecretKeyTokenRepository issuerUserSecretKeyTokenRepository)
        {
            _issuerUserSecretKeyTokenRepository = issuerUserSecretKeyTokenRepository;
        }

        public Task<IEnumerable<IssuerUserSecretKeyTokenEntityModel>> Add(IssuerUserSecretKeyTokenEntityModel entity)
        {
            try
            {
                return _issuerUserSecretKeyTokenRepository.Add(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<IssuerUserSecretKeyTokenEntityModel>> Delete(IssuerUserSecretKeyTokenEntityModel entity)
        {
            try
            {
                return _issuerUserSecretKeyTokenRepository.Delete(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<IssuerUserSecretKeyTokenEntityModel>> DeleteById(int id)
        {
            try
            {
                return _issuerUserSecretKeyTokenRepository.DeleteById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _issuerUserSecretKeyTokenRepository.Dispose();
        }

        public Task<IEnumerable<IssuerUserSecretKeyTokenEntityModel>> GetAll()
        {
            try
            {
                return _issuerUserSecretKeyTokenRepository.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IssuerUserSecretKeyTokenEntityModel> GetById(int id)
        {
            try
            {
                return _issuerUserSecretKeyTokenRepository.GetById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<string> GetTokenById(int id)
        {
            try
            {
                return _issuerUserSecretKeyTokenRepository.GetTokenById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<IssuerUserSecretKeyTokenEntityModel>> Update(IssuerUserSecretKeyTokenEntityModel entity)
        {
            try
            {
                return _issuerUserSecretKeyTokenRepository.Update(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
