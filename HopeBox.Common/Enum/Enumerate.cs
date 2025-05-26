using System.Text.Json.Serialization;

namespace HopeBox.Common.Enum
{
    public class Enumerate
    {
        public enum Gender
        {
            Unknown = 0,
            Male = 1,
            Female = 2,
            Other = 3
        }

        public enum UserStatus
        {
            Active = 1,
            Inactive = 2,
            Suspended = 3,
            Banned = 4,
            Deleted = 5
        }

        public enum VolunteerStatus
        {
            Pending = 1,
            Approved = 2,
            Rejected = 3
        }

        public enum CausesStatus
        {
            Pending = 1,
            Approved = 2,
            Ongoing = 3,
            Completed = 4,
            Canceled = 5
        }
        public enum CausesType
        {
            Water = 1,
            Food = 2,
            Medicine = 3,
            Education = 4,
            Shelter = 5,
            Clothing = 6
        }

        public enum EventStatus
        {
            Upcoming = 1,
            Ongoing = 2,
            Completed = 3,
            Cancelled = 4
        }

        public enum MediaType
        {
            Image = 1,
            Video = 2,
            Document = 3,
        }

        public enum Unit
        {
            kg = 1,
            Pack = 2,
            Bag = 3,
            Box = 4,
            Bottle = 5,
            Carton = 6
        }

        public enum PaymentMethod
        {
            VNPAY = 1,
            MOMO = 2
        }
    }
}
