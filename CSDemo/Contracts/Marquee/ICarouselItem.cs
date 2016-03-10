using Glass.Mapper.Sc.Fields;
namespace CSDemo.Contracts.Marquee
{
    public interface ICarouselItem//:IEditableBase
    {
        Image Image { get; set; }
        string Content { get; set; }
    }
}