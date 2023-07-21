namespace PersonalTravelPlan_BE.Queries {
    public class FilterQuery {
        public string? search { get; set; }
        public string? status { get; set; }
        public int? countryId { get; set; }
        public int? currencyId { get; set; }
        public int? amountFrom { get; set;}
        public int? amountTo { get; set;}
        public DateOnly? startDateFrom { get; set;}
        public DateOnly? startDateTo { get; set;}
        public DateOnly? endDateFrom { get; set; }
        public DateOnly? endDateTo { get; set; }

    }
}
