using EDriveRent.Models.Contracts;
using EDriveRent.Utilities.Messages;
using System;


namespace EDriveRent
{
    public class User : IUser
    {
        private string firstName;

        private string lastName;

        private double rating;

        private string drivingLicenseNumber;

        private bool isBlocked;

        public string FirstName
        {
            get { return firstName; }
        }

        public string LastName
        {
            get { return lastName; }
        }

        public double Rating
        {
            get { return rating; }
        }

        public string DrivingLicenseNumber
        {
            get { return drivingLicenseNumber; }
        }

        public bool IsBlocked
        {
            get { return isBlocked; }
        }

        public void IncreaseRating()
        {
            rating += 0.5;

            if (rating > 10)
            {
                rating = 10.0;
            }
        }

        public void DecreaseRating()
        {
            rating -= 2.0;

            if (rating < 0.0)
            {
                rating = 0.0;
                isBlocked = true;
            }
        }

        public override string ToString()
        {
            return FirstName + " " + LastName + " Driving license: " + drivingLicenseNumber + " Rating: " + rating.ToString();
        }

        public User(string firstName, string lastName, string drivingLicenseNumber)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException(ExceptionMessages.FirstNameNull);
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException(ExceptionMessages.LastNameNull);
            }

            if (string.IsNullOrEmpty(drivingLicenseNumber))
            {
                throw new ArgumentException(ExceptionMessages.DrivingLicenseRequired);
            }

            this.firstName = firstName;
            this.lastName = lastName;
            this.drivingLicenseNumber = drivingLicenseNumber;
            this.rating = 0.0;
            this.isBlocked = false;
        }
    }
}
