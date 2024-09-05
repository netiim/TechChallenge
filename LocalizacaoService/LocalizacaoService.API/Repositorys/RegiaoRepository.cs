using Core.Entidades;
using LocalizacaoService._03_Repositorys;
using LocalizacaoService.Interfaces.Repository;
using MongoDB.Driver;

namespace Infraestrutura.Repositorios
{
    public class RegiaoRepository : BaseRepository<Regiao>,IRegiaoRepository
    {
        public RegiaoRepository(IMongoDatabase database)
            :base(database,"Regiao"){}

        public async Task<Regiao?> GetByDDDAsync(int ddd)
        {
            try
            {
                return await _collection.Find(entity => entity.NumeroDDD == ddd).FirstOrDefaultAsync();
            }
            catch (MongoBulkWriteException ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
