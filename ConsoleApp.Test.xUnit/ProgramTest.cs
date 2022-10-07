using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test.xUnit
{
    public class ProgramTest
    {
        [Fact]
        public void Main_ConsoleOutput_HelloWorld()
        {
            //Arrange
            var main = typeof(Program).Assembly.EntryPoint;
            var stringWriter = new StringWriter();
            var originalConsoleOut = Console.Out;
            Console.SetOut(stringWriter);

            //Act
            main.Invoke(null, new object[] { Array.Empty<string>() });

            //Assert
            Console.SetOut(originalConsoleOut);
            Assert.Equal("Hello, World!\n", stringWriter.ToString(), ignoreLineEndingDifferences: true);
            stringWriter.Dispose();
        }
    }
}
