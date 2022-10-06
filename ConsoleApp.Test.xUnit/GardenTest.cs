using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test.xUnit
{
    public class GardenTest
    {
        [Fact]
        //Plant_<scenario&exprectedResultDescription>
        //public void Plant_GivesTrueWhenProvidedValidName
        //Plant_<scenario>_<exprectedResult>
        //public void Plant_PassValidName_ReturnsTrue()
        public void Plant_ValidName_True()
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 1;
            const string VALID_NAME = "a";
            var garden = new Garden(MINIMAL_VALID_SIZE);

            // Act
            var result = garden.Plant(VALID_NAME);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Plant_OverflowGarden_False()
        {
            // Arrange
            const int NON_ZERO_SIZE = 1;
            var garden = new Garden(NON_ZERO_SIZE);
            const string VALID_PLANT_NAME_1 = "a";
            const string VALID_PLANT_NAME_2 = "b";
            garden.Plant(VALID_PLANT_NAME_1);

            // Act
            var result = garden.Plant(VALID_PLANT_NAME_2);

            // Asset
            Assert.False(result);
        }


        [Fact]
        public void Plant_NullName_ArgumentNullException()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = 0;
            var garden = new Garden(INSIGNIFICANT_SIZE);
            const string? NULL_NAME = null;

            //Act
            Action result = () => garden.Plant(NULL_NAME);

            //Assert
            var argumentNullException = Assert.Throws<ArgumentNullException>(result); //Throws oczekuje konkretnego typu
            //var argumentNullException = Assert.ThrowsAny<ArgumentException>(result); //ThrowsAny uwzględnia dziedziczenie
            Assert.Equal("Name", argumentNullException.ParamName, ignoreCase: true);
        }

        [Fact]
        public void Plant_WhiteSpaceName_ArgumentException()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = 0;
            var garden = new Garden(INSIGNIFICANT_SIZE);
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
            const int MINIMAL_VALID_SIZE = 1;
            var garden = new Garden(MINIMAL_VALID_SIZE);
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
    }
}
