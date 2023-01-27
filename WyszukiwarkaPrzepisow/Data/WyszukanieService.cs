using System.Data;
using Dapper;

namespace WyszukiwarkaPrzepisow.Data;

public class WyszukanieService
{
    private readonly IDbConnection _dbConnection;

    public WyszukanieService(IDbConnection dbConnection) => 
        _dbConnection = dbConnection;

   

    public List<int> CreateWyszukanie(CreateWyszukanie model)
    {
        if (model.WybraneSkladnikiId is null )
        {
            throw new ArgumentNullException(nameof(model));
        }

       // TO DO:  dodac do bazy dane o wyskzuaniu

        List<int> idPrzepisow = new List<int>();

        var first = true;

        // szukamy przepisów w których są wszystkie wybrane składniki (moze byc więcej)
        foreach (var skladnik in model.WybraneSkladnikiId) {

            IEnumerable<int> przepisyLocal = new List<int>();

            przepisyLocal = _dbConnection.Query<int>(
                @$"select przepisId from przepis_skladnik where skladnikId = @id;", new { id = skladnik });

            if (first) {
                first = false;
                foreach (int i in przepisyLocal) {
                    idPrzepisow.Add(i);
                }
            } else {
                List<int> help = new List<int>();
                foreach (int i in przepisyLocal) {
                    if (idPrzepisow.Contains(i)) {
                        help.Add(i);
                    }
                }
                idPrzepisow = help;
            }

        }
        return idPrzepisow;
    }
}