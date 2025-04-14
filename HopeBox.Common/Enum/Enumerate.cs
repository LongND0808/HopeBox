using System.Text.Json.Serialization;

namespace HopeBox.Common.Enum
{
    public class Enumerate
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Gender
        {
            Unknown = 0,
            Male = 1,
            Female = 2,
            Other = 3
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum UserStatus
        {
            Active = 1,
            Inactive = 2,
            Suspended = 3,
            Banned = 4,
            Deleted = 5
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum VolunteerStatus
        {
            Pending = 1,
            Approved = 2,
            Rejected = 3
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum CampaignStatus
        {
            Pending = 1,
            Approved = 2,
            Ongoing = 3,
            Completed = 4,
            Canceled = 5
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum MediaType
        {
            Image = 1,
            Video = 2,
            Document = 3,
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Unit
        {
            kg = 1,
            Pack = 2,
            Bag = 3,
            Box = 4,
            Bottle = 5,
            Carton = 6
        }  

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum PaymentMethod
        {
            VNPAY = 1,
            MOMO = 2
        }
    }
}
