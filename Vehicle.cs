using EDriveRent.Models.Contracts;
using EDriveRent.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDriveRent
{
    public abstract class Vehicle : IVehicle
    {
        protected string brand;

        protected string model;

        protected double maxMileage;

        protected string licensePlateNumber;

        protected int batteryLevel;

        protected bool isDamaged;

        public string Brand
        {
            get { return brand; }
        }

        public string Model
        {
            get { return model; }
        }

        public double MaxMileage
        {
            get { return maxMileage; }
        }

        public string LicensePlateNumber
        {
            get { return licensePlateNumber; }
        }

        public int BatteryLevel
        {
            get { return batteryLevel; }
        }

        public bool IsDamaged
        {
            get { return isDamaged; }
        }

        public abstract void Drive(double mileage);

        public void Recharge()
        {
            batteryLevel = 100;
        }

        public void ChangeStatus()
        {
            isDamaged = isDamaged ? false : true;
        }

        public override string ToString()
        {
            string output = Brand + " " + Model + " License plate: " + LicensePlateNumber + " Battery: " + BatteryLevel.ToString() + "% Status: ";

            if (isDamaged)
            {
                output += "damaged";
            }
            else
            {
                output += "OK";
            }

            return output;
        }

        public Vehicle(string brand, string model, string licensePlateNumber)
        {
            if (string.IsNullOrEmpty(brand))
            {
                throw new ArgumentException(ExceptionMessages.BrandNull);
            }

            if (string.IsNullOrEmpty(model))
            {
                throw new ArgumentException(ExceptionMessages.ModelNull);
            }

            if (string.IsNullOrEmpty(licensePlateNumber))
            {
                throw new ArgumentException(ExceptionMessages.LicenceNumberRequired);
            }

            this.brand = brand;
            this.model = model;
            this.maxMileage = 0;
            this.licensePlateNumber = licensePlateNumber;
            this.batteryLevel = 100;
            this.isDamaged = false;
        }
    }

    public class PassengerCar : Vehicle
    {
        public PassengerCar(string brand, string model, string licensePlateNumber) : base(brand, model, licensePlateNumber)
        {
            this.maxMileage = 450;
        }

        public override void Drive(double mileage)
        {
            double percentageUsed = Math.Round((mileage / MaxMileage) * 100, 0, MidpointRounding.ToEven);
            batteryLevel -= (int)percentageUsed;
        }
    }
    public class CargoVan : Vehicle
    {
        public CargoVan(string brand, string model, string licensePlateNumber) : base(brand, model, licensePlateNumber)
        {
            this.maxMileage = 180;
        }

        public override void Drive(double mileage)
        {
            double percentageUsed = Math.Round((mileage / MaxMileage) * 100, 0, MidpointRounding.ToEven);
            batteryLevel -= (int)percentageUsed;

            if (BatteryLevel < 0)
            {
                batteryLevel = 0;
            }
        }
    }
}
