using Idkmod.Items.Weapons.Swords.HollowKnight;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Projectiles.HollowKnight
{
	// was used for OldNail and Sharpened nail but it doesnt work because of the animations
	public class UpgradedNailBlade : ModProjectile
	{
		int timer = 0;
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
			projectile.scale = 2f;
			//projectile.timeLeft = 2;
		}

		public override void AI()
		{
			timer++;
			
			Player player = Main.player[projectile.owner];
			Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);

			UpdatePosition(player, rrp);

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

			//nail shoot projectile checks
			if (player.HeldItem.type == ModContent.ItemType<ChannelledNail>() && timer % 60 == 0)
            {
				Projectile.NewProjectile(projectile.Center, Vector2.Normalize(projectile.velocity) * 50f, ModContent.ProjectileType<NailSpell1Proj>(), projectile.damage * 2, projectile.knockBack, Main.myPlayer);
			}

			if (player.HeldItem.type == ModContent.ItemType<CoiledNail>() && timer % 60 == 0)
			{
				Projectile.NewProjectile(projectile.Center, Vector2.Normalize(projectile.velocity) * 50f, ModContent.ProjectileType<NailSpell1ProjUP>(), projectile.damage * 2, projectile.knockBack, Main.myPlayer);
			}

			if (player.HeldItem.type == ModContent.ItemType<PureNail>() && timer % 30 == 0)
			{
				Projectile.NewProjectile(projectile.Center, Vector2.Normalize(projectile.velocity) * 50f, ModContent.ProjectileType<NailSpell1ProjUP>(), projectile.damage * 4, projectile.knockBack, Main.myPlayer);
			}
		}

		private void UpdatePosition(Player player, Vector2 playerHandPos)
		{
			//takes care of the rotation and sets the position to the players hand
			projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * 20f;
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