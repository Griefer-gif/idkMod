using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Items.Weapons.Misc.Aegis
{
    public class AegisHoldoutProj : ModProjectile
    {
        public override string Texture => "Idkmod/Items/Weapons/Misc/Aegis/AegisHoldout";

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.width = 60;
            projectile.height = 60;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            
            projectile.penetrate = 1;
            projectile.tileCollide = false;
            projectile.timeLeft = 999999;
        }

        private int counter = 0;
        
        private bool attackR = false;
        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);
            UpdatePlayerVisuals(player, rrp);
            
            if (player.channel)
            {
                
                counter++;
                if (counter == 120)
                {
                    attackR = true;
                   
                    Main.PlaySound(SoundID.MaxMana, (int)projectile.position.X, (int)projectile.position.Y);
                    for (int i = 0; i < 100; i++)
                    {
                        DoDustEffect(player.Center, 40f);
                    }
                }
            }
            else
            {
                if (projectile.owner == Main.myPlayer)
                {
                    if (attackR && counter >= 30)
                    {
                        Main.NewText("proj");
                        Vector2 vel = Vector2.Normalize(Main.MouseWorld - player.Center) * 30f;
                        Projectile.NewProjectile(player.Center, vel, ModContent.ProjectileType<DashProj>(), projectile.damage, projectile.knockBack * 2, projectile.owner);
                    }
                    else
                    {
                        //everything else
                        //spawn hit projectile
                        projectile.active = false;
                    }
                }
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
            projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * 15f;

            projectile.Center = playerHandPos;

            projectile.rotation = projectile.velocity.ToRotation();
            projectile.spriteDirection = projectile.direction;

            //allow the projectile to channel and kills it if the player stops
            if (!player.channel)
            {
                projectile.active = false;
            }
            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();

            if (projectile.spriteDirection == -1)
            {
                projectile.rotation += MathHelper.Pi;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            int spriteSheetOffset = frameHeight * projectile.frame;
            Vector2 sheetInsertPosition = (projectile.Center + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition).Floor();

            spriteBatch.Draw(texture, sheetInsertPosition, new Rectangle?(new Rectangle(0, spriteSheetOffset, texture.Width, frameHeight)), Color.White, projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), projectile.scale, effects, 0f);
            return false;
        }
    }

    public class DashProj : ModProjectile
    {
        public override string Texture => "Idkmod/Items/Weapons/Misc/Aegis/AegisHoldout";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;//The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.width = 40;
            projectile.height = 40;
            projectile.aiStyle = -1;
            projectile.friendly = true;

            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.timeLeft = 60;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 PlayerSpeedExtra = projectile.velocity;
            PlayerSpeedExtra.Normalize();
            player.velocity += PlayerSpeedExtra * 0.5f;

            if (projectile.owner == Main.myPlayer)
            {
                UpdatePlayer(player, rrp);
            }

            if(projectile.timeLeft <= 55)
            {
                projectile.width = 55;
                projectile.height = 55;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            Vector2 PlayerKnockback = projectile.velocity.RotatedBy(Math.PI);
            PlayerKnockback.Normalize();
            player.velocity += PlayerKnockback * 10f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[projectile.owner];
            Vector2 PlayerKnockback = projectile.velocity.RotatedBy(Math.PI);
            PlayerKnockback.Normalize();
            player.velocity += PlayerKnockback * 10f;
            return true;
        }

        private void UpdatePlayer(Player player, Vector2 playerHandPos)
        {
            //projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * 15f;
            player.Center = projectile.Center;

            projectile.rotation = projectile.velocity.ToRotation();
            projectile.spriteDirection = projectile.direction;

            
            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();

            if (projectile.spriteDirection == -1)
            {
                projectile.rotation += MathHelper.Pi;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //special drawcode so the projectile looks right
            SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            int spriteSheetOffset = frameHeight * projectile.frame;
            Vector2 sheetInsertPosition = (projectile.Center + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition).Floor();

            spriteBatch.Draw(texture, sheetInsertPosition, new Rectangle?(new Rectangle(0, spriteSheetOffset, texture.Width, frameHeight)), Color.White, projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), projectile.scale, effects, 0f);

            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(projectile.position, 45, 55, DustID.Pixie, projectile.velocity.X * 0.3f, projectile.velocity.Y * 0.3f, newColor: Color.Black, Scale: 1f);
                Dust.NewDust(projectile.position, 45, 55, DustID.Pixie, projectile.velocity.X * 0.3f, projectile.velocity.Y * 0.3f, newColor: Color.DarkRed, Scale: 1f);
            }
            return false;
        }

    }
}
