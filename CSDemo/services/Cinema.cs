using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Services
{
    [SitecoreType(AutoMap = true, TemplateId = "8D93D0D4-0410-4B67-8012-9260A319BFE2")]
    public class Cinema
    {
        public string Zipcode { get; set; }
    }
}