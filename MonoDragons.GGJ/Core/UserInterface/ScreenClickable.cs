﻿using System;
using Microsoft.Xna.Framework;

namespace MonoDragons.Core.UserInterface
{
    public sealed class ScreenClickable : ClickableUIElement
    {
        private readonly Action _onClick;

        public ScreenClickable(Action onClick, float scale = 1)
            : base(new Rectangle(0, 0, 5000, 5000), scale)
        {
            _onClick = onClick;
        }

        public override void OnEntered()
        {
        }

        public override void OnExitted()
        {
        }

        public override void OnPressed()
        {
        }

        public override void OnReleased()
        {
            _onClick();
        }
    }
}
