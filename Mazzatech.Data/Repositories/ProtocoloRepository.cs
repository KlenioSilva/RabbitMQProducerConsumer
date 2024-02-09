using Mazzatech.Data.EF;
using Mazzatech.Domain.EntitiesModels;
using Mazzatech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Mazzatech.Data.Repositories
{
    public class ProtocoloRepository : IProtocoloRepository
    {
        private readonly Context _context;
        public ProtocoloRepository(Context context)
        {
            _context = context;
        }

        public void AddWithoutReturn(ProtocoloEntityModel entity)
        {
            try
            {
                _context.Add(entity);
                _context.SaveChanges();
                _context.Database.CloseConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProtocoloEntityModel>> Add(ProtocoloEntityModel entity)
        {
            try
            {
                _context.Add(entity);
                await _context.SaveChangesAsync();

                return await _context.Protocolos.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProtocoloEntityModel>> Delete(ProtocoloEntityModel entity)
        {
            try
            {
                _context.Remove(entity);
                _context.SaveChanges();
            
                return await _context.Protocolos.ToListAsync(); 
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProtocoloEntityModel>> DeleteById(int id)
        {
            try
            {
                var protocolo = _context.Protocolos.Where(x => x.Id == id).First();
                _context.Remove(protocolo);
                _context.SaveChanges();

                return await _context.Protocolos.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<ProtocoloEntityModel> GetById(int id)
        {
            try
            {
                return await _context.Protocolos.Where(x => x.Id == id).FirstAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProtocoloEntityModel>> GetAll()
        {
            try
            {
                return await _context.Protocolos.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProtocoloEntityModel>> Update(ProtocoloEntityModel entity)
        {
            try
            {
                var protocoloDb = await _context.Protocolos.Where(x => x.Id == entity.Id).FirstAsync();
                if (protocoloDb != null)
                {
                    // Atualiza as propriedades da entidade rastreada
                    _context.Entry(protocoloDb).CurrentValues.SetValues(entity);
                    _context.SaveChanges();
                }

                return await _context.Protocolos.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProtocoloEntityModel> GetByProtocolo(Guid? protocolo)
        {
            return await _context.Protocolos.Where(x => x.Protocolo == protocolo).FirstAsync();
        }

        public async Task<ProtocoloEntityModel> GetByCPF(string? cpf)
        {
            return await _context.Protocolos.Where(x => x.CPF == cpf).FirstAsync();
        }

        public async Task<ProtocoloEntityModel> GetByRG(string? rg)
        {
            return await _context.Protocolos.Where(x => x.RG == rg).FirstAsync();
        }
    }
}
