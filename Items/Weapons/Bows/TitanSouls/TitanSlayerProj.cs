using Idkmod.Buffs.TitanSouls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Items.Weapons.Bows.TitanSouls
{
    //All of this code took me way too long to do and its kinda of a mess, Pog.

    //----------------------------------------------------------------------------------------------------------------------
    public class TitanSlayerHoldout : ModProjectile
    {
        public override string Texture => "Idkmod/Items/Weapons/Bows/TitanSouls/TitanSlayer";

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.width = 50;
            projectile.height = 50;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.alpha = 255;
            projectile.penetrate = 1;
            projectile.tileCollide = false;
            projectile.timeLeft = 999999;
        }

        private int counter = 0;

        public override bool PreAI()
        {
            int arrow = ModContent.ProjectileType<TitanSlayerArrow>();
            Player player = Main.player[projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);

            UpdatePlayerVisuals(player, rrp);

            if (player.channel)
            {
                int counterMaxValue = 60;
                projectile.Center = player.Center;
                if (counter < counterMaxValue + 1)//+1 so that the next 'if' doesnt get stuck
                {
                    counter++;
                }

                if (counter == counterMaxValue)
                {

                    //Main.NewText("stacks");
                    Main.PlaySound(SoundID.MaxMana, (int)projectile.position.X, (int)projectile.position.Y);
                    for (int i = 0; i < 100; i++)
                    {
                        DoDustEffect(player.Center, 40f);
                    }
                }
            }
            else
            {
                if (projectile.owner == Main.myPlayer && counter >= 20)
                {
                    Vector2 velocity = Vector2.Normalize(Main.MouseWorld - player.Center);

                    Main.PlaySound(SoundID.Item5, player.position);
                    Projectile.NewProjectile(player.Center, (velocity * 30f), arrow, projectile.damage * 2, projectile.knockBack, projectile.owner, counter);
                }
                projectile.active = false;
            }
            
            return true;
        }
        private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
        {
            float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
            Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

            int dust = Dust.NewDust(position - vec * distance, 0, 0, DustID.BoneTorch);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale *= .5f;
            Main.dust[dust].velocity = vel;
            Main.dust[dust].customData = follow;
        }

        private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            projectile.Center = playerHandPos;

            projectile.rotation = projectile.velocity.ToRotation();
            projectile.spriteDirection = projectile.direction;

            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            // If you do not multiply by projectile.direction, the player's hand will point the wrong direction while facing left.
            player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();

            projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center);
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------
    public class TitanSlayerArrow : ModProjectile
    {
        
        private int timer = 0;
        Vector2 direction = Vector2.Zero;
        private bool stickingToTarget = false;
        private int stickTargetIndex;
        private int stickyTimer = 0;

        //i should definetly make an enum for this ai, but meh
        //again, fuck magic numbers, holy shit
        const int comingBack = 1; //coming back
        const int arrowOut = 0; //travelling/standing still
        const int arrowStuck = 2; //stuck to target
        const int pullingBackSticky = 3;//pulling back arrow from sticky

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TitanSlayerArrow");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.timeLeft = 9999;
            projectile.light = 0.5f;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.penetrate = 9999;
            projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            //projectile.ai[0] is the timer passed by the holdout projectile, so dont change it
            //projectile.ai[1] is the state of the projectile
            //1 = coming back
            //0 = travelling/standing still
            //2 = stuck to target
            //3 = pulling back arrow from sticky
            const int maxTicks = 300;
            Player player = Main.player[projectile.owner];
            if (stickingToTarget)
            {
                if (projectile.ai[1] == pullingBackSticky)
                {
                    stickyTimer++;

                    float rotationValue = 0.05f;
                    projectile.rotation += Main.rand.NextFloat(-rotationValue, rotationValue);

                    if (stickyTimer >= maxTicks)
                    {
                        projectile.ai[1] = comingBack;
                        stickingToTarget = false;
                        projectile.damage = player.HeldItem.damage * 2;

                        int numSouls = 3;
                        for (int i = 0; i < numSouls; i++)
                        {
                            Projectile.NewProjectile(projectile.Center, projectile.velocity.RotatedByRandom(Math.PI), ModContent.ProjectileType<SoulProj>(), 0, 0, projectile.owner);
                        }

                        Vector2 dustVel = projectile.velocity.RotatedBy(MathHelper.ToRadians(180));
                        for(int i = 0; i < 50; i++)
                        {
                            Dust.NewDust(projectile.Center, 10, 10, DustID.Blood, dustVel.X, dustVel.Y, Scale: 2f);
                        }
                        
                        Main.npc[stickTargetIndex].StrikeNPC(projectile.damage * 2, projectile.knockBack * 2, projectile.direction);

                        Projectile.NewProjectile(projectile.position, Vector2.Zero, ModContent.ProjectileType<ArrowExplosion>(), projectile.damage * 2, projectile.knockBack, projectile.owner);
                    }
                }
                else stickyTimer = 0;

                StickyAI();
            }
            else
            {
                if (projectile.ai[1] == pullingBackSticky)
                {
                    projectile.ai[1] = arrowOut;
                }
                float inertia = 20f;
                float speed = 20f;

                if (projectile.ai[1] == arrowOut)
                {
                    NormalAI(player);
                }
                else if (projectile.ai[1] == comingBack)
                {
                    ComeBackToPlayer(direction, player, speed, inertia);
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            Main.PlaySound(SoundID.Item10, projectile.position);
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.rand.Next(0, 5) == 0 || stickingToTarget && projectile.ai[1] != arrowOut)
            {
                Main.PlaySound(SoundID.MaxMana, Main.player[projectile.owner].Center);
                projectile.ai[1] = arrowStuck;
                stickingToTarget = true; // we are sticking to a target
                stickTargetIndex = target.whoAmI; // Set the target whoAmI
                projectile.velocity =
                    (target.Center - projectile.Center) *
                    0.75f;
                projectile.netUpdate = true;
                projectile.damage = 0;
            }
        }

        private void ComeBackToPlayer(Vector2 direction, Player player, float speed, float inertia)
        {
            direction = player.Center - projectile.Center;
            direction.Normalize();
            direction *= speed;
            projectile.tileCollide = false;
            projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            projectile.timeLeft = 9999;
            if (projectile.Distance(player.Center) <= 16f)
            {
                Main.PlaySound(SoundID.Grab, player.position);
                projectile.Kill();
            }
        }

        private void NormalAI(Player player)
        {
            float maxTimecounter = projectile.ai[0] * 2;
            timer++;
            if (timer < maxTimecounter)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            else
            {
                if (projectile.Distance(player.Center) <= 16f)
                {
                    Main.PlaySound(SoundID.Grab, player.position);
                    projectile.Kill();
                }
                projectile.velocity = Vector2.Zero;
                if (projectile.rotation != MathHelper.Pi)
                {
                    if (projectile.rotation >= MathHelper.Pi - 0.2 && projectile.rotation <= MathHelper.Pi + 0.2)
                    {
                        projectile.rotation = MathHelper.Pi;
                    }
                    else
                    {
                        if (projectile.rotation >= MathHelper.Pi)
                        {
                            projectile.rotation -= 0.3f;
                        }
                        else
                        {
                            projectile.rotation += 0.3f;
                        }
                    }
                }
                else
                {
                    const double amp = 20;
                    const double freq = 0.07;
                    projectile.position.Y += (float)((Math.Cos(freq * projectile.timeLeft) / 2) * amp * freq);
                }
            }

            if (projectile.ai[0] > 0)
            {
                projectile.ai[0] -= 2;
            }
        }

        private void StickyAI()
        {
            projectile.ignoreWater = true; // Make sure the projectile ignores water
            projectile.tileCollide = false; // Make sure the projectile doesn't collide with tiles anymore
            const int aiFactor = 15; // Change this factor to change the 'lifetime' of this sticking javelin
            projectile.localAI[0] += 1f;

            // Every 30 ticks, the javelin will perform a hit effect
            bool hitEffect = projectile.localAI[0] % 30f == 0f;
            int projTargetIndex = (int)stickTargetIndex;
            if (projectile.localAI[0] >= 60 * aiFactor || projTargetIndex < 0 || projTargetIndex >= 200)
            { // If the index is past its limits, kill it
                stickingToTarget = false;
                projectile.velocity = Vector2.Zero;
            }
            else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage)
            { // If the target is active and can take damage
              // Set the projectile's position relative to the target's center
                projectile.Center = Main.npc[projTargetIndex].Center - projectile.velocity * 2f;
                projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
                if (hitEffect)
                { // Perform a hit effect here
                    Main.npc[projTargetIndex].HitEffect(0, 1.0);
                }
            }
            else
            {
                stickingToTarget = false;
                projectile.velocity = Vector2.Zero;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }

            return true;
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------
    public class ArrowMagnet : ModProjectile
    {
        //these ints are the best thing i have done, fuck magic numbers
        const int comingBack = 1; //coming back
        const int arrowOut = 0; //travelling/standing still
        const int arrowStuck = 2; //stuck to target
        const int pullingBackSticky = 3;//pulling back arrow from sticky
        public override string Texture => "Idkmod/Items/Weapons/Bows/TitanSouls/TitanSlayer";
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.timeLeft = 9999;
            projectile.light = 0.5f;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = 9999;
            projectile.extraUpdates = 0;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            bool arrowDead = false;

            for (int i = 0; Main.projectile.Length > i; i++)
            {
                if (Main.projectile[i].owner == player.whoAmI && Main.projectile[i].type == ModContent.ProjectileType<TitanSlayerArrow>() && Main.projectile[i].active)
                {
                    if (!Main.projectile[i].active)
                    {
                        arrowDead = true;
                    }
                    projectile.velocity = Vector2.Normalize(Main.projectile[i].Center - player.Center);

                    if (player.channel && Main.projectile[i].ai[1] == arrowStuck)
                    {
                        Main.projectile[i].ai[1] = pullingBackSticky;


                        projectile.rotation = projectile.velocity.ToRotation();
                        player.ChangeDir(projectile.direction);
                    }
                    else if (!player.channel && Main.projectile[i].ai[1] == pullingBackSticky)
                    {
                        Main.projectile[i].ai[1] = arrowStuck;
                    }

                    if (player.channel && Main.projectile[i].ai[1] == arrowOut)
                    {
                        if (Main.projectile[i].velocity == Vector2.Zero)
                        {
                            Main.projectile[i].ai[1] = comingBack;
                        }

                        projectile.rotation = projectile.velocity.ToRotation();
                        player.ChangeDir(projectile.direction);
                    }
                    else if (!player.channel && Main.projectile[i].ai[1] == comingBack)
                    {
                        Main.projectile[i].ai[1] = arrowOut;
                    }

                }

            }
            if (!player.channel || arrowDead)
            {
                projectile.active = false;
                player.heldProj = -1;
            }
            projectile.Center = player.Center;

            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------
    public class ArrowExplosion : ModProjectile
    {
        public override string Texture => "Idkmod/Items/Weapons/Bows/TitanSouls/TitanSlayer";
        public override void SetDefaults()
        {

            projectile.width = 100;               //The width of projectile hitbox
            projectile.height = 100;              //The height of projectile hitbox
                                                 //projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
            projectile.friendly = true;         //Can the projectile deal damage to enemies?
            projectile.hostile = false;         //Can the projectile deal damage to the player?
            projectile.melee = true;           //Is the projectile shoot by a ranged weapon?
            projectile.penetrate = -1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            projectile.timeLeft = 30;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            projectile.alpha = 255;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
            projectile.light = 0.5f;            //How much light emit around the projectile
            projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            projectile.tileCollide = false;          //Can the projectile collide with tiles?
            projectile.extraUpdates = 1;            //Set to above 0 if you want the projectile to update multiple time in a frame
                                                    //aiType = ProjectileID.Bullet;           //Act exactly like default Bullet
        }

        public override void AI()
        {
            projectile.velocity = Vector2.Zero;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Blood, projectile.oldVelocity.X, projectile.oldVelocity.Y, 0, Scale: 0.8f);
                Main.dust[dust].noGravity = true;
            }

            return true;
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------
    public class SoulProj : ModProjectile
    {
        bool goingToPlayer = false;
        Vector2 direction = Vector2.Zero;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TitanSoul");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void SetDefaults()
        {

            projectile.width = 10;            
            projectile.height = 10;           
                                               
            projectile.friendly = false;       
            projectile.hostile = false;        
            projectile.melee = true;           
            projectile.penetrate = -1;         
            projectile.timeLeft = 99999;          
            projectile.alpha = 0;            
            projectile.light = 0.5f;           
            projectile.ignoreWater = true;     
            projectile.tileCollide = false;       
                                               
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            const float speed = 5f;
            const float inertia = 5f;

            if(!goingToPlayer)
            {
                projectile.velocity *= 0.9f;
            }
            if ((projectile.velocity.X < 0.1f && projectile.velocity.X > -0.1f) && (projectile.velocity.Y < 0.1f && projectile.velocity.Y > -0.1f) || goingToPlayer)
            {
                projectile.velocity *= 1.2f;
                goingToPlayer = true;
                direction = player.Center - projectile.Center;
                direction.Normalize();
                direction *= speed;
                projectile.tileCollide = false;
                projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
                if(projectile.Distance(player.Center) < 30f)
                {
                    projectile.Kill();
                    player.AddBuff(ModContent.BuffType<SoulPowered>(), 600);
                    player.statLife += player.statLifeMax / 50;
                    Main.PlaySound(SoundID.NPCDeath6, player.position);
                    player.HealEffect(player.statLifeMax / 50);
                }
            }

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }

            return true;
        }
    }
}


