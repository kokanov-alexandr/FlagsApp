
namespace FlagsApp.Models
{
    public class Flag
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string ImageSrc { get; set; }
        public string? ImageName { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Flag flag &&
                   Id == flag.Id &&
                   CountryName == flag.CountryName &&
                   ImageSrc == flag.ImageSrc &&
                   ImageName == flag.ImageName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, CountryName, ImageSrc, ImageName);
        }
    }
}
