namespace EspnBackend.DTO
{
    public class StadiumDTO
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string CityName { get; set; } // We'll look up City by name
    }
}