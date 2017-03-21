using EloBuddy.SDK.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using EloBuddy.SDK;
using System.Drawing;

namespace ApoRyze
{
    class SpellDrawings
    {
        public static void Drawing_OnDraw(EventArgs e)
        {
            if (Menü.RyzeMenu["Activate"].Cast<CheckBox>().CurrentValue)
            {
                if (SpellOption.Q.IsLearned && !SpellOption.Q.IsOnCooldown && Menü.Drawings["Q"].Cast<CheckBox>().CurrentValue)
                {
                    Circle.Draw(SharpDX.Color.Red, SpellOption.Q.Range, SpellOption.Spieler);
                }

                if (SpellOption.W.IsLearned && !SpellOption.W.IsOnCooldown && Menü.Drawings["W"].Cast<CheckBox>().CurrentValue)
                {
                    Circle.Draw(SharpDX.Color.Yellow, SpellOption.W.Range, SpellOption.Spieler);
                }

                if (SpellOption.E.IsLearned && !SpellOption.E.IsOnCooldown && Menü.Drawings["E"].Cast<CheckBox>().CurrentValue)
                {
                    Circle.Draw(SharpDX.Color.Blue, SpellOption.E.Range, SpellOption.Spieler);
                }


                if (SpellOption.R.IsLearned && !SpellOption.R.IsOnCooldown && Menü.Drawings["R"].Cast<CheckBox>().CurrentValue)
                {
                    Circle.Draw(SharpDX.Color.White, SpellOption.R.Range, SpellOption.Spieler);
                }

                HPBar();
            }
        }

        public static void HPBar()
        {
            IEnumerable<Obj_AI_Base> Heroes = EntityManager.Enemies.Where(x => x.IsHPBarRendered && x.IsValidTarget() && !x.IsMonster && !x.IsMinion);

            float Damage = 0;
            float RestLeben;

            foreach (var target in Heroes)
            {
                if (SpellOption.Q.IsLearned && SpellOption.Q.IsReady())
                    Damage += SpellOption.Q.GetSpellDamage(target) * 2;
                if (SpellOption.E.IsLearned && SpellOption.E.IsReady())
                    Damage += SpellOption.E.GetSpellDamage(target);
                if (SpellOption.W.IsLearned && SpellOption.W.IsReady())
                    Damage += SpellOption.W.GetSpellDamage(target);

                RestLeben = target.Health - Damage;

                if (RestLeben <= 0)
                {
                    Drawing.DrawText(target.HPBarPosition.X, target.HPBarPosition.Y + 25, System.Drawing.Color.Red, "Kill him!", 25);
                }
            }
        }
    }
}
   
