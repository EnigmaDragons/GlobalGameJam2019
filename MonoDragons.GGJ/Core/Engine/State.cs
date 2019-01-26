namespace MonoDragons.Core
{
    public static class State<T>
    {
        private static readonly MustInit<T> _value = new MustInit<T>("GameState");

        public static void Init(T state) => _value.Init(state);
        public static void Clear() => _value.Clear();
        public static T Current => _value.Get();
    }
}