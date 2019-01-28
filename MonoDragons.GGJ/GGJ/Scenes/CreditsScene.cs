using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Animations;
using MonoDragons.Core.AudioSystem;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.Network;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Credits;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Scenes
{
    public class CreditsScene : ClickUiScene
    {                  
        private readonly Queue<IAnimation> _animations = new Queue<IAnimation>();
        private TimerTask _timer;
        private bool _animationsAreFinished;
        private bool _shouldDisplayButtons;

        private readonly Optional<Player> _winner;
        
        public CreditsScene(Optional<Player> winner)
        {
            _winner = winner;
        }

        public override void Init()
        {            
            Sound.Music("credits").Play();
            Add(new Sprite {Image = "Outside/desert_bg", Transform = new Transform2(UI.OfScreenSize(1.0f, 1.0f))});
            Add(new Sprite {Image = "Outside/desert_front", Transform = new Transform2(UI.OfScreenSize(1.0f, 1.0f))});
            Add(new Sprite { Image = "UI/title", Transform = new Transform2(new Vector2((1600 - 720) / 2, UI.OfScreenHeight(0.062f)), new Size2(720, 355)), IsActive = () => _animationsAreFinished});

            Add(Buttons.Wood("Main Menu", UI.OfScreenSize(0.41f, 0.79f).ToPoint(), () =>
                Scene.NavigateTo(new MainMenuScene(new NetworkArgs())), () => _shouldDisplayButtons));
            
            Input.On(Control.Start, () => Scene.NavigateTo("MainMenu"));
            Input.On(Control.Select, () => Scene.NavigateTo("MainMenu"));
            Add(new ScreenClickable(() => Scene.NavigateTo("MainMenu")));

            if (_winner.HasValue)
                AddAnimation(new WinnerSegment(_winner.Value));
            AddAnimation(new TitleCreditSegment(() => _shouldDisplayButtons = true));
            
            // Leads
            AddAnimation(new LeadGameDesignerCredit());
            AddAnimation(new LeadProgrammerCredit());
            
            // Gameplay Design
            AddAnimation(new GameplayProgrammerCredit());
            AddAnimation(new GameplayDesigner1Credit());
            AddAnimation(new GameplayDesigner2Credit());
            
            // Front-End
            AddAnimation(new UiDesignerCredit());
            
            // Back-End
            AddAnimation(new NetCodeProgrammerCredit());
            
            // Music
            AddAnimation(new ComposerCredit());
            
            // Project
            AddAnimation(new ProjectManagerCredit());
            AddAnimation(new Tester1Credit());

            _timer = new TimerTask(StartNext, 600, recurring: false);
            Add(_timer);
        }

        private void AddAnimation(IAnimation anim)
        {
            _animations.Enqueue(anim);
            Add(anim);
        }

        private void QueueNext()
        {
            _timer.Reset();
        }

        private void StartNext()
        {
            if (_animations.Count > 0)
                _animations.Dequeue().Start(QueueNext);
            else
                _animationsAreFinished = true;
        }
    }

    public abstract class JamScene : IScene
    {
        private readonly List<IVisual> _visuals = new List<IVisual>();
        private readonly List<IAutomaton> _automata = new List<IAutomaton>();
    
        private ClickUI _clickUi;
    
        protected abstract void OnInit();
        protected abstract void DrawBackground();
        protected abstract void DrawForeground();
    
        public void Init()
        {
            _clickUi = new ClickUI();
            _automata.Add(_clickUi);
            OnInit();
        }
    
        public void Update(TimeSpan delta)
        {
            _automata.ForEach(x => x.Update(delta));
            OnUpdate(delta);
        }
    
        protected virtual void OnUpdate(TimeSpan delta)
        {
        }
    
        public void Draw()
        {
            DrawBackground();
            _visuals.ForEach(x => x.Draw());
            DrawForeground();
        }
    
        protected void AddVisual(IVisual v)
        {
            _visuals.Add(v);
        }
    
        protected void AddUi(ClickableUIElement e)
        {
            _clickUi.Add(e);
        }
    
        protected void Add(VisualClickableUIElement e)
        {
            _clickUi.Add(e);
            _visuals.Add(e);
        }
    
        protected void Add(IVisualAutomaton v)
        {
            _automata.Add(v);
            _visuals.Add(v);
        }
    
        protected void Add(IAutomaton a)
        {
            _automata.Add(a);
        }
    
        protected void Add(IVisual v)
        {
            _visuals.Add(v);
        }
    
        public void Dispose()
        {
            _clickUi.Dispose();
        }
    }
}
