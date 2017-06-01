using System.Runtime.Serialization;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Services
{
    [SitecoreType(AutoMap = true), DataContract]
    public class Movie
    {
        [SitecoreField(Fields.VariantId), DataMember]
        public string Id { get; set; }
        [SitecoreField(Fields.MovieName), DataMember]
        public string Title { get; set; }
        [SitecoreField(Fields.Price), DataMember]
        public double Price { get; set; }
        [SitecoreField(Fields.ShowDate), DataMember]
        public string ShowDate { get; set; }
        [SitecoreField(Fields.ShowTime), DataMember]
        public string ShowTime { get; set; }
        [SitecoreField(Fields.CinemaName), DataMember]
        public string CinemaName { get; set; }
        [SitecoreField(Fields.CinemaZipCode), DataMember]
        public string CinemaZipcode { get; set; }
        [SitecoreField(Fields.CinemaId), DataMember]
        public string CinemaId { get; set; }

        public struct Fields
        {
            public const string VariantId = "_group";
            public const string MovieName = "moviename";
            public const string Price = "listprice";
            public const string ShowDate = "showdate";
            public const string ShowTime = "showtime";
            public const string CinemaName = "movielocationname";
            public const string CinemaZipCode = "moviezipcode";
            public const string CinemaId = "locationid";
        }
    }
}