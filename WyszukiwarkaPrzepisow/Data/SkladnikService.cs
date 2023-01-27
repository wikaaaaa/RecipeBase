using System.Data;
using Dapper;
namespace WyszukiwarkaPrzepisow.Data;

public class SkladnikService
{
    private readonly IDbConnection _dbConnection;

    public SkladnikService(IDbConnection dbConnection) => 
        _dbConnection = dbConnection;

    public IReadOnlyList<Skladnik> GetSkladniki() =>
        _dbConnection.Query<Skladnik>("SELECT id AS skladnikId, name FROM Skladnik order by name").ToList();

    public bool CreateSkladnik(CreateSkladnik model)
    {
        if (model.Name is null )
        {
            throw new ArgumentNullException(nameof(model));
        }

        bool newSkladnik = true;

        foreach (var skladnik in GetSkladniki()) {
            if (model.Name == skladnik.Name) {
                // nie dodajemy drugi raz produktu o tej samej nazwie 
                newSkladnik = false;
            }
        }

        if (newSkladnik) {
            _dbConnection.Execute(
            $"INSERT INTO Skladnik (name) VALUES (@Name);",
            new { model.Name });
        }
       return newSkladnik;
    }
}