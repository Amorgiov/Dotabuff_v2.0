using System.Diagnostics.CodeAnalysis;

namespace Dotabuff_2._0.Models
{
    public class Match
    {
        public int Id { get; set; }
        public string League { get; set; }
        public string MatchId { get; set; }
        public string Date { get; set; }
        public string Series { get; set; }
        public string RadiantTeam { get; set; }
        public string DireTeam { get; set; }
        public string Duration { get; set; }
    }
}
