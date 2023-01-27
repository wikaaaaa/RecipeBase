namespace WyszukiwarkaPrzepisow.Data;

public class Skladnik
{
    public int Id { get; init; }

    public string Name { get; init; }

    public Skladnik(int skladnikId, string name) =>
        (Id, Name) = (skladnikId, name);
}