using Glass.Mapper.Sc.Fields;
namespace CSDemo.Contracts.Marquee
{
    public interface ICarouselItem
    {
        Image Image { get; set; }
        string Content { get; set; }
    }
}