using MonoDragons.GGJ.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDragons.GGJ.AI
{
    public abstract class AI
    {
        private Player _player;
        private CharacterState _characterState;

        protected AI(Player player)
        {
            _player = player;
        }
    }
}
