using MonoDragons.Core.Engine;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.Core.Scenes
{
    public abstract class ClickUiScene : SceneContainer, IScene
    {
        protected ClickUI ClickUi = new ClickUI();

        public ClickUiScene()
        {
            Add(ClickUi);
        }

        public abstract void Init();
        public virtual void Dispose() {}

        public void Draw() => Draw(Transform2.Zero);

        public void Add(IVisualAutomatonControl obj)
        {
            Add((IVisual)obj);
            Add((IAutomaton)obj);
            ClickUi.Add(obj.Branch);
        }

        public void Add(IVisualControl obj)
        {
            Add((IVisual)obj);
            ClickUi.Add(obj.Branch);
        }
        
        public void Add(ClickableUIElement obj)
        {
            ClickUi.Add(obj);
            if (obj is IVisual)
                Add((IVisual)obj);
            if (obj is IAutomaton)
                Add((IAutomaton)obj);
        }
    }
}
