namespace PersonalTravelPlan_BE.Dtos {
    public record LoginResultDto {
        public string? Username { get; set; }
        public string? Token { get; set; }
    }
}
