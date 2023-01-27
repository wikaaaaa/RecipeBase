using System.ComponentModel.DataAnnotations;

namespace WyszukiwarkaPrzepisow.Data;

public sealed class CreatePrzepis
{
    [Required]
    [StringLength(100)]
    public string? Name { get; set; }

    [Required]
    [StringLength(4000)]
    public string? prep { get; set; }

    public IEnumerable<int> SkladnikiId { get; set; } = Enumerable.Empty<int>();
}