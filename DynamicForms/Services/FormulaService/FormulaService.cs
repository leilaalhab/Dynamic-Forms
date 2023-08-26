using System.Collections;
using System.Text.RegularExpressions;
using DynamicForms.Filter;
using DynamicForms.Services.InputService;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DynamicForms.Models;
using ZstdSharp.Unsafe;

namespace DynamicForms.Services.FormulaService
{
    public class FormulaService : IFormulaService
    {
        private readonly IMongoCollection<FormulaTree> _FormulasCollection;
        private readonly IMongoCollection<FormulaInputPaths> _InputPathsCollection;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public FormulaService(DataContext context, IMapper mapper, IOptions<DynamicFormsDatabaseSettings> dynamicFormsDbSettings)
        {
            var mongoClient = new MongoClient(dynamicFormsDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dynamicFormsDbSettings.Value.DatabaseName);
            _FormulasCollection = mongoDatabase.GetCollection<FormulaTree>(dynamicFormsDbSettings.Value.DynamicFormsCollectionName);
            _InputPathsCollection = mongoDatabase.GetCollection<FormulaInputPaths>("InputPaths");
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

        public async Task<ServiceResponse<FormulaTree>> GetFormulaWithFormId(int FormId)
        {
            var response = new ServiceResponse<FormulaTree>();
            try
            {
                var formula = await _FormulasCollection.Find(x => x.FormId == FormId).FirstOrDefaultAsync();
                response.Data = formula;
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
            try
            {
                var Parent = await _context.Forms.FirstOrDefaultAsync(c => c.Id == newFormula.ParentId);

                if (Parent is null)
                {
                    response.Success = false;
                    response.Message = $"Step/Form with id {newFormula.ParentId} was not found.";
                    return response;
                }
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

        public async Task<FormulaInputPaths> GetInputPaths(int formId) {
                        FormulaInputPaths formulaPaths = await _InputPathsCollection.Find(c => c.FormId == formId).FirstOrDefaultAsync();
                        return formulaPaths;

        }

        public async Task<bool[]?> GetInputPath(int formId, int InputId)
        {
            FormulaInputPaths formulaPaths = await _InputPathsCollection.Find(c => c.FormId == formId).FirstOrDefaultAsync();
            var res = formulaPaths.Paths.FirstOrDefault(c => c.InputId == InputId);

            return res.Path;
        }

        public async Task AddInputPaths(FormulaTree tree)
        {

            FormulaInputPaths formulaInputPaths = new() { FormId = tree.FormId };
            List<InputPath> inputPaths = new();

            Stack<Node> nodes = new();
            Models.Stack stack = new();

            var root = tree.Root;
            do
            {
                nodes.Push(root);

                while (!IsLeaf(root))
                {
                    root = root.Left;
                    nodes.Push(root);
                    stack.Push(true);
                }

                if (IsNodeInput(root))
                {
                    var input = await _context.Inputs.FirstOrDefaultAsync(c => c.Id == root.InputId);
                    inputPaths.Add(new InputPath { InputId = input.Id, Path = stack.GetArray() });

                }

                var temp = nodes.Pop();
                stack.Pop();


                if (nodes.Peek().Right == temp)
                {
                    nodes.Pop();
                    stack.Pop();
                    if (nodes.Count == 1)
                        break;
                } else {

                root = nodes.Peek().Right;
                stack.Push(false);
                }

            } while (nodes.Count > 0);

            formulaInputPaths.Paths = inputPaths.ToArray();
            await _InputPathsCollection.InsertOneAsync(formulaInputPaths);
        }

        private bool IsLeaf(Node node)
        {
            if (node.Left is null && node.Right is null)
                return true;
            return false;
        }

        private bool IsNodeInput(Node node)
        {
            if (node.Type == NodeType.Variable)
                return true;
            return false;
        }


        public async Task<ServiceResponse<GetFormulaDto>> UpdateFormula(UpdateFormulaDto updatedFormula)
        {

            var response = new ServiceResponse<GetFormulaDto>();

            FormulaTree newTree = TreeGenerator.GenerateTreeFromExpression(updatedFormula.Formula);
            if (newTree == null)
            {
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