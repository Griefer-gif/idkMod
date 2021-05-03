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
	class BasicRepeater : ModItem
	{


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Basic repeater");
			//Tooltip.SetDefault("Only consumes 1 bullet per shot.");
		}

		public int num = 0;

		public override void SetDefaults()
		{
			item.damage = 10;
			item.ranged = true; 
			item.width = 20; 
			item.height = 20; 
			item.scale = 0.8f;
			item.useTime = 5; 
			item.useAnimation = 10; 
			item.reuseDelay = 30;
			item.useStyle = ItemUseStyleID.HoldingOut; 
			item.noMelee = true; 
			item.knockBack = 4;
			item.value = 10000; 
			item.rare = ItemRarityID.White; 
			item.UseSound = SoundID.Item11; 
			item.autoReuse = true; 
			item.shoot = ProjectileID.PurificationPowder; 
			item.shootSpeed = 10f; 
			item.useAmmo = AmmoID.Bullet;
		}

		public override bool ConsumeAmmo(Player player)
		{
			// Because of how the game works, player.itemAnimation will be 11, 7, and finally 3. (useAnimation - 1, then - useTime until less than 0.) 
			// We can get the Clockwork Assault Riffle Effect by not consuming ammo when itemAnimation is lower than the first shot.
			return !(player.itemAnimation < item.useAnimation - 2);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			var R = new Random();

			Vector2 speed = new Vector2(speedX, speedY);
			speed = speed.RotatedByRandom(MathHelper.ToRadians(R.Next(2)));

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
			//tooltips.Add(new TooltipLine(mod, "", "Converts musket balls to Cursed Bullets while bursting."));
			var quote = new TooltipLine(mod, "", "'Your first gun!'");
			quote.overrideColor = Color.Red;
			tooltips.Add(quote);
		}
	}
}
