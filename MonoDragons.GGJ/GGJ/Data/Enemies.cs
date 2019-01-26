using System.Collections.Generic;
using MonoDragons.Core.Engine;
using MonoDragons.GGJ.Gameplay;

namespace MonoDragons.GGJ.Data
{
    public enum Enemy
    {
        Bed,
    }
    
    public static class Enemies
    {
//        private static Dictionary<Enemy, Sprite> _sprites = new Dictionary<Enemy, Sprite>
//        {
//            {Enemy.Bed, }
//        };
        
        public static IVisualAutomaton Create(Enemy enemy)
        {
            if (enemy == Enemy.Bed)
                return new Bed();
            throw new KeyNotFoundException($"Unknown Enemy: {enemy}");
        }
    }
}