namespace PersonalTravelPlan_BE.Dtos {
    public record JourneyListDtos {
        public int page { get; set; }
        public int total { get; set; }
        public int totalPage { get; set; }
        public int pageSize { get; set; }
        public List<JourneyDto> data { get; set; } = new List<JourneyDto>();
    }
}
