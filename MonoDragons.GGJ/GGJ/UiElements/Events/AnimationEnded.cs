using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDragons.GGJ.UiElements.Events
{
    public class AnimationEnded
    {
        public string Name { get; }

        public AnimationEnded(string name) => Name = name;
    }
}
