using System.Text;
using Camel.Core.Enums;

namespace Camel.Web;

public static class ModsHelper
{
    public static string GetModsString(Mods mods)
    {
        var result = new StringBuilder();

        foreach (Mods mod in Enum.GetValues(typeof(Mods)))
        {
            if (mods.HasFlag(mod))
            {
                result.Append(mod switch
                {
                    Mods.NoFail => "NF",
                    Mods.Easy => "EZ",
                    Mods.Hidden => "HD",
                    Mods.HardRock => "HR",
                    Mods.SuddenDeath => "SD",
                    Mods.DoubleTime => "DT",
                    Mods.Relax => "RX",
                    Mods.HalfTime => "HT",
                    Mods.Nightcore => "NC",
                    Mods.Flashlight => "FL",
                    Mods.SpunOut => "SO",
                    Mods.AutoPilot => "AP",
                    Mods.Perfect => "PF",
                    Mods.ScoreV2 => "SV2",
                    _ => ""
                });
            }
        }

        return result.ToString();
    } 
}