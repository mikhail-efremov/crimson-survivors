using System.Collections.Generic;

namespace DefaultNamespace
{
    public class Battle : MonoGlobalSingleton<Battle>
    {
        public Player Player;
        public List<Enemy> Enemies = new List<Enemy>();
    }
}