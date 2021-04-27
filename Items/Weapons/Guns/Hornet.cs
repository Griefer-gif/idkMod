using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Idkmod.Items.Weapons.Guns
{
    class Hornet : ModItem
    {
		

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hornet");
			Tooltip.SetDefault("Right click to fire a burst of innacurate bullets.");
		}

		public int num = 0;

		public override void SetDefaults()
		{
			item.damage = 20; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			item.ranged = true; // sets the damage type to ranged
			item.width = 20; // hitbox width of the item
			item.height = 20; // hitbox height of the item
			item.scale = 0.8f;
			item.useTime = 5; // The item's use time in ticks (60 ticks == 1 second.)
			item.useAnimation = 10; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			item.reuseDelay = 15;
			item.useStyle = ItemUseStyleID.HoldingOut; // how you use the item (swinging, holding out, etc)
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 4; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			item.value = 10000; // how much the item sells for (measured in copper)
			item.rare = ItemRarityID.Quest; // the color that the item's name will be in-game
			item.UseSound = SoundID.Item11; // The sound that this item plays when used.
			item.autoReuse = true; // if you can hold click to automatically use it again
			item.shoot = ProjectileID.PurificationPowder; //idk why but all the guns in the vanilla source have this
			item.shootSpeed = 16f; // the speed of the projectile (measured in pixels per frame)
			item.useAmmo = AmmoID.Bullet; // The "ammo Id" of the ammo item that this weapon uses. Note that this is not an item Id, but just a magic value.
		}

		public override bool ConsumeAmmo(Player player)
		{
			// Because of how the game works, player.itemAnimation will be 11, 7, and finally 3. (useAnimation - 1, then - useTime until less than 0.) 
			// We can get the Clockwork Assault Riffle Effect by not consuming ammo when itemAnimation is lower than the first shot.
			return !(player.itemAnimation < item.useAnimation - 2);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.useStyle = ItemUseStyleID.HoldingOut;
				item.useTime = 5;
				item.useAnimation = 30;
				item.damage = 10;
				
			}
			else
			{
				item.useStyle = ItemUseStyleID.HoldingOut;
				item.useTime = 0;
				item.useAnimation = 0;
				item.damage = 30;
				
			}
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			var R = new Random();
			// Fix the speedX and Y to point them horizontally.
			
			// Add random Rotation
			Vector2 speed = new Vector2(speedX, speedY);
			if (player.altFunctionUse == 2)
			{
				speed = speed.RotatedByRandom(MathHelper.ToRadians(R.Next(30)));
				
				if (type == ProjectileID.Bullet) // or ProjectileID.WoodenArrowFriendly
				{
					type = ProjectileID.CursedBullet; // or ProjectileID.FireArrow;
				}
			}
					
			speedX = speed.X;
			speedY = speed.Y;
			
			return true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "", "Converts musket balls to Cursed Bullets while bursting."));
			var quote = new TooltipLine(mod, "", "'Fear the Swarm!'");
			quote.overrideColor = Color.Red;
			tooltips.Add(quote);
		}
	}
}
