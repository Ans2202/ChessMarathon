using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChessMarathon.Models;

namespace ChessMarathon.DAO
{
    public interface IChessDao
    {
        Task<int> InsertMatch(Matches p);
        Task<Matches> GetMatchById(int id);

        Task<List<Players>> GetPlayersbyCountry(string Country);

        Task<List<PlayerPerformance>> GetPlayerPerformances();
        Task<List<PlayerPerformance>> GetTopPlayersByWinPercentage();
    }
}
