using System.ComponentModel.DataAnnotations;

namespace BodyBuilderApp.OpenTelemetry {
    public class OpenTelemetrySettings {
        
        [Required] public string ServiceName { get; set; } = default!;
        
        [Required] public string ServiceVersion { get; set; } = default!;
      
        [Required] public string ActivitySourceName { get; set; } = default!;
    }
}
