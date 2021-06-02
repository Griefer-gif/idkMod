using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Idkmod.Projectiles.Hades
{
    public class VarathaSpinAttack : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 200;
            projectile.height = 200;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = 999;
            projectile.scale = 1f;
            projectile.timeLeft = 30;
            projectile.light = 1f;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            projectile.Center = Main.player[projectile.owner].Center;
            Main.player[projectile.owner].velocity = Vector2.Zero;
            //projectile.scale = projectile.ai[0];
        }

        public override bool PreAI()
        {
            projectile.width = 200 * (int)projectile.ai[0];
            projectile.height = 200 * (int)projectile.ai[0];
            return true;
        }
    }
}
