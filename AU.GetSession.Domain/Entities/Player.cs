using AU.GetSession.Domain.Enums;

namespace AU.GetSession.Domain.Entities
{
    public class Player
    {
        public string Name { get; set; }

        public Position Position { get; set; }

        public Colour Colour { get; set; }
    }
}
