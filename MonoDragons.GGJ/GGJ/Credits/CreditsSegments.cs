using Microsoft.Xna.Framework;

namespace MonoDragons.GGJ.Credits
{
    public static class CreditColors
    {
        public static readonly Color Backend = Color.FromNonPremultiplied(120, 80, 255, 255);
        public static readonly Color Project = Color.FromNonPremultiplied(50, 255, 50, 255);
        public static readonly Color Frontend = Color.FromNonPremultiplied(255, 80, 80, 255);
        public static readonly Color Gameplay = Color.FromNonPremultiplied(50, 160, 255, 255);
    }
    
    public static class CreditNames
    {
        public const string Noah = "Noah Reinagel";
        public const string Silas = "Silas Reinagel";
        public const string Abe = "Abraham Reinagel";
        public const string hac = "hac.creative";
        public const string Tim = "Tim Reinagel";
        public const string Gordy = "Gordy Keene";
    }
    
    public sealed class ProjectManagerCredit : BasicJamCreditSegment
    {
        public override Color RoleColor => CreditColors.Project;
        public override string Role => "Project Manager";
        public override string Name => CreditNames.Silas;
    }

    public sealed class LeadGameDesignerCredit : BasicJamCreditSegment
    {
        public override Color RoleColor => CreditColors.Gameplay;
        public override string Role => "Lead Game Designer";
        public override string Name => CreditNames.Noah;
    }

    public sealed class UiDesignerCredit : BasicJamCreditSegment
    {
        public override Color RoleColor => CreditColors.Frontend;
        public override string Role => "UI Designer";
        public override string Name => CreditNames.Silas;
    }
    
    public sealed class ComposerCredit : BasicJamCreditSegment
    {
        public override Color RoleColor => CreditColors.Frontend;
        public override string Role => "Original Music";
        public override string Name => CreditNames.hac;
    }
    
    public sealed class GameplayDesigner1Credit : BasicJamCreditSegment
    {
        public override Color RoleColor => CreditColors.Gameplay;
        public override string Role => "Gameplay Designer";
        public override string Name => CreditNames.Noah;
    }

    public sealed class GameplayDesigner2Credit : BasicJamCreditSegment
    {
        public override Color RoleColor => CreditColors.Gameplay;
        public override string Role => "Gameplay Designer";
        public override string Name => CreditNames.Abe;
    }
    
    public sealed class LeadProgrammerCredit : BasicJamCreditSegment
    {
        public override Color RoleColor => CreditColors.Backend;
        public override string Role => "Lead Programmer";
        public override string Name => CreditNames.Tim;
    }

    public sealed class GameplayProgrammerCredit : BasicJamCreditSegment
    {
        public override Color RoleColor => CreditColors.Backend;
        public override string Role => "Gameplay Programmer";
        public override string Name => CreditNames.Noah;
    }
    
    public sealed class NetCodeProgrammerCredit : BasicJamCreditSegment
    {
        public override Color RoleColor => CreditColors.Backend;
        public override string Role => "Network Programmer";
        public override string Name => CreditNames.Silas;
    }

    public sealed class Tester1Credit : BasicJamCreditSegment
    {
        public override Color RoleColor => CreditColors.Project;
        public override string Role => "Game Tester";
        public override string Name => CreditNames.Gordy;
    }
}