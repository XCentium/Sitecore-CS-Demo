using System.Runtime.Serialization;

namespace CSDemo.Services
{
    [DataContract]
    public class MovieOrder
    {
        [DataMember]
        public string MovieVariantId { get; set; }
        [DataMember]
        public string NoOfTickets { get; set; }
        [DataMember]
        public bool IsCustomerAnonymous { get; set; }
        [DataMember]
        public string CustomerUsername { get; set; }
        [DataMember]
        public string CustomerEmailAddress { get; set; }
        [DataMember]
        public bool IsOrderSuccessful { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string OrderNo { get; set; }
        [DataMember]
        public string OrderDateTime { get; set; }
    }
}