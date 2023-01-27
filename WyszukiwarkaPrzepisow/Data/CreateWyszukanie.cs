using System.ComponentModel.DataAnnotations;

namespace WyszukiwarkaPrzepisow.Data;

public sealed class CreateWyszukanie
{  
     public IEnumerable<int> WybraneSkladnikiId { get; set; } = Enumerable.Empty<int>();

}