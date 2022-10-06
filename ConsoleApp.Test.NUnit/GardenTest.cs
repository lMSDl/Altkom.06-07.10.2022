using NuGet.Frameworks;

namespace ConsoleApp.Test.NUnit
{
    public class GardenTest
    {
            #region BAD_PRACTISE
            private Garden Garden { get; set; }
            [SetUp]
            public void SetUp()
            {
                Garden = new Garden(1);
            }

            [TearDown]
            public void Clean()
            {
            }
            #endregion BAD_PRACTISE

            private static Garden GetNonZeroSizeGarden()
            {
                const int NON_ZERO_SIZE = 1;
                var garden = new Garden(NON_ZERO_SIZE);
                return garden;
            }
            private static Garden GetInsignificantSizeGarden()
            {
                const int INSIGNIFICANT_SIZE = 0;
                var garden = new Garden(INSIGNIFICANT_SIZE);
                return garden;
            }

            [Theory]
            [TestCase(-1)]
            [TestCase(int.MinValue)]
            public void Garden_InvalidSize_Exception(int invalidSize)
            {
                //Act
                TestDelegate result = () => new Garden(invalidSize);

                //Assert
                var exception = Assert.Throws<ArgumentOutOfRangeException>(result);
                Assert.That(exception.ParamName, Is.EqualTo("size"));
            }

            [Theory]
            [TestCase(0)]
            [TestCase(1)]
            [TestCase(int.MaxValue)]
            public void Garden_ValidSize_SizeInit(int validSize)
            {
                //Act
                var garden = new Garden(validSize);

                //Assert
                Assert.That(validSize, Is.EqualTo(garden.Size));
            }


            [Test]
            public void Plant_ValidName_True()
            {
                // Arrange
                Garden garden = GetNonZeroSizeGarden();
                const string VALID_NAME = "a";

                // Act
                var result = garden.Plant(VALID_NAME);

                // Assert
                Assert.That(result, Is.True);
            }

            [Test]
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
                Assert.That(result, Is.False);
            }

            [Theory]
            [TestCase(null)]
            [TestCase("")]
            [TestCase(" ")]
            public void Plant_InvalidName_ArgumentException(string invalidName)
            {
                //Arrange
                Garden garden = GetInsignificantSizeGarden();

                //Act
                TestDelegate result = () => garden.Plant(invalidName);

                //Assert
                Assert.Throws(Is.InstanceOf<ArgumentException>()
                    .And.Property("ParamName").EqualTo("name"),
                    result);
            }


            [Test]
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
                Assert.That(garden.GetPlants(), Does.Contain(VALID_NAME+"2"));
            }

            [Test]
            public void Plant_ValidName_AddedToGarden()
            {
                //Arrange
                Garden garden = GetNonZeroSizeGarden();
                const string VALID_NAME = "a";

                //Act
                garden.Plant(VALID_NAME);

                //Assert
                Assert.That(garden.GetPlants(), Does.Contain(VALID_NAME));
            }


            [Test]
            public void GetPlants_CopyOfPlantsCollection()
            {
                //Arrange
                const int INSIGNIFICANT_SIZE = 0;
                var garden = new Garden(INSIGNIFICANT_SIZE);

                //Act
                var result1 = garden.GetPlants();
                var result2 = garden.GetPlants();

                //Assert
                Assert.That(result1, Is.Not.SameAs(result2));
            }

        }
}