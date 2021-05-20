using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace idkmod.Projectiles.HollowKnight
{
	//ported from my tAPI mod because I don't want to make artwork
	public class OldNailProj : ModProjectile
	{
		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.Arkhalis);
			projectile.width = 60;
			projectile.height = 60;
			//aiType = 959;
			//projectile.aiStyle = 75;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			//projectile.hide = true;
			projectile.ownerHitCheck = true; //so you can't hit enemies through walls
			projectile.melee = true;
			projectile.timeLeft = 3;
		}

        public override void AI()
        {
			Player player = Main.player[projectile.owner];
			Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);

			//projectile.position = player.position
			UpdatePlayerVisuals(player, rrp);
			projectile.Center += Vector2.Normalize(rrp);
		}


		private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
		{
			// Place the Prism directly into the player's hand at all times.
			//projectile.position = playerHandPos;
			// The beams emit from the tip of the Prism, not the side. As such, rotate the sprite by pi/2 (90 degrees).
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			projectile.spriteDirection = projectile.direction;

			// The Prism is a holdout projectile, so change the player's variables to reflect that.
			// Constantly resetting player.itemTime and player.itemAnimation prevents the player from switching items or doing anything else.
			player.ChangeDir(projectile.direction);
			player.heldProj = projectile.whoAmI;
			player.itemTime = 15;
			player.itemAnimation = 15;

			// If you do not multiply by projectile.direction, the player's hand will point the wrong direction while facing left.
			player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D texture = Main.projectileTexture[projectile.type];
			int frameHeight = texture.Height / Main.projFrames[projectile.type];
			int spriteSheetOffset = frameHeight * projectile.frame;
			Vector2 sheetInsertPosition = (projectile.Center + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition).Floor();

			// The Prism is always at full brightness, regardless of the surrounding light. This is equivalent to it being its own glowmask.
			// It is drawn in a non-white color to distinguish it from the vanilla Last Prism.
			Color drawColor = Color.White;
			spriteBatch.Draw(texture, sheetInsertPosition, new Rectangle?(new Rectangle(0, spriteSheetOffset, texture.Width, frameHeight)), drawColor, projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), projectile.scale, effects, 0f);
			return false;
		}

	}
}