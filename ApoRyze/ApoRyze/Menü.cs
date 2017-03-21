using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApoRyze
{
    class Menü
    {
        public static List<Spell.SpellBase> SpellList = new List<Spell.SpellBase>();

        public static Menu RyzeMenu, ComboM, Drawings, AutoShield, Flee, Clear;

        public static void SetupMenu()
        {
            SpellList.Add(SpellOption.Q); SpellList.Add(SpellOption.W); SpellList.Add(SpellOption.E); SpellList.Add(SpellOption.R);

            RyzeMenu = MainMenu.AddMenu("Ryze", "Ryze");
            MainMenu.OnClose += Setup;

            // Combo Menu -------------------------------------------------------------------------------------------------------------------

            ComboM = RyzeMenu.AddSubMenu("Combo");  // Use in Combo
            ComboM.Add("Q", new CheckBox("Use Q"));
            ComboM.Add("W", new CheckBox("Use W"));
            ComboM.Add("E", new CheckBox("Use E"));
            ComboM.Add("R", new CheckBox("Use R"));
            ComboM.Add("Combo", new ComboBox("Combo Modus", 0, "EW", "EQ"));    // Combo Settings
            ComboM.AddSeparator();
            ComboM.Add("QPred", new Slider("QPrediction Hitchance", 80, 0, 100));   // Q Hit Chance


            // Drawings Menu ----------------------------------------------------------------------------------------------------------------
            Drawings = RyzeMenu.AddSubMenu("Drawings", "Drawings");
            Drawings.AddLabel("Draw Skill Range:");
            foreach (var Item in SpellList)
            {
                Drawings.Add(Item.Slot.ToString(), new CheckBox("Draw " + Item.Slot.ToString() + " Range"));
            }
            Drawings.AddSeparator();
            //Drawings.AddLabel("Mark Enemys:");
            //Drawings.Add("DrawKillable", new CheckBox("Mark Killable Enemys", true));
            //Drawings.Add("DrawKillable", new CheckBox("Draw Enemys Attack Range", false));

            // Main Menu -------------------------------------------------------------------------------------------------------------------
            RyzeMenu.AddLabel("Activate Script", 25);
            RyzeMenu.Add("Activate", new CheckBox("Activate Script", true));
            RyzeMenu.AddSeparator();
            RyzeMenu.AddLabel("Delay between Spells:");
            RyzeMenu.Add("DelayMin", new Slider("Minimum Delay", 20, 0, 1000));
            RyzeMenu.Add("DelayMax", new Slider("Maximum Delay", 120, 0, 1000));
            RyzeMenu.AddSeparator();
            RyzeMenu.AddLabel("Automatic Killsteal Mode", 25);
            RyzeMenu.Add("Steal", new CheckBox("Killsteal", false));
            RyzeMenu.AddLabel("Kill Steal Spells", 25);
            RyzeMenu.Add("StealQ", new CheckBox("Use Q For KS", true));
            RyzeMenu.Add("StealE", new CheckBox("Use E For KS", true));
            RyzeMenu.AddSeparator();
            RyzeMenu.AddLabel("Auto Stack Tear");
            RyzeMenu.Add("AutoStack", new KeyBind("AutoStackTear", false, KeyBind.BindTypes.PressToggle, 'Y'));

            // Clear Mode
            Clear = RyzeMenu.AddSubMenu("ClearMode", "ClearMode");
            Clear.AddLabel("Use Spells in ClearMode");
            Clear.Add("UseQ", new CheckBox("Use Q", true));
            Clear.Add("UseE", new CheckBox("Use E", true));
            Clear.Add("UseW", new CheckBox("Use W", false));
            Clear.AddSeparator();
            Clear.AddLabel("Clear Mode Minimum Mana");
            Clear.Add("Mana", new Slider("Minimum Mana", 20, 0, 100));
            // Flee Mode --------------------------------------------------------------------------------------------------------------------
            Flee = RyzeMenu.AddSubMenu("FleeMode", "FleeMode");
            Flee.AddLabel("The Flee Mode will Port you to the furthest Tower in Range.");
            Flee.AddLabel("If no Tower is in Range it will Port in Direction of your Base");
            Flee.AddSeparator();
            Flee.Add("Zhonyas", new CheckBox("Use Zhonyas"));
            Flee.Add("Seraphen", new CheckBox("Use Seraphen"));

            // Auto Shield ------------------------------------------------------------------------------------------------------------------
            AutoShield = RyzeMenu.AddSubMenu("AutoShield", "AutoShield");
            AutoShield.AddLabel("Automatic Shield if you get attacked and get low Life");
            AutoShield.Add("Active", new CheckBox("Activate AutoShield Mode", true));
            AutoShield.AddSeparator();
            AutoShield.AddLabel("Use Items:", 25);
            AutoShield.Add("Zhonyas", new CheckBox("Use Zhonyas", true));
            AutoShield.Add("Seraphen", new CheckBox("Use Seraphen", true));
            AutoShield.AddSeparator();
            AutoShield.Add("Prozent", new Slider("Use Shield at % HP", 20, 0, 100));
            
        }

        public static void Setup(object sender, EventArgs args)
        {
            SpellOption.SetupPrediction();
            SpellOption.SetupShield(AutoShield["Active"].Cast<CheckBox>().CurrentValue);
        }
    }
}
