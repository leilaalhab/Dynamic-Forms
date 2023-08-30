using DynamicForms.Services.FormulaService;

namespace DynamicForms.Services.HandleFormulaService
{
    public class HandleFormulaService : IHandleFormulaService
    {
        private readonly IFormulaService _FormulaService;
        public static InputWrapper[]? inputs;
        public static Stack<Node>? stepPath;
        public static Node currentStepRoot;

        public HandleFormulaService(IFormulaService formulaService)
        {
            _FormulaService = formulaService;
        }
 
        public async Task<FormulaTree> GetFormula(int formId)
        {
            var formula = await _FormulaService.GetFormulaWithFormId(formId);
            await _FormulaService.AddInputPaths(formula.Data);

            if (formula.Data is not null)
                return formula.Data;

            throw new Exception($"No formula with formId {formId} found.");
        }

        public double EvaluateFormula(FormulaTree formula, InputWrapper[] _inputs)
        {
            
            inputs = _inputs;
            RecursiveEvaluate(formula.Root);
            return formula.Root.Value;
        }

        public async Task<double> EvaluateFormula(FormulaTree formula, InputWrapper[] _inputs, int InputId)
        {
            double value = await CalculatePrice(formula.FormId, InputId);
            inputs = _inputs;
            return value;
        }        

        private async Task<double> CalculatePrice(int formId, int inputId)
        {
            
            Stack<Node>? nodes = await FindInputPath(formId, currentStepRoot, inputId);

            if (nodes is null)
            {
                throw new Exception("Input Id not found.");
            }
            else
            {
                nodes.Pop();
                double temp = 0;

                foreach (var node in nodes)
                {
                    var operation = node;
                    var op1 = node.Left;
                    var op2 = node.Right;
                    Calculate(GetValue(op1), GetValue(op2), operation);
                    temp = node.Value;
                }

                return temp;
            }
        }

        private void RecursiveEvaluate(Node root)
        {
            currentStepRoot = root;
            if (root.Left is not null && IsOperation(root.Left))
                RecursiveEvaluate(root.Left);
            if (root.Right is not null && IsOperation(root.Right))
                RecursiveEvaluate(root.Right);

            Calculate(GetValue(root.Left), GetValue(root.Right), root);
        }

        public static bool IsOperation(Node node)
        {
            if (node.Type == NodeType.Addition || node.Type == NodeType.Subtraction || node.Type == NodeType.Multiplication || node.Type == NodeType.Division)
                return true;
            return false;
        }

        public void ChangeStep(Node root, int stepId) {
            var node = FindStepFormula(root, stepId);
            currentStepRoot = node;
            
        }

        private Node FindStepFormula(Node root, int stepId)
        {
            Stack<Node>? nodes = new();

            do
            {
                nodes.Push(root);

                while (!IsLeaf(root))
                {
                    if (NodeMatchesInput(root, stepId))
                    {
                        stepPath = nodes;
                        return root;
                    }
                    root = root.Left;
                    nodes.Push(root);
                }

                var temp = nodes.Pop();

                if (root.Right == temp)
                {
                    nodes.Pop();
                }

                root = nodes.Peek().Right;


            } while (nodes.Count > 0);

            return null;
        }

        public async Task<FormulaInputPaths> GetInputPaths(int formId) {
            var paths = await _FormulaService.GetInputPaths(formId);
            return paths;
        }
        private async Task<Stack<Node>?> FindInputPath(int formId, Node root, int inputId)
        {
            Stack<Node> nodes = stepPath;
            var path = await _FormulaService.GetInputPath(formId, inputId);
            var temproot = root;


            if (path is null)
                return null;

            foreach (var direction in path)
            {
                if (direction) {
                    temproot = temproot.Left;
                    nodes.Push(temproot);
                }
                else {
                    temproot = temproot.Right;
                    nodes.Push(temproot);
                }
            }

            if (root.InputId == inputId)
                return nodes;
            
            Console.WriteLine("Didnt find the node.");
            nodes = stepPath;
            
            do
            {
                nodes.Push(root);

                while (!IsLeaf(root))
                {
                    root = root.Left;
                    nodes.Push(root);
                }

                if (NodeMatchesInput(root, inputId))
                {
                    return nodes;
                }
                else
                {
                    var temp = nodes.Pop();

                    if (root.Right == temp)
                    {
                        nodes.Pop();
                    }

                    root = nodes.Peek().Right;
                }

            } while (nodes.Count > 0);

            return null;
        }
        private bool IsLeaf(Node node)
        {
            if (node.Left is null && node.Right is null)
                return true;
            return false;
        }

        public static void Calculate(double op1, double op2, Node operation)
        {
            switch (operation.Type)
            {
                case NodeType.Addition:
                    operation.Value = op2 + op1;
                    return;
                case NodeType.Subtraction:
                    operation.Value = op2 - op1;
                    return;
                case NodeType.Multiplication:
                    operation.Value = op2 * op1;
                    return;
                case NodeType.Division:
                    if (op1 == 0)
                    {
                        operation.Value = 0;
                        return;
                    }
                    operation.Value = op2 / op1;
                    return;
                default: throw new Exception("Invalid operation");
            }
        }

        public bool NodeMatchesInput(Node node, int inputId)
        {
            if (node.Type == NodeType.Variable && node.InputId == inputId)
                return true;
            return false;
        }


        private double GetValue(Node? node)
        {
            if (node is null)
                throw new Exception("Formula calculation error.");

            if (node.Type == NodeType.Variable)
            {
                if (node.InputId is not null)
                    return FindInputWithId(node.InputId.Value).Value.Value;
                else
                    throw new Exception("Node does not have InputId.");
            }
            else
                return node.Value;
        }

        private InputWrapper FindInputWithId(int InputId)
        {
            if (inputs is not null)
                return inputs.FirstOrDefault(r => r.Input.Id == InputId) ?? throw new Exception($"Input with Id {InputId} was not found.");
            else
                throw new Exception("inputs not initialized when calculating price.");
        }

    }
}