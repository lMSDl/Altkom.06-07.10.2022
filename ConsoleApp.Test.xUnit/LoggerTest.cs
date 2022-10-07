using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test.xUnit
{
    public class LoggerTest
    {
        [Fact]
        public void Log_AnyMessage_EventInvoked()
        {
            //Arrange
            var logger = new Logger();
            const string ANY_MESSAGE = "a";
            var result = false;
            logger.MessageLogged += (sender, args) => result = true;

            //Act
            logger.Log(ANY_MESSAGE);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void Log_AnyMessage_ValidEventInvoked()
        {
            //Arrange
            var logger = new Logger();
            const string ANY_MESSAGE = "a";
            Logger.LoggerEventArgs? eventArgs = null;
            object? eventSender = null;

            logger.MessageLogged += (sender, args) => { eventArgs = args; eventSender = sender; };
            var timeBefore = DateTime.Now;

            //Act
            logger.Log(ANY_MESSAGE);

            //Assert
            var timeAfter = DateTime.Now;
            Assert.Equal(logger, eventSender);
            Assert.NotNull(eventArgs);
            Assert.Equal(ANY_MESSAGE, eventArgs.Message);
            Assert.InRange(eventArgs.DateTime, timeBefore, timeAfter);
        }
    }
}
