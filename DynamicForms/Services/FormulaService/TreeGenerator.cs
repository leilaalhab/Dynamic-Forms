using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Services.FormulaService
{
    public static class TreeGenerator
    {
        private static Element[] elements;
        private static int counter;

        public static FormulaTree GenerateTreeFromExpression(Element[] input) {
            elements = input;
            counter = 0;

            return new FormulaTree {
                Root = GenerateExpressionTree()
            };
        }

        private static Node GenerateExpressionTree()
        {

            Node leftNode = CreateChildNode(ReadInput());
            Node root = CreateRootNode(ReadInput());
            Node rightNode = CreateChildNode(ReadInput());

            AssignChildren(root, leftNode, rightNode);

            return root;
        }

        private static Node ReadInput()
        {
            Element temp = elements[counter];
            counter++;
            
                return temp.Type switch
                {
                    NodeType.Number => new Node(temp.Value, NodeType.Number),
                    NodeType.Variable => new Node(0, temp.InputId, NodeType.Variable),
                    NodeType.Addition => new Node( NodeType.Addition),
                    NodeType.Subtraction => new Node( NodeType.Subtraction),
                    NodeType.Division => new Node(NodeType.Division),
                    NodeType.Multiplication => new Node( NodeType.Multiplication),
                    NodeType.Parenthesis => new Node( NodeType.Parenthesis),
                    NodeType.ClosedParenthesis => ReadInput(),
                    _ => throw new Exception("Invalid input."),
                };
        }

        private static void AssignChildren(Node root, Node leftNode, Node rightNode)
        {
            root.Left = leftNode;
            root.Right = rightNode;
        }

        public static bool IsOperation(Node node)
        {
            if (node.Type == NodeType.Addition || node.Type == NodeType.Subtraction || node.Type == NodeType.Multiplication || node.Type == NodeType.Division)
                return true;
            return false;
        }

        private static Node CreateChildNode(Node temp)
        {
            Node child;

            if (temp.Type == NodeType.Parenthesis)
                child = GenerateExpressionTree();
            else
                child = temp;
            return child;
        }

        private static Node CreateRootNode(Node temp)
        {
            if (IsOperation(temp))
                return temp;
            else
                throw new Exception("Invalid expression.");
        }

        public static void Calculate(double op1, double op2, Node operation)
        {
            switch (operation.Type)
            {
                case NodeType.Addition: 
                operation.Value =  op2 + op1;
                return;
                case NodeType.Subtraction: 
                operation.Value = op2 - op1;
                return;
                case NodeType.Multiplication: 
                operation.Value =  op2 * op1;
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

        


    }
}