using System.Text.RegularExpressions;
using DynamicForms.Filter;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace DynamicForms.Services.FormulaService
{
    public class FormulaService : IFormulaService
    {
        private readonly IMongoCollection<FormulaTree> _FormulasCollection;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public FormulaService(DataContext context, IMapper mapper, IOptions<DynamicFormsDatabaseSettings> dynamicFormsDbSettings)
        {
            var mongoClient = new MongoClient(dynamicFormsDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dynamicFormsDbSettings.Value.DatabaseName);
            _FormulasCollection = mongoDatabase.GetCollection<FormulaTree>(dynamicFormsDbSettings.Value.DynamicFormsCollectionName);
            _mapper = mapper;
            _context = context;
        }


        public async Task<ServiceResponse<GetFormulaDto>> GetFormula(string Id)
        {
            var response = new ServiceResponse<GetFormulaDto>();
            try
            {
                var formula = await _FormulasCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();
                response.Data = _mapper.Map<GetFormulaDto>(formula);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetFormulaDto>> AddFormula(AddFormulaDto newFormula)
        {
            var response = new ServiceResponse<GetFormulaDto>();
            try {
            var Parent = await _context.Forms.FirstOrDefaultAsync(c => c.Id == newFormula.ParentId);

            if (Parent is null)
            {
                response.Success = false;
                response.Message = $"Step/Form with id {newFormula.ParentId} was not found.";
                return response;
            }
            Console.WriteLine("reach");
            var tree = TreeGenerator.GenerateTreeFromExpression(newFormula.Formula);
            tree.FormId = newFormula.ParentId;

            await _FormulasCollection.InsertOneAsync(tree);

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
             return response;
        }


        public async Task<ServiceResponse<GetFormulaDto>> UpdateFormula(UpdateFormulaDto updatedFormula)
        {

            var response = new ServiceResponse<GetFormulaDto>();
            
            FormulaTree newTree = TreeGenerator.GenerateTreeFromExpression(updatedFormula.Formula);
            if (newTree == null) {
                throw new Exception($"Formula with Id {updatedFormula.Id} was not found.");
            }
            await _FormulasCollection.FindOneAndReplaceAsync(x => x.Id == updatedFormula.Id, newTree);

            return response;
        }

        public async Task<ServiceResponse<int>> DeleteFormula(string Id)
        {
            var response = new ServiceResponse<int>();

            await _FormulasCollection.FindOneAndDeleteAsync(x => x.Id == Id);

            return response;
        }

    }
}