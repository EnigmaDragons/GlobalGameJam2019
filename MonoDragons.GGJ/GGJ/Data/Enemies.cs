using System.Collections.Generic;
using MonoDragons.Core.Engine;
using MonoDragons.GGJ.Gameplay;

namespace MonoDragons.GGJ.Data
{
    public enum Enemy
    {
        Bed,
        Computer,
    }
    
    public static class Enemies
    {        
        public static IHouseChar Create(Enemy enemy)
        {
            if (enemy == Enemy.Bed)
                return new Bed();
            if (enemy == Enemy.Computer)
                return new Computer();
            throw new KeyNotFoundException($"Unknown Enemy: {enemy}");
        }
    }
}