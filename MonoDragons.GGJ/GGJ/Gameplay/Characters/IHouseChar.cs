using MonoDragons.Core;
using MonoDragons.Core.Engine;

namespace MonoDragons.GGJ.Gameplay
{
    public interface IHouseChar : IVisualAutomaton
    {
        Transform2 Transform { get; }
    }
}