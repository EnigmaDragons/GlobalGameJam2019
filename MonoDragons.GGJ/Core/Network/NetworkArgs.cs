namespace MonoDragons.Core.Network
{
    public class NetworkArgs
    {
        public bool ShouldHost { get; }
        public bool ShouldAutoLaunch { get; }
        public string Ip { get; }
        public int Port { get; }

        public NetworkArgs(string[] args)
            : this(args.Length >= 2, false, args.Length >= 2 ? args[0] : "", args.Length >= 2 ? int.Parse(args[1]) : -1) { }

        public NetworkArgs(bool shouldLaunch, bool shouldHost, string ip, int port)
        {
            ShouldAutoLaunch = shouldLaunch;
            ShouldHost = shouldHost;
            Ip = ip;
            Port = port;
        }
    }
}
