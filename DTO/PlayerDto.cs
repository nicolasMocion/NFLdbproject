namespace EspnBackend.DTO{

    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int CollegeId { get; set; }
        public int TeamId { get; set; }
        public int HomeTownCityId { get; set; }
        public int Number { get; set; }

        // Add these display properties
        public string? TeamName { get; set; }
        public string? CollegeName { get; set; }
        public string? HomeTownName { get; set; }
    }

}


