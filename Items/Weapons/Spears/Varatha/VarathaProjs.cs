using Idkmod.Projectiles.Hades.Varatha;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Items.Weapons.Spears.Varatha
{
	//spear projectile
    public class VarathaSpearProj : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("VarathaProj");
		}

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.aiStyle = 19;
			projectile.penetrate = -1;
			projectile.scale = 1.3f;
			projectile.alpha = 0;

			projectile.hide = true;
			projectile.ownerHitCheck = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.friendly = true;
		}

		// In here the AI uses this example, to make the code more organized and readable
		// Also showcased in ExampleJavelinProjectile.cs
		public float movementFactor // Change this value to alter how fast the spear moves
		{
			get => projectile.ai[0];
			set => projectile.ai[0] = value;
		}

		// It appears that for this AI, only the ai0 field is used!
		public override void AI()
		{
			// Since we access the owner player instance so much, it's useful to create a helper local variable for this
			// Sadly, Projectile/ModProjectile does not have its own
			Player projOwner = Main.player[projectile.owner];
			// Here we set some of the projectile's owner properties, such as held item and itemtime, along with projectile direction and position based on the player
			Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
			projectile.direction = projOwner.direction;
			projOwner.heldProj = projectile.whoAmI;
			projOwner.itemTime = projOwner.itemAnimation;
			projectile.position.X = ownerMountedCenter.X - (float)(projectile.width / 2);
			projectile.position.Y = ownerMountedCenter.Y - (float)(projectile.height / 2);
			// As long as the player isn't frozen, the spear can move
			if (!projOwner.frozen)
			{
				if (movementFactor == 0f) // When initially thrown out, the ai0 will be 0f
				{
					movementFactor = 3f; // Make sure the spear moves forward when initially thrown out
					projectile.netUpdate = true; // Make sure to netUpdate this spear
				}
				if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3) // Somewhere along the item animation, make sure the spear moves back
				{
					movementFactor -= 2.4f;
				}
				else // Otherwise, increase the movement factor
				{
					movementFactor += 2.1f;
				}
			}
			// Change the spear position based off of the velocity and the movementFactor
			projectile.position += projectile.velocity * movementFactor;
			// When we reach the end of the animation, we can kill the spear projectile
			if (projOwner.itemAnimation == 0)
			{
				projectile.Kill();
			}
			// Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
			// MathHelper.ToRadians(xx degrees here)
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
			// Offset by 90 degrees here
			if (projectile.spriteDirection == -1)
			{
				projectile.rotation -= MathHelper.ToRadians(90f);
			}

			projOwner.ChangeDir(projectile.direction);
		}
	}

	//holdout projectile for charging attacks and throwing
	public class VarathaSwingProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.width = 20;
			projectile.height = 20;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 999999;
			projectile.scale = 1.1f;
		}

		int counter = 0;
		int stacks = 0;
		public override bool PreAI()
		{


			Player player = Main.player[projectile.owner];
			projectile.velocity = new Vector2(0, -1);

			Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);
			UpdatePlayerVisuals(player, rrp);
			if (player.channel)
			{
				//player.ChangeDir(projectile.direction);
				//player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();
				counter++;
				if (counter % 40 == 0 && stacks < 3)
				{
					stacks++;
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
				if (stacks > 0)
				{
					Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<VarathaSpinAttack>(), projectile.damage * stacks, projectile.knockBack, projectile.owner, stacks);
				}
				else
				{
					Projectile.NewProjectile(player.position, Vector2.Normalize(Main.MouseWorld - player.position) * 10f, ModContent.ProjectileType<VarathaThrowProj>(), projectile.damage * 3, projectile.knockBack, projectile.owner);
				}
				projectile.active = false;
			}
			player.heldProj = projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
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
			projectile.Center = playerHandPos - new Vector2(0, 60);

			projectile.rotation = projectile.velocity.ToRotation();
			projectile.spriteDirection = projectile.direction;

			player.ChangeDir(projectile.direction);
			player.heldProj = projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;

			// If you do not multiply by projectile.direction, the player's hand will point the wrong direction while facing left.
			player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
		}
	}
}
