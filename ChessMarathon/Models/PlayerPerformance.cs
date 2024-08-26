using System.ComponentModel.DataAnnotations;

namespace ChessMarathon.Models
{
    public class PlayerPerformance
    {
        
        public string FullName { get; set; }

        
        public int TotalMatchesPlayed { get; set; }


        public int TotalMatchesWon { get; set; }

 
        public double WinPercentage { get; set; }
    }
}

