
namespace FlagsApp.Models
{
    public class Lines
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Lines lines &&
                   Id == lines.Id &&
                   Name == lines.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}
