using System;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Animations;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay.Events;
using MonoDragons.GGJ.UiElements.Events;

namespace MonoDragons.GGJ.UiElements
{
    public class CounteredEffect : IAnimation
    {
        private readonly HorizontalFlyInAnimation _anim = new HorizontalFlyInAnimation(
            new UiImage
            { 
                Image = "UI/Countered", 
                Transform = new Transform2(new Vector2(-800, UI.OfScreenHeight(0.2f)), new Size2(600, 121))
            });

        public CounteredEffect()
        {
            Event.Subscribe<CardCountered>(x => Start(() => { }), this);
        }

        public void Update(TimeSpan delta)
        {
            _anim.Update(delta);
        }

        public void Draw(Transform2 parentTransform)
        {
            _anim.Draw(parentTransform);
        }

        public void Start(Action onFinished)
        {
            Event.Publish(new AnimationStarted("Countered!"));
            _anim.Start(() =>
            {
                Event.Publish(new AnimationEnded("Countered!"));
                onFinished();
            });
        }
    }
}