using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp.Test.MSTest
{
    [TestClass]
    public class GardenTest
    {
        #region BAD_PRACTISE
        private Garden Garden { get; set; }
        [TestInitialize]
        public void SetUp()
        {
            Garden = new Garden(1);
        }

        [TestCleanup]
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

        [TestMethod]
        [DataRow(-1)]
        [DataRow(int.MinValue)]
        public void Garden_InvalidSize_Exception(int invalidSize)
        {
            //Act
            Action result = () => new Garden(invalidSize);

            //Assert
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(result);
            Assert.AreEqual("size", exception.ParamName);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(int.MaxValue)]
        public void Garden_ValidSize_SizeInit(int validSize)
        {
            //Act
            var garden = new Garden(validSize);

            //Assert
            Assert.AreEqual(validSize, garden.Size);
        }


        [TestMethod]
        public void Plant_ValidName_True()
        {
            // Arrange
            Garden garden = GetNonZeroSizeGarden();
            const string VALID_NAME = "a";

            // Act
            var result = garden.Plant(VALID_NAME);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
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
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException))]
        public void Plant_InvalidName_ArgumentException(string invalidName)
        {
            //Arrange
            Garden garden = GetInsignificantSizeGarden();

            //Act
            garden.Plant(invalidName);
        }


        [TestMethod]
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
            Assert.IsTrue(garden.GetPlants().Contains(VALID_NAME + "2"));
        }

        [TestMethod]
        public void Plant_ValidName_AddedToGarden()
        {
            //Arrange
            Garden garden = GetNonZeroSizeGarden();
            const string VALID_NAME = "a";

            //Act
            garden.Plant(VALID_NAME);

            //Assert
            Assert.IsTrue(garden.GetPlants().Contains(VALID_NAME));
        }


        [TestMethod]
        public void GetPlants_CopyOfPlantsCollection()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = 0;
            var garden = new Garden(INSIGNIFICANT_SIZE);

            //Act
            var result1 = garden.GetPlants();
            var result2 = garden.GetPlants();

            //Assert
            Assert.AreNotSame(result1, result2);
        }

    }
}