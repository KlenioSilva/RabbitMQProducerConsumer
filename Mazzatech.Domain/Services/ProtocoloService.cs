using Mazzatech.Domain.EntitiesModels;
using Mazzatech.Domain.Interfaces.Repositories;
using Mazzatech.Domain.Interfaces.Services;
using System;

namespace Mazzatech.Domain.Services
{
    public class ProtocoloService : IProtocoloService
    {
        private readonly IProtocoloRepository _protocoloRepository;
        public ProtocoloService(IProtocoloRepository protocoloRepository)
        {
            _protocoloRepository = protocoloRepository;
        }

        public void AddWithoutReturn(ProtocoloEntityModel entity)
        {
            try
            {
                _protocoloRepository.AddWithoutReturn(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<ProtocoloEntityModel>> Add(ProtocoloEntityModel entity)
        {
            try
            {
                return _protocoloRepository.Add(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<ProtocoloEntityModel>> Delete(ProtocoloEntityModel entity)
        {
            try
            {
                return _protocoloRepository.Delete(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public Task<IEnumerable<ProtocoloEntityModel>> DeleteById(int id)
        {
            try
            {
                return _protocoloRepository.DeleteById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _protocoloRepository.Dispose();
        }

        public Task<IEnumerable<ProtocoloEntityModel>> GetAll()
        {
            try
            {
                return _protocoloRepository.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<ProtocoloEntityModel>> Update(ProtocoloEntityModel entity)
        {
            try
            {
                return _protocoloRepository.Update(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<ProtocoloEntityModel> GetById(int id)
        {
            try
            {
                return _protocoloRepository.GetById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<ProtocoloEntityModel> GetByProtocolo(Guid? protocolo)
        {
            try
            {
                return _protocoloRepository.GetByProtocolo(protocolo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<ProtocoloEntityModel> GetByCPF(string? cpf)
        {
            try
            {
                return _protocoloRepository.GetByCPF(cpf);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<ProtocoloEntityModel> GetByRG(string? rg)
        {
            try
            {
                return _protocoloRepository.GetByRG(rg);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
