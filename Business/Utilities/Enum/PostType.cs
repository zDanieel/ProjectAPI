using System.ComponentModel;

namespace Business.Utilities.Enum
{
    public enum PostType
    {
        [Description("Farándula")]
        Farandula = 1,

        [Description("Política")]
        Politica = 2,

        [Description("Fútbol")]
        Futbol = 3,
    }
}
