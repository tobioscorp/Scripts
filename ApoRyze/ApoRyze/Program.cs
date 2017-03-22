using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApoRyze
{

    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }


        private static void Loading_OnLoadingComplete(EventArgs e)
        {
            if (SpellOption.Spieler.ChampionName != "Ryze")
            {
                Chat.Print("[Ryze] Deaktiviert.");
                return;
            }

            SpellOption.SetupPrediction();            
            SpellOption.Spells();
            Menü.SetupMenu();

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += SpellDrawings.Drawing_OnDraw;
            AIHeroClient.OnLevelUp += UltRange;           
        }

        private static void UltRange(object sender, Obj_AI_BaseLevelUpEventArgs e)
        {
            if (SpellOption.Spieler.Level >= 10)
                SpellOption.R.Range = 3000;
        }

        private static void Game_OnTick(EventArgs e)
        {
            if (Menü.RyzeMenu["Activate"].Cast<CheckBox>().CurrentValue)
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    Combo.CastCombo();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                {
                    Combo.Flee();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                {
                    ClearModes.ClearMode();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                {
                    ClearModes.JungleClear();
                }
            }
        }
    }
}
