using Idkmod;
using idkmod.Buffs.Minions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using idkmod.Projectiles.ElementalBullets.SlaggBullets;
using Microsoft.Xna.Framework.Graphics;

namespace idkmod.Projectiles.Minions
{
    class PsyFlyMinion : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Example Minion");
			// Sets the amount of frames this minion has on its spritesheet
			Main.projFrames[projectile.type] = 1;
			// This is necessary for right-click targeting
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = false;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[projectile.type] = true;
			// This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			projectile.width = 25;
			projectile.height = 25;
			// Makes the minion go through tiles freely
			projectile.tileCollide = false;

			// These below are needed for a minion weapon
			// Only controls if it deals damage to enemies on contact (more on that later)
			projectile.friendly = true;
			// Only determines the damage type
			projectile.minion = true;
			// Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			projectile.minionSlots = 1f;
			// Needed so the minion doesn't despawn on collision with enemies or tiles
			projectile.penetrate = -1;
			projectile.scale = 1.5f;
		}

		// Here you can decide if your minion breaks things like grass or pots
		public override bool? CanCutTiles()
		{
			return false;
		}

		// This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
		public override bool MinionContactDamage()
		{
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			#region Active check
			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<PsyFlyBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<PsyFlyBuff>()))
			{
				projectile.timeLeft = 2;
			}
			#endregion

			#region General behavior
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)

			// If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
			// The index is projectile.minionPos
			float minionPositionOffsetX = (10 + projectile.minionPos * 40) * -player.direction;
			idlePosition.X += minionPositionOffsetX; // Go behind the player

			// All of this code below this line is adapted from Spazmamini code (ID 388, aiStyle 66)

			// Teleport to player if distance is too big
			Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
			{
				// Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
				// and then set netUpdate to true
				projectile.position = idlePosition;
				projectile.velocity *= 0.1f;
				projectile.netUpdate = true;
			}

			// If your minion is flying, you want to do this independently of any conditions
			float overlapVelocity = 0.04f;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				// Fix overlap with other minions
				Projectile other = Main.projectile[i];
				if (i != projectile.whoAmI && other.active && other.owner == projectile.owner && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width)
				{
					if (projectile.position.X < other.position.X) projectile.velocity.X -= overlapVelocity;
					else projectile.velocity.X += overlapVelocity;

					if (projectile.position.Y < other.position.Y) projectile.velocity.Y -= overlapVelocity;
					else projectile.velocity.Y += overlapVelocity;
				}
			}
			#endregion

			#region Find target
			// Starting search distance
			

			// friendly needs to be set to true so the minion can deal contact damage
			// friendly needs to be set to false so it doesn't damage things like target dummies while idling
			// Both things depend on if it has a target or not, so it's just one assignment here
			// You don't need this assignment if your minion is shooting things instead of dealing contact damage
			projectile.friendly = player.GetModPlayer<idkPlayer>().psyFlyQueue.Count > 0;
			#endregion

			#region Movement

			// Default movement parameters (here for attacking)
			float speed = 30f;
			float inertia = 5f;

			if (player.GetModPlayer<idkPlayer>().psyFlyQueue.Count > 0)
			{
				// Minion has a target: attack (here, fly towards the enemy)
				float distanceFromTarget = Vector2.Distance(player.GetModPlayer<idkPlayer>().psyFlyQueue.Peek().Center, projectile.Center);
				Vector2 targetCenter = player.GetModPlayer<idkPlayer>().psyFlyQueue.Peek().Center;
				// The immediate range around the target (so it doesn't latch onto it when close)
				if (distanceFromTarget > 45f)
				{
					Vector2 direction = targetCenter - projectile.Center;
					direction.Normalize();
					direction *= speed;
					projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
				}
				if (distanceFromTarget < 45f)
				{
					Vector2 Velocity = player.GetModPlayer<idkPlayer>().psyFlyQueue.Peek().velocity.RotatedBy(MathHelper.ToRadians(180));
					if (projectile.owner == Main.myPlayer)
                    {
						Projectile.NewProjectile(projectile.Center, Velocity, ModContent.ProjectileType<PsyFlyReflectProj>(), player.GetModPlayer<idkPlayer>().psyFlyQueue.Peek().damage, player.GetModPlayer<idkPlayer>().psyFlyQueue.Peek().knockBack, Main.myPlayer);
					}
					player.GetModPlayer<idkPlayer>().psyFlyQueue.Peek().active = false;
				}
			}
			else
			{
				speed = 10f;
				if (distanceToIdlePosition > 5f)
				{
					// The immediate range around the player (when it passively floats about)

					// This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
					vectorToIdlePosition.Normalize();
					vectorToIdlePosition *= speed;
					projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
				}
				else if (projectile.velocity == Vector2.Zero)
				{
					// If there is a case where it's not moving at all, give it a little "poke"
					projectile.velocity.X = -0.15f;
					projectile.velocity.Y = -0.05f;
				}
			}
		
			#endregion

			#region Animation and visuals
			// So it will lean slightly towards the direction it's moving
			projectile.rotation = projectile.velocity.X * 0.05f;

			// This is a simple "loop through all frames from top to bottom" animation
			int frameSpeed = 5;
			projectile.frameCounter++;
			if (projectile.frameCounter >= frameSpeed)
			{
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type])
				{
					projectile.frame = 0;
				}
			}

			// Some visuals here
			Lighting.AddLight(projectile.Center, Color.White.ToVector3() * 0.78f);
			#endregion
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Player player = Main.player[projectile.owner];
			if(player.GetModPlayer<idkPlayer>().psyFlyQueue.Count > 0)
            {
				int dust = Dust.NewDust(projectile.Center, 10, 10, DustID.PurpleTorch, Scale: 2f);
				Main.dust[dust].noGravity = true;
			}
            return true;
        }
        
    }
}

