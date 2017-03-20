using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy.SDK.Menu.Values;

namespace ApoRyze
{
    class SpellOption
    {
        public static AIHeroClient Spieler = Player.Instance;

        public static Spell.Skillshot Q, R, Flash;
        public static Spell.Targeted W, E, Ignite;
        public static Item Hourglas, Seraphen, Tear, Archangels;
        public static Vector3 Spawnpunkt;
        public static int MinDelay, MaxDelay;
        public static double Mana;

        public static void Spells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1700, 60)
            { AllowedCollisionCount = 0 };
            W = new Spell.Targeted(SpellSlot.W, 615, DamageType.Magical);
            E = new Spell.Targeted(SpellSlot.E, 615, DamageType.Magical);
            R = new Spell.Skillshot(SpellSlot.R, 1750, SkillShotType.Circular, 2250, int.MaxValue, 475);

            Hourglas = new Item(ItemId.Zhonyas_Hourglass);
            Seraphen = new Item(ItemId.Seraphs_Embrace);
            Tear = new Item(ItemId.Tear_of_the_Goddess);
            Archangels = new Item(ItemId.Archangels_Staff);

            if (Player.Instance.GetSpellSlotFromName("summonerdot") != SpellSlot.Unknown)
            {
                Ignite = new Spell.Targeted(Player.Instance.GetSpellSlotFromName("summonerdot"), 600);
            }

            if (Player.Instance.GetSpellSlotFromName("summonerdot") != SpellSlot.Unknown)
            {
                Flash = new Spell.Skillshot(Player.Instance.GetSpellSlotFromName("flash"), 425, SkillShotType.Circular, 250, null, null); 
            }

            if (SpellOption.R.Level == 2)
                R.Range = 3000;

            if(Spieler.Team == GameObjectTeam.Order)
                Spawnpunkt = new Vector3(712, 724, 183);
            if (Spieler.Team == GameObjectTeam.Chaos)
                Spawnpunkt = new Vector2(13806, 13834).To3D();
        }

        public static HitChance Chance;

        public static void SetupPrediction()
        {
            Chance = HitChance.High;
            MinDelay = 20;
            MaxDelay = 120;
            Mana = 20;
            try
            {
                int Value = Menü.ComboM["QPred"].Cast<Slider>().CurrentValue;

                if (Value == 0)
                    Chance = HitChance.Collision;
                else if (Value < 20)
                    Chance = HitChance.Immobile;
                else if (Value >= 20 && Value <= 40)
                    Chance = HitChance.Low;
                else if (Value > 40 && Value <= 60)
                    Chance = HitChance.Medium;
                else if (Value > 60 && Value <= 80)
                    Chance = HitChance.High;
                else if (Value > 80 && Value < 100)
                    Chance = HitChance.Dashing;
                else if (Value == 100)
                    Chance = HitChance.Impossible;

                Mana = Menü.Clear["Mana"].Cast<Slider>().CurrentValue;
                MinDelay = Menü.RyzeMenu["MinDelay"].Cast<Slider>().CurrentValue;
                MaxDelay = Menü.RyzeMenu["MaxDelay"].Cast<Slider>().CurrentValue;
            }
            catch { }
        }

        public static Vector3 GetPunkt()
        {
            Random Randomize = new Random();

            Vector2 Spawn = Spawnpunkt.To2D();
            float X = Spawn.X;
            float Y = Spawn.Y;
            int Rechenart = Randomize.Next(0, 3);
            Vector3 Punkt = new Vector3(0,0,0);

            switch(Rechenart)
            {
                case 0:
                    Punkt = new Vector2(X + Randomize.Next(0, 1000), Y + Randomize.Next(0, 1000)).To3D();
                    return Punkt;
                break;

                case 1:
                    Punkt = new Vector2(X - Randomize.Next(0, 1000), Y + Randomize.Next(0, 1000)).To3D();
                    return Punkt;
                break;

                case 2:
                    Punkt = new Vector2(X + Randomize.Next(0, 1000), Y - Randomize.Next(0, 1000)).To3D();
                    return Punkt;
                break;

                case 3:
                    Punkt = new Vector2(X - Randomize.Next(0, 1000), Y - Randomize.Next(0, 1000)).To3D();
                    return Punkt;
                break;
            }

            return Punkt;
        }
    }
}
