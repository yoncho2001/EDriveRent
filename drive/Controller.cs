using EDriveRent.Core.Contracts;
using EDriveRent.Models.Contracts;
using EDriveRent.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EDriveRent
{
    public class Controller : IController
    {
        private UserRepository users;
        private VehicleRepository vehicles;
        private RouteRepository routes;

        public Controller()
        {
            users = new UserRepository();
            vehicles = new VehicleRepository();
            routes = new RouteRepository();
        }

        public string RegisterUser(string firstName, string lastName, string drivingLicenseNumber)
        {
            if (users.FindById(drivingLicenseNumber) != null)
            {
                return string.Format(OutputMessages.UserWithSameLicenseAlreadyAdded, drivingLicenseNumber);
            }

            IUser user = new User(firstName, lastName, drivingLicenseNumber);

            users.AddModel(user);
            return string.Format(OutputMessages.UserSuccessfullyAdded, firstName, lastName, drivingLicenseNumber);
        }

        public string UploadVehicle(string vehicleType, string brand, string model, string licensePlateNumber)
        {
            if (vehicles.FindById(licensePlateNumber) != null)
            {
                return string.Format(OutputMessages.LicensePlateExists, licensePlateNumber);
            }

            IVehicle vehicle;

            switch (vehicleType)
            {
                case "PassengerCar":
                    vehicle = new PassengerCar(brand, model, licensePlateNumber);
                    break;
                case "CargoVan":
                    vehicle = new CargoVan(brand, model, licensePlateNumber);
                    break;
                default:
                    return string.Format(OutputMessages.VehicleTypeNotAccessible, vehicleType);
            }

            vehicles.AddModel(vehicle);
            return string.Format(OutputMessages.VehicleAddedSuccessfully, brand, model, licensePlateNumber);
        }

        public string AllowRoute(string startPoint, string endPoint, double length)
        {
            foreach (var route in routes.GetAll())
            {
                if (route.StartPoint == startPoint && route.EndPoint == endPoint && route.Length == length)
                {
                    return string.Format(OutputMessages.RouteExisting, startPoint, endPoint, length);
                }

                if (route.StartPoint == startPoint && route.EndPoint == endPoint && route.Length < length)
                {
                    return string.Format(OutputMessages.RouteIsTooLong, startPoint, endPoint, length);
                }

                if (route.StartPoint == startPoint && route.EndPoint == endPoint && route.Length > length)
                {
                    route.LockRoute();
                }
            }

            IRoute newRoute = new Route(startPoint,endPoint,length, routes.GetAll().Count + 1);
            routes.AddModel(newRoute);

            return string.Format(OutputMessages.NewRouteAdded, startPoint, endPoint, length);
        }

        public string MakeTrip(string drivingLicenseNumber, string licensePlateNumber, string routeId, bool isAccidentHappened)
        {
            if (users.FindById(drivingLicenseNumber) == null)
            {
                return string.Format("{0} is not registered in our platform.", drivingLicenseNumber);
            }

            if (vehicles.FindById(licensePlateNumber) == null)
            {
                return string.Format("{0} is not registered in our platform.", licensePlateNumber);
            }

            if (routes.FindById(routeId) == null)
            {
                return string.Format("{0} is not registered in our platform.", routeId);
            }

            if (users.FindById(drivingLicenseNumber).IsBlocked == true)
            {
                return string.Format(OutputMessages.UserBlocked, drivingLicenseNumber);
            }

            if (vehicles.FindById(licensePlateNumber).IsDamaged == true)
            {
                return string.Format(OutputMessages.VehicleDamaged, licensePlateNumber);
            }

            if (routes.FindById(routeId).IsLocked == true)
            {
                return string.Format(OutputMessages.RouteLocked, routeId);
            }

            vehicles.FindById(licensePlateNumber).Drive(routes.FindById(routeId).Length);

            if (isAccidentHappened)
            {
                vehicles.FindById(licensePlateNumber).ChangeStatus();
                users.FindById(drivingLicenseNumber).DecreaseRating();
            }
            else
            {
                users.FindById(drivingLicenseNumber).IncreaseRating();
            }

            IVehicle tempV = vehicles.FindById(licensePlateNumber);
            string output = string.Format("{0} {1} License plate: {2} Battery: {3}% Status:", tempV.Brand, tempV.Model, tempV.LicensePlateNumber, tempV.BatteryLevel);

            if (tempV.IsDamaged)
            {
                output += "damaged";
            }
            else
            {
                output += "OK";
            }

            return output;
        }

        public string RepairVehicles(int count)
        {
             List<IVehicle> damagedVehicles = vehicles.GetAll()
                                              .Where(v => v.IsDamaged)
                                              .OrderBy(v => v.Brand)
                                              .ThenBy(v => v.Model)
                                              .Take(count)
                                              .ToList();

            foreach (var vehicle in damagedVehicles)
            {
                vehicle.ChangeStatus();
                vehicle.Recharge();
            }

            return string.Format(OutputMessages.RepairedVehicles, damagedVehicles.Count);
        }
    

        public string UsersReport()
        {
            List<IUser> usersReport = users.GetAll()
                  .OrderByDescending(u => u.Rating)
                  .ThenBy(u => u.LastName)
                  .ThenBy(u => u.FirstName)
                  .ToList();

            string output = "*** E-Drive-Rent ***\n";
            foreach (var user in usersReport)
            {
                output += user.ToString()+"\n";
            }

            return output.ToString().TrimEnd();
        }
    }
}
