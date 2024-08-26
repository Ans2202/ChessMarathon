using System.ComponentModel.DataAnnotations;

namespace ChessMarathon.Models
{
    public class Players
    {
        public int PlayerId { get; set; }

        public string FirstName { get; set; }

       
        public string LastName { get; set; }

        
        public string Country { get; set; }

       
        public int Current_World_Ranking { get; set; }

       
        public int TotalMatchesPlayed { get; set; }
    }
}

