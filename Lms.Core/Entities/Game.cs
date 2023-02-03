namespace Lms.Core.Entities
{
    public class Game
    {
        public int GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
       public DateTime Time { get; set; }

        //Foreign Keys
        public int TournamentId { get; set; }

    }
}