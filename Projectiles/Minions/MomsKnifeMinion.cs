using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using idkmod.Buffs.Minions;
using Microsoft.Xna.Framework;
using Idkmod.Buffs.Minions;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace Idkmod.Projectiles.Minions
{
	public class ExampleMinionItem : ModItem
	{
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Example Minion Item");
			Tooltip.SetDefault("Summons an example minion to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 30;
			item.knockBack = 3f;
			item.mana = 10;
			item.width = 32;
			item.height = 32;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.buyPrice(0, 30, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<MomsKnifeBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = ModContent.ProjectileType<MomsKnifeMinion>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(item.buffType, 2);

			// Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position.
			position = Main.MouseWorld;
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SoulofFright, 25);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}


	public class MomsKnifeMinion : ModProjectile
	{
		int timer = 0;
		bool attacking = false;

		private void UpdatePlayerVisuals()
		{
			if(projectile.velocity == new Vector2(0, 0))
            {
				return;
            }
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			projectile.spriteDirection = projectile.direction;

		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mom's knife");
			// This is necessary for right-click targeting
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[projectile.type] = true;
			// This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[projectile.type] = true;

			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public sealed override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 28;
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

			UpdatePlayerVisuals();

			#region Active check
			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<MomsKnifeBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<MomsKnifeBuff>()))
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
			float distanceFromTarget = 1000f;
			Vector2 targetCenter = projectile.position;
			bool foundTarget = false;

			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, projectile.Center);
				// Reasonable distance away so it doesn't target across multiple screens
				if (between < 2000f)
				{
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
				}
			}
			if (!foundTarget)
			{
				// This code is required either way, used for finding a target
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy())
					{
						float between = Vector2.Distance(npc.Center, projectile.Center);
						bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;

						if (((closest && inRange) || !foundTarget))
						{
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}

			// friendly needs to be set to true so the minion can deal contact damage
			// friendly needs to be set to false so it doesn't damage things like target dummies while idling
			// Both things depend on if it has a target or not, so it's just one assignment here
			// You don't need this assignment if your minion is shooting things instead of dealing contact damage
			projectile.friendly = foundTarget;
			#endregion

			#region Movement

			// Default movement parameters (here for attacking)
			float speed = 40f;
			float inertia = 5f;
			
			if (foundTarget)
			{
				if(projectile.velocity == new Vector2(0, 0))
                {
					projectile.rotation = projectile.DirectionTo(targetCenter).ToRotation() + MathHelper.PiOver2;
				}
				
				Vector2 direction = new Vector2();
				if(timer >= 1 && timer < 300)
                {
					projectile.velocity = new Vector2(0, 0);
					direction = targetCenter - projectile.Center;
					direction.Normalize();
				}
				
				if (distanceFromTarget < 200f && attacking == false)
                {
					timer++;

					if (timer >= 200)
                    {
						direction *= speed;
						projectile.velocity = (projectile.velocity * direction);
						attacking = true;
						timer = 0;
					}
                }
                else
                {
					if(attacking == true && distanceFromTarget >= 200)
                    {
						attacking = false;
					}
					direction = targetCenter - projectile.Center;
					direction.Normalize();
					direction *= speed;
					projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
				}
			}
			else
			{
				attacking = false;
				// Minion doesn't have a target: return to player and idle
				if (distanceToIdlePosition > 600f)
				{
					// Speed up the minion if it's away from the player
					speed = 50f;
					inertia = 60f;
				}
				else
				{
					// Slow down the minion if closer to the player
					speed = 10f;
					inertia = 80f;
				}
				if (distanceToIdlePosition > 20f)
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
			
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if(timer > 0)
            {
				Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
				Vector2 drawPos = projectile.oldPos[0] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY) - new Vector2(timer / 6, 0);
				Vector2 drawPos2 = projectile.oldPos[0] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY) + new Vector2(timer / 6, 0);

				Color color = projectile.GetAlpha(lightColor) * 0.5f;
				// idea: change the individual values
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos2, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			if(attacking)
            {
				Random r = new Random();
				//Redraw the projectile with the color not influenced by light
				Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);

					Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
					spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);

				}

				for (int i = 0; i < r.Next(5, 20); i++)
				{
					var dust = Dust.NewDust(projectile.oldPosition, projectile.width, projectile.height, DustID.Blood, projectile.oldVelocity.X, projectile.oldVelocity.Y, 0, Scale: 1.5f);
					Main.dust[dust].noGravity = true;
				}
			}
			
			return true;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			
        }
    }
}
