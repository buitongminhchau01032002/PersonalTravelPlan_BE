namespace PersonalTravelPlan_BE.Dtos {
    public class FilePathDto {
        public string? path {  get; set; }
        public FilePathDto(string path)
        {
            this.path = path;
        }
    }
}
