using System;

namespace MonoDragons.Core.Engine
{
    public class OnlyOnceAutomaton : IAutomaton
    {
        private bool _done;
        private readonly Action<TimeSpan> _action;

        public OnlyOnceAutomaton(Action action) : this(x => action()) {}
        public OnlyOnceAutomaton(Action<TimeSpan> action)
        {
            _action = action;
        }
        
        public void Update(TimeSpan delta)
        {
            if (_done)
                return;
            
            _done = true;
            _action(delta);
        }
    }
}