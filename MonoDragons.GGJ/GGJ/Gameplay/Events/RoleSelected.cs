
namespace MonoDragons.GGJ.Gameplay
{
    public struct RoleSelected
    {
        public Player Role { get; set; }

        public RoleSelected(Player role)
        {
            Role = role;
        }
    }
}
