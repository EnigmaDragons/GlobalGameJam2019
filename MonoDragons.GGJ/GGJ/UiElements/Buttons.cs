using System;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.AudioSystem;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Text;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.UiElements.Events;

namespace MonoDragons.GGJ.UiElements
{
    public static class Buttons
    {
        public static TextButton Text(string text, Point position, Action action) => Text(text, position, action, () => true);
        public static TextButton Text(string text, Point position, Action action, Func<bool> isVisible)
        {
            return new TextButton(
                new Rectangle(position, new Point(300, 50)),
                action,
                text,
                Color.FromNonPremultiplied(208, 42, 208, 160),
                Color.FromNonPremultiplied(208, 42, 208, 100),
                Color.FromNonPremultiplied(208, 42, 208, 40),
                isVisible
            );
        }

        public static ImageTextButton Wood(string text, Point position, Action action, Func<bool> isVisible)
        {
            return new ImageTextButton(new Transform2(position.ToVector2(), UI.OfScreenSize(0.18f, 0.09f)), 
                    () =>
                    {
                        Sound.SoundEffect("Sounds\\ButtonClick.wav").Play();
                        action();
                    }, text, "UI/sign", "UI/sign-hover", "UI/sign-press", isVisible)
                { 
                    Font = DefaultFont.Large,
                    TextColor = UiConsts.DarkBrown 
                };
        }
    }
}