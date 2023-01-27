using System.ComponentModel.DataAnnotations;

namespace WyszukiwarkaPrzepisow.Data;

public sealed class CreateSkladnik
{
    [Required]
    [StringLength(100)]
    public string? Name { get; set; }

}