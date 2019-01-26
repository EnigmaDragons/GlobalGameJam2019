using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.UserInterface;
using System;

namespace MonoDragons.GGJ.UiElements
{
    class LevelBackground : IVisualAutomaton
    {
        private readonly Sprite _bg;        

        public LevelBackground(string initial)
        {
            _bg = new Sprite { Image = initial, Transform = new Transform2(GetBgSize()) };            
        }

        public void Draw(Transform2 parentTransform)
        {
            _bg.Draw(parentTransform);
        }

        public void Update(TimeSpan delta)
        {
            _bg.Transform.Size = GetBgSize();
        }

        private Size2 GetBgSize()
        {
            var screenWidth = UI.OfScreenWidth(1.0f);
            var size = new Size2(screenWidth, (int)(screenWidth * (1 / UiConsts.BgRatio)));
            return size;
        }
    }
}
