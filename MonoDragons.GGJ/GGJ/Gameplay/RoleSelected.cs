using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDragons.GGJ.Gameplay
{
    public struct RoleSelected
    {
        public bool IsPlayingAsHouse;

        public RoleSelected(bool isPlayingAsHouse)
        {
            IsPlayingAsHouse = isPlayingAsHouse;
        }
    }
}
