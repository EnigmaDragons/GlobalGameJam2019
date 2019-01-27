using System;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Text;
using MonoDragons.Core.UserInterface;

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

        public static ImageTextButton Wood(string text, Point position, Action action)
        {
            return new ImageTextButton(new Transform2(position.ToVector2(), UI.OfScreenSize(0.18f, 0.09f)), 
                    action, text, "UI/sign", "UI/sign-hover", "UI/sign-press")
                { 
                    Font = DefaultFont.Large,
                    TextColor = UiConsts.DarkBrown 
                };
        }
    }
}