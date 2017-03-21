using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ApoRyze
{
    class Combo
    {
        // Combo Mode
        public static void CastCombo()
        {
            var target = TargetSelector.GetTarget(SpellOption.E.Range, DamageType.Magical);
            Random Delay = new Random();

            if (target == null) return;

            var QPred = SpellOption.Q.GetPrediction(target);

            if (target.IsValidTarget(SpellOption.Q.Range) && SpellOption.Q.IsReady() && QPred.HitChance >= SpellOption.Chance && SpellOption.Spieler.Mana >= SpellOption.Q.ManaCost && Menü.ComboM["Q"].Cast<CheckBox>().CurrentValue == true)
            {
                QPred = SpellOption.Q.GetPrediction(target);
                SpellOption.Q.Cast(target);
            }

            // Combo Option EW
            if (Menü.ComboM["Combo"].Cast<ComboBox>().CurrentValue == 0)
            {
                if (target.IsValidTarget(SpellOption.E.Range) && SpellOption.E.IsReady() && SpellOption.Spieler.Mana >= SpellOption.E.ManaCost && Menü.ComboM["E"].Cast<CheckBox>().CurrentValue == true && SpellOption.Q.IsOnCooldown)
                {
                    Core.DelayAction(() => SpellOption.E.Cast(target), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                }

                if (target.IsValidTarget(SpellOption.W.Range) && SpellOption.W.IsReady() && SpellOption.Spieler.Mana >= SpellOption.W.ManaCost && Menü.ComboM["W"].Cast<CheckBox>().CurrentValue == true && SpellOption.Q.IsOnCooldown)
                {
                    Core.DelayAction(() => SpellOption.W.Cast(target), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                }

                if (target.IsValidTarget(SpellOption.Q.Range) && SpellOption.Q.IsReady() && QPred.HitChance >= SpellOption.Chance && SpellOption.Spieler.Mana >= SpellOption.Q.ManaCost && Menü.ComboM["Q"].Cast<CheckBox>().CurrentValue == true && target.HasBuff("RyzeE"))
                {
                    QPred = SpellOption.Q.GetPrediction(target);
                    SpellOption.Q.Cast(target);
                }
            }

            // Combo Option EQ
            if (Menü.ComboM["Combo"].Cast<ComboBox>().CurrentValue == 1)
            {
                if (target.IsValidTarget(SpellOption.E.Range) && SpellOption.E.IsReady() && SpellOption.Spieler.Mana >= SpellOption.E.ManaCost && Menü.ComboM["E"].Cast<CheckBox>().CurrentValue == true && SpellOption.Q.IsOnCooldown)
                {
                    Core.DelayAction(() => SpellOption.E.Cast(target), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                }

                if (target.IsValidTarget(SpellOption.Q.Range) && SpellOption.Q.IsReady() && QPred.HitChance >= SpellOption.Chance && SpellOption.Spieler.Mana >= SpellOption.Q.ManaCost && Menü.ComboM["Q"].Cast<CheckBox>().CurrentValue == true && target.HasBuff("RyzeE"))
                {
                    QPred = SpellOption.Q.GetPrediction(target);
                    SpellOption.Q.Cast(target);
                }

                if (target.IsValidTarget(SpellOption.W.Range) && SpellOption.W.IsReady() && SpellOption.Spieler.Mana >= SpellOption.W.ManaCost && Menü.ComboM["W"].Cast<CheckBox>().CurrentValue == true && SpellOption.Q.IsOnCooldown)
                {
                    Core.DelayAction(() => SpellOption.W.Cast(target), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                }
            }
        }
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Automatic Killsteal Mode

        public static void KillSteal()
        {
            var target = TargetSelector.GetTarget(SpellOption.E.Range, DamageType.Magical);
            Random Delay = new Random();

            if (target == null)
            { return; }

            var QPred = SpellOption.Q.GetPrediction(target);
            float DamageE = SpellOption.E.GetSpellDamage(target);
            float DamageQ = SpellOption.Q.GetSpellDamage(target) * 2;

            if (target.Health <= DamageE + DamageQ)
            {
                if (target.IsValidTarget(SpellOption.Q.Range) && SpellOption.Q.IsReady() && QPred.HitChance >= SpellOption.Chance && SpellOption.Spieler.Mana >= SpellOption.Q.ManaCost && Menü.RyzeMenu["StealQ"].Cast<CheckBox>().CurrentValue)
                {
                    Core.DelayAction(() => SpellOption.Q.Cast(target), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                }

                if (target.IsValidTarget(SpellOption.E.Range) && SpellOption.E.IsReady() && SpellOption.Spieler.Mana >= SpellOption.E.ManaCost && Menü.RyzeMenu["StealE"].Cast<CheckBox>().CurrentValue)
                {
                    Core.DelayAction(() => SpellOption.E.Cast(target), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                }
            }
        }
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Flee Mode

        public static void Flee()
        {
            Random Delay = new Random();
            bool Port;
            IEnumerable<Obj_AI_Turret> Türme = EntityManager.Turrets.Allies.OrderBy(x => x.Distance(SpellOption.Spieler.Position));
            Obj_AI_Turret EntfernterTurm = null;
            
            foreach (var Item in Türme)
            {
                if (EntfernterTurm == null)
                    EntfernterTurm = Item;

                if (Item.Distance(SpellOption.Spieler.Position) <= SpellOption.R.Range && Item.Distance(SpellOption.Spieler.Position) > EntfernterTurm.Distance(SpellOption.Spieler.Position) && Item.IsAlive()
                    && Item.Distance(SpellOption.Spawnpunkt) <= EntfernterTurm.Distance(SpellOption.Spawnpunkt))
                {
                    EntfernterTurm = Item;
                }
            }

            SpellOption.R.IsInRange(EntfernterTurm);

            if (EntfernterTurm.Distance(SpellOption.Spieler.Position) > SpellOption.R.Range || SpellOption.Spieler.Distance(EntfernterTurm) <= EntfernterTurm.AttackRange)
            { Port = false; }
            else { Port = true; }

            // Port Tower
            if (Port == true)
            {
                if (SpellOption.R.IsReady())
                    SpellOption.R.Cast(EntfernterTurm.Position);

                if (SpellOption.Hourglas.IsOwned() && SpellOption.Hourglas.IsReady() && Menü.Flee["Zhonyas"].Cast<CheckBox>().CurrentValue && SpellOption.R.IsOnCooldown)
                {
                    Core.DelayAction(() => SpellOption.Hourglas.Cast(), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                }
                else if (SpellOption.Seraphen.IsOwned() && SpellOption.Seraphen.IsReady() && Menü.Flee["Seraphen"].Cast<CheckBox>().CurrentValue)
                {
                    Core.DelayAction(() => SpellOption.Seraphen.Cast(), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                }
            }

            // Get Shield
            if (SpellOption.R.IsOnCooldown)
            {
                var target = TargetSelector.GetTarget(SpellOption.E.Range, DamageType.Magical);

                if (target != null)
                {
                    var QPred = SpellOption.Q.GetPrediction(target);

                    if (SpellOption.Seraphen.IsOwned() && SpellOption.Seraphen.IsReady() && Menü.Flee["Seraphen"].Cast<CheckBox>().CurrentValue)
                    {
                        Core.DelayAction(() => SpellOption.Seraphen.Cast(), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                    }


                    if (target.IsValidTarget(SpellOption.E.Range) && SpellOption.E.IsReady() && SpellOption.Spieler.Mana >= SpellOption.E.ManaCost)
                    {
                        Core.DelayAction(() => SpellOption.E.Cast(target), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                    }

                    if (target.IsValidTarget(SpellOption.Q.Range) && SpellOption.Q.IsReady() && QPred.HitChance >= SpellOption.Chance && SpellOption.Spieler.Mana >= SpellOption.Q.ManaCost && SpellOption.Spieler.HasBuff("RyzeQIconFullCharge"))
                    {
                        Core.DelayAction(() => SpellOption.Q.Cast(target), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                    }
                }
            }

            // Port Position in Direction of the Base
            if (Port == false)
            {
                Vector3 Punkt = GetPosition();

                if (SpellOption.R.IsReady())
                    SpellOption.R.Cast(Punkt);

                if (SpellOption.Hourglas.IsOwned() && SpellOption.Hourglas.IsReady() && Menü.Flee["Zhonyas"].Cast<CheckBox>().CurrentValue && SpellOption.R.IsOnCooldown)
                    Core.DelayAction(() => SpellOption.Hourglas.Cast(), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                else if (SpellOption.Seraphen.IsOwned() && SpellOption.Seraphen.IsReady() && Menü.Flee["Seraphen"].Cast<CheckBox>().CurrentValue)
                    Core.DelayAction(() => SpellOption.Seraphen.Cast(), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
            }
        }
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Lane Clear Modus

        public static void ClearMode()
        {
            Random Delay = new Random();
            Obj_AI_Minion Minion = null;
            IEnumerable<Obj_AI_Minion> M = EntityManager.MinionsAndMonsters.GetLaneMinions();

            var Flux = EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m.IsValidTarget(SpellOption.Spieler.AttackRange + 200) && m.HasBuff("RyzeE")).OrderBy(x => x.Distance(Player.Instance.Position)).FirstOrDefault();
            var Killable = EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m.IsValidTarget(SpellOption.Spieler.AttackRange + 200) && m.Health <= SpellOption.E.GetSpellDamage(m)).OrderBy(m => m.Health).LastOrDefault();
            var Minions = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(m => m.IsValidTarget(SpellOption.Spieler.AttackRange + 200)).OrderBy(a => a.Distance(SpellOption.Spieler)).FirstOrDefault(a => a.IsValidTarget(SpellOption.E.Range));

            var target = SpellOption.Q.GetTarget();

            if (target != null && target.IsValid)
            {
                CastCombo();
            }

            if (Killable != null) Minion = Killable;
            else if (Flux != null) Minion = Flux;
            else if (Minions != null) Minion = Minions;

            if (Minion == null || Minion.Health <= SpellOption.Spieler.GetAutoAttackDamage(Minion)) return;

            foreach (var Item in M)
            {
                if (Item.HasBuff("RyzeE") && Item.Health <= SpellOption.Q.GetSpellDamage(Item) * 2 && Item.IsValidTarget() && SpellOption.Q.GetPrediction(Item).HitChance >= SpellOption.Chance
                    && SpellOption.Q.IsReady() && SpellOption.Mana <= SpellOption.Spieler.Mana && Minion.Health >= 40)
                    Core.DelayAction(() => SpellOption.Q.Cast(Item), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
            }

            //if (Minion.Health <= SpellOption.Spieler.GetAutoAttackDamage(Minion) / 2 && Minion.HasBuff("RyzeE") && SpellOption.Q.IsReady() && SpellOption.E.IsReady()) Orbwalker.DisableAttacking = true;
            if (Minion.Health <= SpellOption.Spieler.GetAutoAttackDamage(Minion)) return;

            var QPred = SpellOption.Q.GetPrediction(Minion);

            if (Minion.IsValidTarget(SpellOption.E.Range) && SpellOption.E.IsReady() && SpellOption.Spieler.ManaPercent >= SpellOption.Mana && Minion.IsAlive() && (Minion.Health - SpellOption.E.GetSpellDamage(Minion) >= 40 || Minion.Health - SpellOption.E.GetSpellDamage(Minion) <= 0) && Menü.Clear["UseE"].Cast<CheckBox>().CurrentValue)
            {
                Core.DelayAction(() => SpellOption.E.Cast(Minion), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
            }

            if (Minion.IsValidTarget(SpellOption.Q.Range) && SpellOption.Q.IsReady() && SpellOption.Spieler.ManaPercent >= SpellOption.Mana && Minion.HasBuff("RyzeE") && QPred.HitChance >= SpellOption.Chance && Minion.IsAlive() && (Minion.Health - SpellOption.Q.GetSpellDamage(Minion) >= 40 || Minion.Health - SpellOption.Q.GetSpellDamage(Minion) <= 0) && Menü.Clear["UseQ"].Cast<CheckBox>().CurrentValue)
            {
                Core.DelayAction(() => SpellOption.Q.Cast(Minion), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
            }

            if (Minion.IsValidTarget(SpellOption.W.Range) && SpellOption.W.IsReady() && SpellOption.Spieler.ManaPercent >= SpellOption.Mana && QPred.HitChance >= SpellOption.Chance && Minion.IsAlive() && (Minion.Health - SpellOption.W.GetSpellDamage(Minion) >= 40 || Minion.Health - SpellOption.W.GetSpellDamage(Minion) <= 0) && Menü.Clear["UseW"].Cast<CheckBox>().CurrentValue)
            {
                Core.DelayAction(() => SpellOption.W.Cast(Minion), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
            }
        }
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Jungle Clear Mode

        public static void JungleClear()
        {
            Random Delay = new Random();
            Obj_AI_Base Minion = null;
            IEnumerable<Obj_AI_Base> M = EntityManager.MinionsAndMonsters.GetJungleMonsters();

            var Flux = EntityManager.MinionsAndMonsters.Monsters.Where(m => m.IsValidTarget(SpellOption.Spieler.AttackRange + 200) && m.HasBuff("RyzeE")).OrderBy(x => x.Distance(Player.Instance.Position)).FirstOrDefault();
            var Killable = EntityManager.MinionsAndMonsters.Monsters.Where(m => m.IsValidTarget(SpellOption.Spieler.AttackRange + 200) && m.Health <= SpellOption.E.GetSpellDamage(m)).OrderBy(m => m.Health).LastOrDefault();
            var Minions = EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(m => m.IsValidTarget(SpellOption.Spieler.AttackRange + 200)).OrderBy(a => a.MaxHealth).FirstOrDefault(a => a.IsValidTarget(SpellOption.E.Range));

            if (Killable != null) Minion = Killable;
            else if (Flux != null) Minion = Flux;
            else if (Minions != null) Minion = Minions;

            foreach (var Item in M)
            {
                if (Item.HasBuff("RyzeE") && Item.Health >= SpellOption.Q.GetSpellDamage(Item) * 2 && Item.IsValidTarget() && SpellOption.Q.GetPrediction(Item).HitChance >= SpellOption.Chance
                    && SpellOption.Q.IsReady() && SpellOption.Q.ManaCost <= SpellOption.Spieler.Mana && Minion.Health >= 40)
                    Core.DelayAction(() => SpellOption.Q.Cast(Item), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
            }

            if (Minion == null) return;

            if (Minion.Health <= SpellOption.Spieler.GetAutoAttackDamage(Minion) / 2 && Minion.HasBuff("RyzeE") && SpellOption.Q.IsReady() && SpellOption.E.IsReady()) Orbwalker.DisableAttacking = true;
            else if (Minion.Health <= SpellOption.Spieler.GetAutoAttackDamage(Minion)) return;

            var QPred = SpellOption.Q.GetPrediction(Minion);

            if (Minion.IsValidTarget(SpellOption.E.Range) && SpellOption.E.IsReady() && SpellOption.Spieler.Mana >= SpellOption.E.ManaCost && Minion.IsAlive())
            {
                Core.DelayAction(() => SpellOption.E.Cast(Minion), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
            }

            if (Minion.IsValidTarget(SpellOption.Q.Range) && SpellOption.Q.IsReady() && SpellOption.Spieler.Mana >= SpellOption.Q.ManaCost && QPred.HitChance >= SpellOption.Chance && Minion.IsAlive())
            {
                Core.DelayAction(() => SpellOption.Q.Cast(Minion), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
            }

            if (Minion.IsValidTarget(SpellOption.W.Range) && SpellOption.W.IsReady() && SpellOption.Spieler.Mana >= SpellOption.W.ManaCost && Minion.IsAlive())
            {
                Core.DelayAction(() => SpellOption.W.Cast(Minion), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
            }
        }
        // ------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Ult Position Bestimmen
        private static Vector3 GetPosition()
        {
            Vector3 newPosition;
            newPosition = Player.Instance.Position.Extend(SpellOption.GetPunkt(), SpellOption.R.Range).To3DWorld();

            return newPosition;
        }
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Tear Auto Stack Mode

        public static int Ticks = 0;

        public static void AutoStack()
        {
            Random Delay = new Random();
            int TPS = Game.TicksPerSecond;

            Ticks++;

            if (Ticks >= TPS * 4)
            {
                Ticks = 0;
                Obj_AI_Base target = TargetSelector.GetTarget(SpellOption.Q.Range + 200, DamageType.Magical);
                var Minions = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(m => m.IsValidTarget(SpellOption.Q.Range) && SpellOption.Q.GetPrediction(m).HitChance >= SpellOption.Chance).OrderBy(a => a.MaxHealth).FirstOrDefault(a => a.IsValidTarget(SpellOption.Q.Range));

                if (SpellOption.Q.IsReady() && target != null && target.IsValid && (SpellOption.Tear.IsOwned() || SpellOption.Archangels.IsOwned()))
                {
                    if (SpellOption.Q.GetPrediction(target).HitChance >= SpellOption.Chance && !SpellOption.Spieler.IsRecalling())
                    {
                        Core.DelayAction(() => SpellOption.Q.Cast(target), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                        return;
                    }
                }

                if (SpellOption.Q.IsReady() && Minions != null && Minions.IsValid && (SpellOption.Tear.IsOwned() || SpellOption.Archangels.IsOwned()) && !SpellOption.Spieler.IsRecalling())
                {

                    Core.DelayAction(() => SpellOption.Q.Cast(Minions), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                    return;
                }

                if (SpellOption.Q.IsReady() && target == null && (SpellOption.Tear.IsOwned() || SpellOption.Archangels.IsOwned()) && !SpellOption.Spieler.IsRecalling())
                {
                    Core.DelayAction(() => SpellOption.Q.Cast(Game.CursorPos), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                    return;
                }

                if (target == null) return;

                var QPred = SpellOption.Q.GetPrediction(target);
            }
        }

        // -------------------------------------------------------------------------------------------------------------------------------------------------------------
        // AutoShield Mode
        public static void AutoShield(Obj_AI_Base Sender, GameObjectProcessSpellCastEventArgs e)
        {
            var Attacker = Sender as AIHeroClient;
            var target = e.Target as AIHeroClient;

            if (target == null || Sender == null) return;

            if(target.IsMe)
            {
                if (SpellOption.Spieler.HealthPercent <= Menü.AutoShield["Prozent"].Cast<Slider>().CurrentValue && SpellOption.Hourglas.IsOwned() && Menü.AutoShield["Zhonyas"].Cast<CheckBox>().CurrentValue && SpellOption.Hourglas.IsReady() && SpellOption.Spieler.IsInRange(target, SpellOption.Q.Range + 200))
                {
                    SpellOption.Hourglas.Cast();
                }

                if (SpellOption.Spieler.HealthPercent <= Menü.AutoShield["Prozent"].Cast<Slider>().CurrentValue && SpellOption.Seraphen.IsOwned() && Menü.AutoShield["Seraphen"].Cast<CheckBox>().CurrentValue && SpellOption.Seraphen.IsReady() && SpellOption.Spieler.IsInRange(target, SpellOption.Q.Range + 200))
                {
                    SpellOption.Seraphen.Cast();
                }
            }
        }
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
