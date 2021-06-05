using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace idkmod.Projectiles.HollowKnight
{
	// was used for OldNail and Sharpened nail but it doesnt work because of the animations
	public class BaseNailBlade : ModProjectile
	{
        public override void SetStaticDefaults()
        {
			Main.projFrames[projectile.type] = 28;
		}

        public override void SetDefaults()
		{
			projectile.width = 70;
			projectile.height = 70;
			projectile.friendly = true;
			projectile.penetrate = 999;
			projectile.tileCollide = false;
			projectile.ownerHitCheck = true; //so you can't hit enemies through walls
			projectile.melee = true;
			//projectile.timeLeft = 2;
		}

        public override void AI()
        {
			Player player = Main.player[projectile.owner];
			Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);

			UpdatePlayerVisuals(player, rrp);

			if (++projectile.frameCounter >= 1)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 28)
				{
					projectile.frame = 0;
				}
			}

			if (projectile.spriteDirection == -1)
			{
				projectile.rotation += MathHelper.Pi;
			}
		}

		private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
		{
			projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * 20f;
			projectile.Center = playerHandPos;
			
			projectile.rotation = projectile.velocity.ToRotation();
			projectile.spriteDirection = projectile.direction;

			if(!player.channel)
            {
				projectile.active = false;
            }
			player.ChangeDir(projectile.direction);
			player.heldProj = projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;

			player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.immune[projectile.owner] = 5;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D texture = Main.projectileTexture[projectile.type];
			int frameHeight = texture.Height / Main.projFrames[projectile.type];
			int spriteSheetOffset = frameHeight * projectile.frame;
			Vector2 sheetInsertPosition = (projectile.Center + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition).Floor();

			Color drawColor = Color.White;
			spriteBatch.Draw(texture, sheetInsertPosition, new Rectangle?(new Rectangle(0, spriteSheetOffset, texture.Width, frameHeight)), drawColor, projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), projectile.scale, effects, 0f);
			return false;
		}

	}
}