namespace MonoDragons.Core.Network
{
    public class NetworkArgs
    {
        public bool ShouldConnect { get; }
        public string Ip { get; }
        public int Port { get; }

        public NetworkArgs(string[] args)
        {
            ShouldConnect = args.Length >= 2;
            if (ShouldConnect)
            {
                Ip = args[0];
                Port = int.Parse(args[1]);
            }
        }
    }
}
