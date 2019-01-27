using System;
using MonoDragons.Core;

namespace MonoDragons.GGJ.Gameplay
{
    public class NoChar : IHouseChar
    {
        public void Update(TimeSpan delta)
        {
        }

        public void Draw(Transform2 parentTransform)
        {
        }

        public Transform2 Transform => Transform2.Zero;
    }
}