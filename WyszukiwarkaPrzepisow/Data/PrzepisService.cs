using System.Data;
using Dapper;

namespace WyszukiwarkaPrzepisow.Data;

public class PrzepisService
{
    private readonly IDbConnection _dbConnection;

    public PrzepisService(IDbConnection dbConnection) => 
        _dbConnection = dbConnection;

    public List<Przepis> GetPrzepisy() {

        var przepisDictionary = new Dictionary<int, Przepis>();

        var przepisy = _dbConnection.Query<Przepis, Skladnik, Przepis>(
            @"SELECT p.id AS przepisId, p.name, p.prep, s.id AS skladnikId, s.name
                FROM Przepis p
                LEFT JOIN przepis_skladnik ps
                    ON p.id = ps.przepisId
                LEFT JOIN Skladnik s
                    ON ps.skladnikId = s.id;",
            (p, s) => 
            {
                Przepis? przepis;

                if (!przepisDictionary.TryGetValue(p.Id, out przepis))
                {
                    przepis = p;
                    przepisDictionary.Add(przepis.Id, przepis);
                }

                if (s is not null)
                {
                    przepis.Skladniki.Add(s);
                }

                return przepis;
            },
            splitOn: "skladnikId");

        return przepisy.Distinct().ToList();
    }


    public IReadOnlyList<Przepis> GetPrzepisyFromId(List<int> idPrzepisow) {

        IReadOnlyList<Przepis> wszystkiePrzepisy = GetPrzepisy();
        List<Przepis> wynik = new List<Przepis>();

        foreach (var przepis in wszystkiePrzepisy) {
            if (idPrzepisow.Contains(przepis.Id)) {
                wynik.Add(przepis);
            }
        }
        return wynik;

    }
    

    public void CreatePrzepis(CreatePrzepis model)
    {
        if (model.Name is null || model.prep is null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        // tranzakcja zeby nie dalo sie dodac samego przepisu bez jego skladnikow  
        using var transaction = _dbConnection.BeginTransaction();

        var przepisId = _dbConnection.QuerySingle<int>(
            $"INSERT INTO Przepis (name, prep) VALUES (@Name, @Prep) RETURNING id;",
            new { model.Name, model.prep });

        _dbConnection.Execute(
            $"INSERT INTO przepis_skladnik (przepisId, skladnikId) VALUES (@PrzepisId, @SkladnikId);",
            model.SkladnikiId.Select(a => new {PrzepisId = przepisId, SkladnikId = a})
        );

        transaction.Commit();
    }
}