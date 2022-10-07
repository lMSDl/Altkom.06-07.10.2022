using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;

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
            var fixture = new Fixture();
            string ANY_MESSAGE = fixture.Create<string>();
            Logger.LoggerEventArgs? eventArgs = null;
            object? eventSender = null;

            logger.MessageLogged += (sender, args) => { eventArgs = args; eventSender = sender; };
            // timeBefore = DateTime.Now;

            //Act
            logger.Log(ANY_MESSAGE);

            //Assert
            //var timeAfter = DateTime.Now;
            /*Assert.Equal(logger, eventSender);
            Assert.NotNull(eventArgs);
            Assert.Equal(ANY_MESSAGE, eventArgs.Message);
            Assert.InRange(eventArgs.DateTime, timeBefore, timeAfter);*/

            using (new AssertionScope())
            {
                eventSender.Should().Be(logger);
                eventArgs.Should().NotBeNull();
                eventArgs.Message.Should().Be(ANY_MESSAGE);
                eventArgs.DateTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            }
        }

        [Fact]
        public void Log_AnyMessage_ValidEventInvoked_WithMonitor()
        {
            //Arrange
            var logger = new Logger();
            const string ANY_MESSAGE = "a";

            using var monitor = logger.Monitor();

            //Act
            logger.Log(ANY_MESSAGE);

            //Assert

            monitor.Should().Raise(nameof(Logger.MessageLogged))
                .WithSender(logger)
                .WithArgs<Logger.LoggerEventArgs>(x => x.Message == ANY_MESSAGE);
        }

        [Fact]
        public void GetLogsAsync_DateRage_LoggedMessages()
        {
            //Arrange
            var logger = new Logger();
            const string ANY_MESSAGE = "a";
            logger.Log(ANY_MESSAGE);

            //Act
            var task = logger.GetLogsAsync(DateTime.Now.AddSeconds(-1), DateTime.Now);
            task.Wait();
            var result = task.Result;

            //Assert
            task.IsCompletedSuccessfully.Should().BeTrue();
            result.Should().Contain(ANY_MESSAGE);
            DateTime.TryParseExact(result.Split(": ")[0], "dd.MM.yyyy hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _).Should().BeTrue();
        }
    }
}
