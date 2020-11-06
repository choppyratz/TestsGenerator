using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestsGenerator;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace TestGeneratorUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private string testSourceCode = @"using System;
                                         
                                         namespace TestLibrary
                                         {
                                             public class Calculator
                                             {
                                                 public Calculator(int a, int b)
                                                 {
                                         
                                                 }
                                         
                                                 public int Addition(int a, int b)
                                                 {
                                                     return a + b;
                                                 }
                                         
                                                 public int Submission(int a, int b)
                                                 {
                                                    return a - b;
                                                 }
                                         
                                                 public int Division(int a, int b)
                                                 {
                                                    return a / b;
                                                 }

                                                 public int Multiplication(int a, int b)
                                                 {
                                                    return a * b;
                                                 }
                                             }
                                         }";
        private SyntaxTree tree;

        [TestInitialize]
        public void SetUp()
        {
            var generator = new Generator();
            tree = SyntaxFactory.ParseSyntaxTree(generator.GenerateUnitTestClass(testSourceCode));
        }

        [TestMethod]
        public void TestMethodsCount()
        {
            IEnumerable<MethodDeclarationSyntax> methods = tree
              .GetRoot()
              .DescendantNodes()
              .OfType<MethodDeclarationSyntax>();

            Assert.AreEqual(methods.Count(), 4);
        }

        [TestMethod]
        public void TestUsings()
        {
            var usings = tree.GetRoot().DescendantNodes().Where(node => node is UsingDirectiveSyntax).ToList();
            Assert.AreEqual((usings[0] as UsingDirectiveSyntax).Name.ToString(), "Sysytem");
            Assert.AreEqual((usings[1] as UsingDirectiveSyntax).Name.ToString(), "Microsoft.VisualStudio.TestTools.UnitTesting");
        }

        [TestMethod]
        public void TestClass()
        {
            var classes = tree.GetRoot().DescendantNodes().Where(node => node is ClassDeclarationSyntax).ToList();
            string test = (classes[0] as ClassDeclarationSyntax).Identifier.Text;
            Assert.AreEqual((classes[0] as ClassDeclarationSyntax).Identifier.Text, "UnitTest1");
            Assert.AreEqual((classes[0] as ClassDeclarationSyntax).AttributeLists.Count, 1);
        }
    }
}
