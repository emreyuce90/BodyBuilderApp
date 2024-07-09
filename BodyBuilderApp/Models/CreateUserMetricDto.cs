namespace BodyBuilderApp.Models {
    public class CreateUserMetricDto {
        public Guid BodyMetricId { get; set; }
        public Guid UserId { get; set; }
        public float Value { get; set; }
    }
}
