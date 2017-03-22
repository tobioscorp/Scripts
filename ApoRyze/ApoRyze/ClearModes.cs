using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApoRyze
{
    class ClearModes
    {
        public static void ClearMode()
        {
            Random Delay = new Random();
            Obj_AI_Minion Minion = null;
            IEnumerable<Obj_AI_Minion> M = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, SpellOption.Spieler.Position, SpellOption.E.Range);

            var Killable = EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m.IsValidTarget(SpellOption.Spieler.AttackRange) && m.Health <= SpellOption.E.GetSpellDamage(m)).OrderBy(m => m.Health).LastOrDefault();
            var Minions = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(m => m.IsValidTarget(SpellOption.Spieler.AttackRange)).OrderBy(a => a.Distance(SpellOption.Spieler)).FirstOrDefault(a => a.IsValidTarget(SpellOption.E.Range));
            int Buff = 0;
            var target = SpellOption.Q.GetTarget();

            if (target != null && target.IsValid && SpellOption.Q.GetPrediction(target).HitChance >= SpellOption.Chance)
            {
                Combo.CastCombo();
            }

            if (M.Count<Obj_AI_Base>() == 0) return;

            if (Killable != null) Minion = Killable;
            else if (Minions != null) Minion = Minions;
            else Minion = GetBestTarget(ref M);

            if (Minion == null) return;

            foreach (var Item1 in M)
            {
                if (Item1.HasBuff("RyzeE"))
                {
                    Buff++;
                }
            }

            foreach (var Item in M)
            {
                if (Orbwalker.ForcedTarget == Item && Item.HasBuff("RyzeE") && Item.Health >= 30) Orbwalker.DisableAttacking = true;

                if (CheckMinionNumber(Item) >= 2 && Item.IsValidTarget() && Item.HasBuff("RyzeE") && (Item.Health - SpellOption.E.GetSpellDamage(Item) >= 40 || Item.Health - SpellOption.E.GetSpellDamage(Item) <= 0)  && Item.Health >= 40 && SpellOption.E.IsReady() && CheckMinionNumber(Item) >= 2)
                {   
                    SpellOption.E.Cast(Item);
                }

                if ((Buff >= 3 || SpellOption.Q.GetSpellDamage(Item) >= Item.Health && Item.Health >= 40) && Item.HasBuff("RyzeE") && Item.IsValidTarget() && SpellOption.Q.GetPrediction(Item).HitChance >= SpellOption.Chance && SpellOption.Q.IsReady())
                {
                    Core.DelayAction(() => SpellOption.Q.Cast(Item), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                }
            }

            var QPred = SpellOption.Q.GetPrediction(Minion);

            if(Minion.IsValidTarget(SpellOption.E.Range) && SpellOption.E.IsReady() && SpellOption.Spieler.ManaPercent >= SpellOption.Mana && Minion.IsAlive() && Menü.Clear["UseE"].Cast<CheckBox>().CurrentValue && (Minion.Health - SpellOption.E.GetSpellDamage(Minion) >= 20 || Minion.Health - SpellOption.E.GetSpellDamage(Minion) <= 0))
            {
                Core.DelayAction(() => SpellOption.E.Cast(Minion), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
            }

            if (Minion.IsValidTarget(SpellOption.W.Range) && SpellOption.W.IsReady() && SpellOption.Q.IsOnCooldown && QPred.HitChance >= SpellOption.Chance && Minion.IsAlive() && (Minion.Health - SpellOption.W.GetSpellDamage(Minion) >= 40 || Minion.Health - SpellOption.W.GetSpellDamage(Minion) <= 0) && Menü.Clear["UseW"].Cast<CheckBox>().CurrentValue)
            {
                Core.DelayAction(() => SpellOption.W.Cast(Minion), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
            }


        }

        public static Obj_AI_Minion GetBestTarget(ref IEnumerable<Obj_AI_Minion> M)
        {
            List<int> Targets = new List<int>();
            int Index = 0;
            foreach (var Item in M)
            {
                int Number = 0;

                for (int i = 0; i < M.Count<Obj_AI_Minion>(); i++)
                {
                    if (Item.Distance(M.ElementAt(i).Position) <= 200)
                    {
                        Number++;
                    }
                }
                Targets.Add(Number);

                for (int i = 0; i < Targets.Count; i++)
                {
                    if (Targets[i] == Targets.Max())
                    {
                        Index = i;
                    }
                }
            }
            return M.ElementAt(Index);
        }

        public static int CheckMinionNumber(Obj_AI_Minion target)
        {
            IEnumerable<Obj_AI_Minion> M = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, SpellOption.Spieler.Position, SpellOption.E.Range);
            int Number = 0;

            for (int i = 0; i < M.Count<Obj_AI_Minion>(); i++)
            {
                if (target.Distance(M.ElementAt(i).Position) <= 200)
                {
                    Number++;
                }
            }

            return Number;
        }


        // -------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Jungle Clear Mode

        public static void JungleClear()
        {
            Random Delay = new Random();
            Obj_AI_Base Minion = null;
            IEnumerable<Obj_AI_Base> M = EntityManager.MinionsAndMonsters.GetJungleMonsters();
            var Flux = EntityManager.MinionsAndMonsters.Monsters.Where(m => m.IsValidTarget(SpellOption.Spieler.AttackRange + 200) && m.HasBuff("RyzeE")).OrderBy(x => x.Health).LastOrDefault();
            var MinionPredict = EntityManager.MinionsAndMonsters.Monsters.Where(m => m.IsValidTarget(SpellOption.Spieler.AttackRange + 200) && m.HasBuff("RyzeE") && SpellOption.Q.GetPrediction(m).HitChance >= HitChance.Medium).OrderBy(x => x.Health).LastOrDefault();
            var Minions = EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(m => m.IsValidTarget(SpellOption.Spieler.AttackRange + 200)).OrderBy(a => a.MaxHealth).LastOrDefault(a => a.IsValidTarget(SpellOption.E.Range));
            int Buff = 0;

            if (M.Count<Obj_AI_Base>() == 0) return;

            if (MinionPredict != null) Minion = MinionPredict;
            else if (Flux != null) Minion = Flux;
            else if (Minions != null) Minion = Minions;

            foreach (var Item in M)
            {
                if (Item.IsValidTarget() && (Item.HasBuff("RyzeE") || Item.Health <= SpellOption.E.GetSpellDamage(Item)) && SpellOption.E.IsReady())
                {
                    SpellOption.E.Cast(Item);
                    if (SpellOption.E.GetSpellDamage(Item) >= Item.Health) return;
                }
            }

            if (Minion == null) return;


            if (M.Count<Obj_AI_Base>() < 4)
            {
                var QPred = SpellOption.Q.GetPrediction(Minion);

                if (Minion.IsValidTarget(SpellOption.Q.Range) && SpellOption.Q.IsReady() && SpellOption.Spieler.Mana >= SpellOption.Q.ManaCost && QPred.HitChance >= SpellOption.Chance && Minion.IsAlive())
                {
                    QPred = SpellOption.Q.GetPrediction(Minion);
                    Core.DelayAction(() => SpellOption.Q.Cast(Minion), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                    if (SpellOption.Q.GetSpellDamage(Minion) >= Minion.Health) return;
                }

                if (Minion.IsValidTarget(SpellOption.W.Range) && SpellOption.W.IsReady() && SpellOption.Spieler.Mana >= SpellOption.W.ManaCost && Minion.IsAlive() && !Minion.HasBuff("RyzeE") && !SpellOption.E.IsOnCooldown && SpellOption.Q.IsOnCooldown)
                {
                    Core.DelayAction(() => SpellOption.W.Cast(Minion), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                    if (SpellOption.W.GetSpellDamage(Minion) >= Minion.Health) return;
                }
                    
                if (Minion.IsValidTarget(SpellOption.E.Range) && SpellOption.E.IsReady() && SpellOption.Spieler.Mana >= SpellOption.E.ManaCost && Minion.IsAlive() && SpellOption.Q.IsOnCooldown)
                {
                    Core.DelayAction(() => SpellOption.E.Cast(Minion), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                    if (SpellOption.E.GetSpellDamage(Minion) >= Minion.Health) return;
                }
            }
            else
            {
                if (Minion.IsValidTarget(SpellOption.E.Range) && SpellOption.E.IsReady() && SpellOption.Spieler.Mana >= SpellOption.E.ManaCost && Minion.IsAlive())
                {
                    Core.DelayAction(() => SpellOption.E.Cast(Minion), Delay.Next(SpellOption.MinDelay, SpellOption.MaxDelay));
                    if (SpellOption.E.GetSpellDamage(Minion) >= Minion.Health) return;
                }

                foreach (var Item in M)
                {
                    if(Item.HasBuff("RyzeE"))
                    {
                        Buff++;
                    }
                }

                if(Buff >= 3 && Minion.HasBuff("RyzeE") && SpellOption.Q.GetPrediction(Minion).HitChance >= SpellOption.Chance)
                {
                    SpellOption.Q.Cast(Minion);
                }
            }
            
        }
    }
}
