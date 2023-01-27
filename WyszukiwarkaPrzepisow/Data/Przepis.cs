namespace WyszukiwarkaPrzepisow.Data;

public class Przepis
{
    public int Id { get; init; }

    public string Name { get; init; }

    public string Prep { get; init; }


    public HashSet<Skladnik> Skladniki { get; init; } = new HashSet<Skladnik>();

    public string SkladnikiLista => string.Join(", ", Skladniki.Select(a => a.Name));


    public Przepis(int przepisId, string name, string prep) =>
        (Id, Name, Prep) = (przepisId, name, prep);
}