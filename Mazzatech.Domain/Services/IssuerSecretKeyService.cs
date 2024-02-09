using Mazzatech.Domain.EntitiesModels;
using Mazzatech.Domain.Interfaces.Repositories;
using Mazzatech.Domain.Interfaces.Services;

namespace Mazzatech.Domain.Services
{
    public class IssuerSecretKeyService : IIssuerSecretKeyService
    {
        private readonly IIssuerSecretKeyRepository _issuerUserSecretKeyRepository;
        public IssuerSecretKeyService(IIssuerSecretKeyRepository issuerUserSecretKeyRepository)
        {
            _issuerUserSecretKeyRepository = issuerUserSecretKeyRepository;
        }

        public Task<IEnumerable<IssuerSecretKeyEntityModel>> Add(IssuerSecretKeyEntityModel entity)
        {
            try
            {
                return _issuerUserSecretKeyRepository.Add(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<IssuerSecretKeyEntityModel>> Delete(IssuerSecretKeyEntityModel entity)
        {
            try
            {
                return _issuerUserSecretKeyRepository.Delete(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<IssuerSecretKeyEntityModel>> DeleteById(int id)
        {
            try
            {
                return _issuerUserSecretKeyRepository.DeleteById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _issuerUserSecretKeyRepository.Dispose();
        }

        public Task<IEnumerable<IssuerSecretKeyEntityModel>> GetAll()
        {
            try
            {
                return _issuerUserSecretKeyRepository.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IssuerSecretKeyEntityModel> GetById(int id)
        {
            try
            {
                return _issuerUserSecretKeyRepository.GetById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IssuerSecretKeyEntityModel> GetSecretKeyByIssuer(string issuer)
        {
            try
            {
                return _issuerUserSecretKeyRepository.GetSecretKeyByIssuer(issuer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<IssuerSecretKeyEntityModel>> Update(IssuerSecretKeyEntityModel entity)
        {
            try
            {
                return _issuerUserSecretKeyRepository.Update(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
