using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class FootballMatchApiClient
{
    private HttpClient _client;

    public FootballMatchApiClient()
    {
        _client = new HttpClient();
    }

    public async Task<JArray> GetMatchesAsync(int year, string team, bool isTeam1)
    {
        string teamParameter = isTeam1 ? "team1" : "team2";
        string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&{teamParameter}={team}";
        HttpResponseMessage response = await _client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            JObject data = JsonConvert.DeserializeObject<JObject>(json);
            return (JArray)data["data"];
        }
        else
        {
            Console.WriteLine($"Failed to retrieve data for team {team} in year {year}.");
            return new JArray();
        }
    }
}

public class FootballMatchService
{
    private FootballMatchApiClient _apiClient;

    public FootballMatchService(FootballMatchApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<int> GetTotalGoalsAsync(string team, int year)
    {
        int totalGoals = 0;

        // Get matches where the team is team1
        JArray matchesTeam1 = await _apiClient.GetMatchesAsync(year, team, true);
        totalGoals += CalculateTotalGoals(matchesTeam1, true);

        // Get matches where the team is team2
        JArray matchesTeam2 = await _apiClient.GetMatchesAsync(year, team, false);
        totalGoals += CalculateTotalGoals(matchesTeam2, false);

        return totalGoals;
    }

    private int CalculateTotalGoals(JArray matches, bool isTeam1)
    {
        int totalGoals = 0;

        foreach (var match in matches)
        {
            int goals = isTeam1 ? (int)match["team1goals"] : (int)match["team2goals"];
            totalGoals += goals;
        }

        return totalGoals;
    }
}

public class Program
{
    public static async Task Main()
    {
        FootballMatchApiClient apiClient = new FootballMatchApiClient();
        FootballMatchService matchService = new FootballMatchService(apiClient);

        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await matchService.GetTotalGoalsAsync(teamName, year);

        Console.WriteLine($"Team {teamName} scored {totalGoals} goals in {year}");

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await matchService.GetTotalGoalsAsync(teamName, year);

        Console.WriteLine($"Team {teamName} scored {totalGoals} goals in {year}");
        // Output expected:
        // Team Paris Saint-Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }
}