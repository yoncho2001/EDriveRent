using EDriveRent;
using EDriveRent.Models.Contracts;
using EDriveRent.Core.Contracts;
using EDriveRent.IO.Contracts;
using EDriveRent.IO;

namespace TestDrive
{
    [TestClass]
    public class Tests
    {
        private IController controller = new Controller();

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
    "A null or empty string was inappropriately allowed.")]
        public void UserNull()
        {
            IUser user3 = new User("", "Reenie", "7246506");
            IUser user4 = new User(null, "Reenie", "7246506");
        }

        [TestMethod]
        public void UserNormal()
        {
            IUser user1 = new User("Tisha", "Reenie", "7246506");

            Assert.AreEqual("Tisha", user1.FirstName, "User name is not saved corectly");
            Assert.AreEqual("Reenie", user1.LastName, "User last name is not saved corectly");
            Assert.AreEqual("7246506", user1.DrivingLicenseNumber, "User driving license is not saved corectly");
        }

        [TestMethod]
        public void UserIncreaseRating()
        {
            IUser user1 = new User("Tisha", "Reenie", "7246506");
            user1.IncreaseRating();

            Assert.AreEqual(0.5, user1.Rating, "User dont increase rating corectly");
        }

        public void UserDecreaseRating()
        {
            IUser user1 = new User("Tisha", "Reenie", "7246506");
            user1.IncreaseRating();
            user1.IncreaseRating();
            user1.IncreaseRating();
            user1.IncreaseRating();
            user1.IncreaseRating();
            user1.DecreaseRating();

            Assert.AreEqual(0.5, user1.Rating, "User dont decrease rating corectly");

            user1.DecreaseRating();
            Assert.AreEqual(0.0, user1.Rating, "User dont decrease rating corectly");
            Assert.AreEqual(true, user1.IsBlocked, "User dont change isBlocked corectly");
        }

        [TestMethod]
        public void RegisterUser()
        {
            Assert.AreEqual("Tisha Reenie is registered successfully with DLN-7246506"
                , controller.RegisterUser("Tisha", "Reenie", "7246506"), "dont register the user");

            Assert.AreEqual("7246506 is already registered in our platform."
                , controller.RegisterUser("Penka", "Reenie", "7246506"), "don't check if the id already exist");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
    "A null or empty string was inappropriately allowed.")]
        public void VehicleNull()
        {
            IVehicle vehicle1 = new PassengerCar("", "Volt", "CWP8032");
            IVehicle vehicle2 = new PassengerCar(null, "Volt", "CWP8032");
        }

        [TestMethod]
        public void VehicleNormal()
        {
            IVehicle vehicle1 = new PassengerCar("Chevrolet", "Volt", "CWP8032");

            Assert.AreEqual("Volt", vehicle1.Model, "Vehicle model is not saved corectly");
            Assert.AreEqual("Chevrolet", vehicle1.Brand, "Vehicle brand is not saved corectly");
            Assert.AreEqual("CWP8032", vehicle1.LicensePlateNumber, "Vehicle license plate is not saved corectly");
        }

        [TestMethod]
        public void VehicleChangeStatus()
        {
            IVehicle vehicle1 = new PassengerCar("Chevrolet", "Volt", "CWP8032");
            vehicle1.ChangeStatus();

            Assert.AreEqual(true, vehicle1.IsDamaged, "Vehicle model is not saved corectly");
        }

        [TestMethod]
        public void VehicleDrivePassengerCar()
        {
            IVehicle vehicle1 = new PassengerCar("Chevrolet", "Volt", "CWP8032");
            vehicle1.Drive(144);

            IVehicle vehicle2 = new CargoVan("Chevrolet", "Volt", "CWP8111");
            vehicle2.Drive(90);

            Assert.AreEqual(68, vehicle1.BatteryLevel, "Vehicle drive is not working corectly");
            Assert.AreEqual(45, vehicle2.BatteryLevel, "Cargo van drive is not working corectly");
        }

        [TestMethod]
        public void UploadVehicle()
        {

            Assert.AreEqual("Chevrolet Volt is uploaded successfully with LPN-CWP8032"
               , controller.UploadVehicle("PassengerCar", "Chevrolet", "Volt", "CWP8032"), "don't insert the vehicle corectly");

            Assert.AreEqual("CWP8032 belongs to another vehicle."
                , controller.UploadVehicle("PassengerCar", "Chevro", "Vo", "CWP8032"), "don't check if the id already exist");
        }

        [TestMethod]
        public void UploadVehicleWrongType()
        {
            Assert.AreEqual("Passenger is not accessible in our platform."
                , controller.UploadVehicle("Passenger", "Chevrolet", "Volt", "CWP8032"), "don't insert the vehicle corectly");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
    "A null or empty string was inappropriately allowed.")]
        public void RouteNull()
        {
            IRoute route1 = new Route("", "PLD", 144, 1);
            IRoute route2 = new Route(null, "PLD", 144, 2);
        }

        [TestMethod]
        public void RouteNormal()
        {
            IRoute route1 = new Route("SOF", "PLD", 144, 1);

            Assert.AreEqual("SOF", route1.StartPoint, "Route start is not saved corectly");
            Assert.AreEqual("PLD", route1.EndPoint, "Route end is not saved corectly");
            Assert.AreEqual(144.0, route1.Length, "Route length  is not saved corectly");
        }

        [TestMethod]
        public void UploadRoute()
        {

            Assert.AreEqual("SOF/PLD - 144 km is unlocked in our platform."
               , controller.AllowRoute("SOF", "PLD", 144), "don't insert the route corectly");

            Assert.AreEqual("SOF/PLD - 144 km is already added in our platform."
                , controller.AllowRoute("SOF", "PLD", 144), "don't check if the route already exist");

            Assert.AreEqual("SOF/PLD shorter route is already added in our platform."
                , controller.AllowRoute("SOF", "PLD", 145), "don't check if the route is biger");

            controller.AllowRoute("SOF", "PLD", 142);
            controller.UploadVehicle("PassengerCar", "Chevrolet", "Volt", "CWP8032");
            controller.RegisterUser("Tisha", "Reenie", "7246506");

            Assert.AreEqual("Route 1 is locked! Trip is not allowed."
               , controller.MakeTrip("7246506", "CWP8032", "1", false), "don't lock the route corectly");
        }

        [TestMethod]
        public void MakeTrip()
        {
            controller.AllowRoute("SOF", "PLD", 144);
            controller.UploadVehicle("PassengerCar", "Chevrolet", "Volt", "CWP8032");
            controller.RegisterUser("Tisha", "Reenie", "7246506");
            controller.RegisterUser("Penka", "Reenie", "7246596");

            Assert.AreEqual("Chevrolet Volt License plate: CWP8032 Battery: 68% Status:damaged"
               , controller.MakeTrip("7246506", "CWP8032", "1", true), "don't damage the car corectly");

            Assert.AreEqual("User 7246506 is blocked in the platform! Trip is not allowed."
               , controller.MakeTrip("7246506", "CWP8032", "1", true), "don't chesk if the user is bloked");

            Assert.AreEqual("Vehicle CWP8032 is damaged! Trip is not allowed."
               , controller.MakeTrip("7246596", "CWP8032", "1", true), "don't chesk if the car is damaged");
        }
    }
}