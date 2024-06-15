using System.ComponentModel.DataAnnotations;

namespace GameStore.Dtos;

public record CreateGameDto(
    
    [Required][StringLength(100)] string Name,
    [Required][StringLength(100)]string Genre,
    [Required][Range(1,100)]decimal Price,
    [Required] DateOnly ReleaseDate
);