using EDriveRent.Models.Contracts;
using EDriveRent.Utilities.Messages;
using System;


namespace EDriveRent
{
    public class Route: IRoute
    {
        public string startPoint;

        public string endPoint;

        public double length;

        public int routeId;

        public bool isLocked;

        public string StartPoint
        {
            get { return startPoint; }
        }

        public string EndPoint
        {
            get { return endPoint; }
        }

        public double Length
        {
            get { return length; }
        }

        public int RouteId
        {
            get { return routeId; }
        }

        public bool IsLocked
        {
            get { return isLocked; }
        }

        public void LockRoute()
        {
            isLocked = true;
        }

        public Route(string startPoint, string endPoint, double length, int routeId)
        {
            if (string.IsNullOrEmpty(startPoint))
            {
                throw new ArgumentException(ExceptionMessages.StartPointNull);
            }

            if (string.IsNullOrEmpty(endPoint))
            {
                throw new ArgumentException(ExceptionMessages.EndPointNull);
            }

            if (length < 1)
            {
                throw new ArgumentException(ExceptionMessages.RouteLengthLessThanOne);
            }

            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.length = length;
            this.routeId = routeId;
            this.isLocked = false;
        }

        public override string ToString()
        {
            string output ="StartPoint: " + StartPoint + "EndPoint: " + EndPoint + "Length: " + Length.ToString() + "RouteId: " + RouteId.ToString() ;

            if (isLocked)
            {
                output += "locked";
            }
            else
            {
                output += "OK";
            }

            return output;
        }
    }
}
