using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Questao2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await MostrarGolsPorAno("Paris Saint-Germain", 2013);
            await MostrarGolsPorAno("Chelsea", 2014);
            Console.ReadKey();

        }

        static async Task MostrarGolsPorAno(string time, int ano)
        {
            int gols = await ObterGols(time, ano, "team1");
            gols += await ObterGols(time, ano, "team2");

            Console.WriteLine($"Team {time} scored {gols} goals in {ano}");
        }

        static async Task<int> ObterGols(string time, int ano, string parametroTime)
        {
            int totalGols = 0;
            int pagina = 1;
            int totalPaginas;

            using (HttpClient client = new HttpClient())
            {
                do
                {
                    string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={ano}&{parametroTime}={Uri.EscapeDataString(time)}&page={pagina}";
                    var resposta = await client.GetStringAsync(url);
                    JObject json = JObject.Parse(resposta);

                    totalPaginas = (int)json["total_pages"];
                    foreach (var partida in json["data"])
                    {
                        int golsPartida = int.Parse((string)partida[$"{parametroTime}goals"], CultureInfo.InvariantCulture);
                        totalGols += golsPartida;
                    }

                    pagina++;
                } while (pagina <= totalPaginas);
            }

            return totalGols;
        }
    }
}