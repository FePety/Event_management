namespace Event_management.Core.Models
{
    public class Event
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public int? Capacity { get; set; }

        public Event() { }

        public Event(string name, string location, string country, int? capacity)
        {
            Name = name;
            Location = location;
            Country = country;
            Capacity = capacity;
        }
    }
}
