using System.ComponentModel.DataAnnotations;

namespace BodyBuilderApp.Extensions; 
public class ElasticSearchSettings {
    [Required] public string IndexName { get; set; } = default!;
    [Required] public string UserName { get; set; } = default!;
    [Required] public string Password { get; set; } = default!;
    [Required] public string BaseUrl { get; set; } = default!;
}
