using AutoFixture;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test.xUnit
{
    public class GardenTest : IDisposable
    {
        #region BAD_PRACTISE
        private Garden Garden { get; }
        //odpowiedniek Setup w xUnit
        public GardenTest()
        {
            Garden = new Garden(1);
        }

        //odpowiedniek TearDown w xUnit
        public void Dispose()
        {
        }
        #endregion BAD_PRACTISE

        // metody konstrukcyjne są bardziej preferowane od SetUp i TearDown
        private static Garden GetNonZeroSizeGarden()
        {
            const int NON_ZERO_SIZE = 1;
            var loggerStub = new Mock<ILogger>();
            var garden = new Garden(NON_ZERO_SIZE, loggerStub.Object);
            return garden;
        }
        private static Garden GetInsignificantSizeGarden()
        {
            const int INSIGNIFICANT_SIZE = 0;
            var garden = new Garden(INSIGNIFICANT_SIZE);
            return garden;
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Garden_InvalidSize_Exception(int invalidSize)
        {
            //Act
            Action result = () => new Garden(invalidSize);

            //Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(result);
            Assert.Equal("size", exception.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void Garden_ValidSize_SizeInit(int validSize)
        {
            //Act
            var garden = new Garden(validSize);

            //Assert
            Assert.Equal(validSize, garden.Size);
        }


        [Fact]
        //Plant_<scenario&exprectedResultDescription>
        //public void Plant_GivesTrueWhenProvidedValidName
        //Plant_<scenario>_<exprectedResult>
        //public void Plant_PassValidName_ReturnsTrue()
        public void Plant_ValidName_True()
        {
            // Arrange
            Garden garden = GetNonZeroSizeGarden();
            const string VALID_NAME = "a";

            // Act
            var result = garden.Plant(VALID_NAME);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Plant_OverflowGarden_False()
        {
            // Arrange
            Garden garden = GetNonZeroSizeGarden();
            const string VALID_PLANT_NAME_1 = "a";
            const string VALID_PLANT_NAME_2 = "b";
            garden.Plant(VALID_PLANT_NAME_1);

            // Act
            var result = garden.Plant(VALID_PLANT_NAME_2);

            // Asset
            Assert.False(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Plant_InvalidName_ArgumentException(string invalidName)
        {
            //Arrange
            Garden garden = GetInsignificantSizeGarden();

            //Act
            Action result = () => garden.Plant(invalidName);

            //Assert
            var argumentNullException = Assert.ThrowsAny<ArgumentException>(result);
            Assert.Equal("name", argumentNullException.ParamName);
        }

        [Fact(Skip = "Replaced by Plant_InvalidName_ArgumentException")]
        public void Plant_NullName_ArgumentNullException()
        {
            //Arrange
            Garden garden = GetInsignificantSizeGarden();
            const string? NULL_NAME = null;

            //Act
            Action result = () => garden.Plant(NULL_NAME);

            //Assert
            var argumentNullException = Assert.Throws<ArgumentNullException>(result); //Throws oczekuje konkretnego typu
            //var argumentNullException = Assert.ThrowsAny<ArgumentException>(result); //ThrowsAny uwzględnia dziedziczenie
            Assert.Equal("Name", argumentNullException.ParamName, ignoreCase: true);
        }

        [Fact(Skip = "Replaced by Plant_InvalidName_ArgumentException")]
        public void Plant_WhiteSpaceName_ArgumentException()
        {
            //Arrange
            Garden garden = GetInsignificantSizeGarden();
            const string WHITE_SPACE_NAME = " ";

            //Act

            var exception = Record.Exception(() => garden.Plant(WHITE_SPACE_NAME));

            //Assert
            Assert.NotNull(exception);
            var argumentException = Assert.IsType<ArgumentException>(exception);
            Assert.Equal("name", argumentException.ParamName);
            Assert.Contains("Roślina musi posiadać nazwę!", argumentException.Message);
        }


        [Fact]
        public void Plant_ExistingName_ChangedNameOnList()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 2;
            var garden = new Garden(MINIMAL_VALID_SIZE);
            const string VALID_NAME = "a";
            garden.Plant(VALID_NAME);

            //Act
            garden.Plant(VALID_NAME);

            //Assert
            Assert.Contains(VALID_NAME + "2", garden.GetPlants());
        }

        [Fact]
        public void Plant_ValidName_AddedToGarden()
        {
            //Arrange
            Garden garden = GetNonZeroSizeGarden();
            const string VALID_NAME = "a";

            //Act
            garden.Plant(VALID_NAME);

            //Assert
            Assert.Contains(VALID_NAME, garden.GetPlants());
        }


        [Fact]
        public void GetPlants_CopyOfPlantsCollection()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = 0;
            var garden = new Garden(INSIGNIFICANT_SIZE);

            //Act
            var result1 = garden.GetPlants();
            var result2 = garden.GetPlants();

            //Assert
            Assert.NotSame(result1, result2);
        }

        [Fact]
        public void Plant_ValidName_MessageLogged()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 1;
            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Log(It.IsAny<string>())).Verifiable();

            var garden = new Garden(MINIMAL_VALID_SIZE, loggerMock.Object);
            var plantName = new Fixture().Create<string>();

            //Act
            garden.Plant(plantName);

            //Assert
            loggerMock.Verify();
        }

        [Fact]
        public void Plant_DuplicatedName_MessageLogged()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 2;
            var loggerMock = new Mock<ILogger>();

            var garden = new Garden(MINIMAL_VALID_SIZE, loggerMock.Object);
            var plantName = new Fixture().Create<string>();
            garden.Plant(plantName);

            //Act
            garden.Plant(plantName);

            //Assert
            loggerMock.Verify(x => x.Log(It.Is<string>(x => x.Contains(plantName))), Times.Exactly(3));
        }


        [Fact]
        public void ShowLastLog_LastLog()
        {
            //Arrange
            var fixture = new Fixture();
            var plantName1 = fixture.Create<string>();
            var plantName2 = fixture.Create<string>();
            var logger = new Mock<ILogger>();
            logger.Setup(x => x.GetLogsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync($"{plantName1}\n{plantName2}");

            const int MINIMAL_VALID_SIZE = 2;
            var garden = new Garden(1, logger.Object);
            garden.Plant(plantName1);
            garden.Plant(plantName2);

            //Act
            var result = garden.ShowLastLog();

            //Assert
            result.Should().Be(plantName2);
        }
    }
}
