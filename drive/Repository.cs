using EDriveRent.Models.Contracts;
using EDriveRent.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDriveRent
{
    public class UserRepository : IRepository<IUser>
    {
        private readonly List<IUser> users = new List<IUser>();

        public void AddModel(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            users.Add(user);
        }

        public bool RemoveById(string identifier)
        {
            IUser user = users.FirstOrDefault(u => u.DrivingLicenseNumber == identifier);

            if (user == null)
            {
                return false;
            }

            users.Remove(user);
            return true;
        }

        public IUser FindById(string identifier)
        {
            return users.FirstOrDefault(u => u.DrivingLicenseNumber == identifier);
        }

        public IReadOnlyCollection<IUser> GetAll()
        {
            return users.AsReadOnly();
        }
    }

    public class VehicleRepository : IRepository<IVehicle>
    {
        private readonly List<IVehicle> vehicles = new List<IVehicle>();

        public void AddModel(IVehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new ArgumentNullException(nameof(vehicle));
            }

            vehicles.Add(vehicle);
        }

        public bool RemoveById(string identifier)
        {
            IVehicle vehicle = vehicles.FirstOrDefault(v => v.LicensePlateNumber == identifier);

            if (vehicle == null)
            {
                return false;
            }

            vehicles.Remove(vehicle);
            return true;
        }

        public IVehicle FindById(string identifier)
        {
            return vehicles.FirstOrDefault(v => v.LicensePlateNumber == identifier);
        }

        public IReadOnlyCollection<IVehicle> GetAll()
        {
            return vehicles.AsReadOnly();
        }
    }

    public class RouteRepository : IRepository<IRoute>
    {
        private readonly List<IRoute> routes = new List<IRoute>();

        public void AddModel(IRoute route)
        {
            if (route == null)
            {
                throw new ArgumentNullException(nameof(route));
            }

            routes.Add(route);
        }

        public bool RemoveById( string identifier)
        {
            IRoute route = routes.FirstOrDefault(r => r.RouteId.ToString() == identifier);

            if (route == null)
            {
                return false;
            }

            routes.Remove(route);
            return true;
        }

        public IRoute FindById(string identifier)
        {
            return routes.FirstOrDefault(r => r.RouteId.ToString() == identifier);
        }

        public IReadOnlyCollection<IRoute> GetAll()
        {
            return routes.AsReadOnly();
        }
    }
}
