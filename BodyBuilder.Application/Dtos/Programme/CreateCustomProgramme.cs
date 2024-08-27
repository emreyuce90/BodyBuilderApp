using BodyBuilder.Domain.Entities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

public class CreateCustomProgramme {
   
    public string ProgrammeName { get; set; }

    public List<SubProgramme> SubProgramme { get; set; }
}

public class SubProgramme {
   

    public int Id { get; set; }

    public List<SubProgrammeMovementDto> SubProgrammeMovements { get; set; }

    public string SubProgrammeName { get; set; }
}

public class SubProgrammeMovementDto {
    public int Reps { get; set; }

    public int Sets { get; set; }

    public Guid MovementId { get; set; }

    public MovementModel Movement { get; set; }
}

public class MovementModel {
    public Guid Id { get; set; }
    public string Title { get; set; } 
    public string Description { get; set; }
    public string? Tip { get; set; } 
    public string ImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public Guid BodyPartId { get; set; }
    public Guid? SubBodyPartId { get; set; }

   
}