namespace FlagsApp.Models
{
    public class GraphicElement
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is GraphicElement element &&
                    Id == element.Id &&
                    Name == element.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}
