using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DynamicForms.Repositories.FormulaRepo
{
    public class FormulaRepo : IFormulaRepo
    {
        private readonly IMongoCollection<FormulaTree> _FormulasCollection;

        public FormulaRepo(DataContext context, IOptions<DynamicFormsDatabaseSettings> dynamicFormsDbSettings)
        {
            var mongoClient = new MongoClient(dynamicFormsDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dynamicFormsDbSettings.Value.DatabaseName);
            _FormulasCollection = mongoDatabase.GetCollection<FormulaTree>(dynamicFormsDbSettings.Value.DynamicFormsCollectionName);
        }
        public async Task<FormulaTree> GetFormulaTree(string _Id)
        {
            return await _FormulasCollection.Find(x => x.Id == _Id).FirstOrDefaultAsync();
        }

        public async Task<FormulaTree> GetFormulaWithFormId(int Id)
        {
            return await _FormulasCollection.Find(x => x.FormId == Id).FirstOrDefaultAsync();
        }
    }
}