using System;
using System.Reflection.Metadata;
using DynamicForms.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DynamicForms.Models;
using DynamicForms.Data;
using Microsoft.Extensions.Options;

namespace HandlingFormTests
{

    public class InputContext : DataContext
    {
        public InputContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public virtual DbSet<Input> Inputs { get; set; }
        public virtual DbSet<Step>  Steps { get; set; }
    }

    [TestClass]
    public class HandleFormTest
    {

        [TestMethod]
        public void TestErrorGeneration()
        {
            var req = new Requirement
            {
                Type = ConditionType.GreaterThan,
                Value = 18,
            };

            var input = new InputWrapper {
                Input = new Input { Id = 1},
            };


            var answer = HandleFormService.CheckRequirement(req, 19, input);
            Assert.AreEqual(true, answer);
        }

        [TestMethod]
        public void TestPriceCalculationFromStack()
        {
            Console.WriteLine("Test");
            
            var mockInputs = new List<InputWrapper>()
            {
                new InputWrapper {Value = 10, Input = new Input {Id = 1} },
                new InputWrapper {Value = 5, Input = new Input {Id = 2} },
                new InputWrapper {Value = 2, Input = new Input {Id = 3} },
                new InputWrapper {Value = 2.5, Input = new Input {Id = 4} },
            }.ToArray();

            var test = new DynamicFormsDatabaseSettings();
            //var service = new HandleFormService(null, null, test);
            // service.inputs = mockInputs;

            var FormulaRoot = new @int
            {
                Type = NodeType.Addition,
                Left = new @int
                {
                    Type = NodeType.Variable,
                    InputId = 1
                },
                Right = new @int
                {
                    Type = NodeType.Multiplication,
                    Left = new @int
                    {
                        Type = NodeType.Variable,
                        InputId = 2
                    },
                    Right = new @int
                    {
                        Type = NodeType.Variable,
                        InputId = 3
                    }
                },
            };

            

            //var res = service.CalculatePrice(FormulaRoot, mockInputs[2].Input);

            //Console.WriteLine(res);
            //Assert.AreEqual( 20, res);
        }



        [TestMethod]
        public void TestGetInputsInOrder()
        {
            
            var data = new List<Input>
            {
                new Input { Id = 1, Order = 3},
                new Input { Id = 2, Order = 1 },
                new Input { Id = 3, Order = 2 },
            };

            var steps = new List<Step>
            {
                new Step {FormId = 1, Inputs = data},
            }.AsQueryable();

            var inputs = data.AsQueryable();



            var mockSet = new Mock<DbSet<Step>>();
            mockSet.As<IQueryable<Step>>().Setup(m => m.Provider).Returns(steps.Provider);
            mockSet.As<IQueryable<Step>>().Setup(m => m.Expression).Returns(steps.Expression);
            mockSet.As<IQueryable<Step>>().Setup(m => m.ElementType).Returns(steps.ElementType);
            mockSet.As<IQueryable<Step>>().Setup(m => m.GetEnumerator()).Returns(() => steps.GetEnumerator());

            var mockContext = new Mock<InputContext>();
            mockContext.Setup(c => c.Steps).Returns(mockSet.Object);

            var service = new HandleFormService(null, mockContext.Object, null);
            

            //Assert.AreEqual(3, service.inputs.Count());
            //Assert.AreEqual(0, service.inputs[0].Index);
           // Assert.AreEqual(1, service.inputs[1].Index);
           // Assert.AreEqual(2, service.inputs[2].Index);
        }

    }
}

