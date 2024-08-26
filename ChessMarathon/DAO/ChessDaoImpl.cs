using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChessMarathon.Models;


namespace ChessMarathon.DAO
{
    public class ChessDaoImpl : IChessDao
    {
        private readonly NpgsqlConnection _conn;

        public ChessDaoImpl(NpgsqlConnection conn)
        {
            _conn = conn;
        }
        //1
        public async Task<int> InsertMatch(Matches match)
        {
            int rowsInserted = 0;
            const string insertQuery = @"
        INSERT INTO chess.matches (player1_id, player2_id, match_date, match_level, winner_id) 
        VALUES (@Player1Id, @Player2Id, @MatchDate, @MatchLevel, @WinnerId)";

            try
            {
                await _conn.OpenAsync();
                using var cmd = new NpgsqlCommand(insertQuery, _conn);
                cmd.Parameters.AddWithValue("@Player1Id", match.Player1Id);
                cmd.Parameters.AddWithValue("@Player2Id", match.Player2Id);
                cmd.Parameters.AddWithValue("@MatchDate", match.MatchDate);
                cmd.Parameters.AddWithValue("@MatchLevel", match.MatchLevel);
                cmd.Parameters.AddWithValue("@WinnerId", match.WinnerId ?? (object)DBNull.Value); 

                rowsInserted = await cmd.ExecuteNonQueryAsync();
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                await _conn.CloseAsync();
            }

            return rowsInserted;
        }
        

        public async Task<Matches?> GetMatchById(int id)
        {
            Matches? match = null;
            const string query = "SELECT * FROM chess.matches WHERE match_id = @Id";

            try
            {
                await _conn.OpenAsync();
                using var cmd = new NpgsqlCommand(query, _conn);
                cmd.Parameters.AddWithValue("@Id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    match = new Matches
                    {
                        MatchId = reader.GetInt32(0),
                        Player1Id = reader.GetInt32(1),
                        Player2Id = reader.GetInt32(2),
                        MatchDate = reader.GetDateTime(3),
                        MatchLevel = reader.GetString(4),
                        WinnerId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5)
                    };
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                await _conn.CloseAsync();
            }

            return match;
        }


        //2

        public async Task<List<Players>> GetPlayersbyCountry(string country)
        {
            var playersList = new List<Players>();
            const string query = @"
                SELECT player_id, first_name, last_name, country, current_world_ranking, total_matches_played
                FROM chess.players
                WHERE country = @Country";

            try
            {
                await _conn.OpenAsync();
                using var cmd = new NpgsqlCommand(query, _conn);
                cmd.Parameters.AddWithValue("@Country", country);

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var player = new Players
                    {
                        PlayerId = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Country = reader.GetString(3),
                        Current_World_Ranking = reader.GetInt32(4),
                        TotalMatchesPlayed = reader.GetInt32(5)
                    };

                    playersList.Add(player);
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                await _conn.CloseAsync();
            }

            return playersList;
        }

        //3
        public async Task<List<PlayerPerformance>> GetPlayerPerformances()
        {
            var performances = new List<PlayerPerformance>();
            const string query = @"
                SELECT 
                    CONCAT(p.first_name, ' ', p.last_name) AS FullName,
                    p.total_matches_played AS TotalMatchesPlayed,
                    COUNT(CASE WHEN m.winner_id = p.player_id THEN 1 END) AS TotalMatchesWon,
                    ROUND(COUNT(m.winner_id) * 100.0 / p.total_matches_played, 2) AS WinPercentage
                FROM chess.Players p
                LEFT JOIN chess.Matches m ON p.player_id = m.player1_id OR p.player_id = m.player2_id
                GROUP BY p.player_id, p.first_name, p.last_name
                ORDER BY p.current_world_ranking";

            try
            {
                await _conn.OpenAsync();
                using var cmd = new NpgsqlCommand(query, _conn);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    performances.Add(new PlayerPerformance
                    {
                        FullName = reader.GetString(0),
                        TotalMatchesPlayed = reader.GetInt32(1),
                        TotalMatchesWon = reader.GetInt32(2),
                        WinPercentage = reader.GetDouble(3)
                    });
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                await _conn.CloseAsync();
            }

            return performances;
        }

        public async Task<List<PlayerPerformance>> GetTopPlayersByWinPercentage()
        {
            var topPlayers = new List<PlayerPerformance>();
            const string query = @"
                SELECT 
                    CONCAT(p.first_name, ' ', p.last_name) AS FullName,
                    p.total_matches_played AS TotalMatchesPlayed,
                    COUNT(CASE WHEN m.winner_id = p.player_id THEN 1 END) AS TotalMatchesWon,
                    ROUND(COUNT(m.winner_id) * 100.0 / p.total_matches_played, 2) AS WinPercentage
                FROM chess.Players p
                LEFT JOIN chess.Matches m ON p.player_id = m.player1_id OR p.player_id = m.player2_id
                GROUP BY p.player_id, p.first_name, p.last_name
                HAVING ROUND(COUNT(m.winner_id) * 100.0 / p.total_matches_played, 2) > @MinWinPercentage
                ORDER BY WinPercentage DESC";

            try
            {
                await _conn.OpenAsync();
                using var cmd = new NpgsqlCommand(query, _conn);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    topPlayers.Add(new PlayerPerformance
                    {
                        FullName = reader.GetString(0),
                        TotalMatchesPlayed = reader.GetInt32(1),
                        TotalMatchesWon = reader.GetInt32(2),
                        WinPercentage = reader.GetDouble(3)
                    });
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                await _conn.CloseAsync();
            }

            return topPlayers;
        }

        

       
    }
}
