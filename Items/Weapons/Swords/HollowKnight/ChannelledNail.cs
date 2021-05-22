using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using idkmod.Projectiles.HollowKnight;
using Idkmod.Projectiles.HollowKnight;
using Microsoft.Xna.Framework;

namespace Idkmod.Items.Weapons.Swords.HollowKnight
{
    class ChannelledNail : ModItem
    {
		public int projCount = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Channelled nail");
			Tooltip.SetDefault("A fine looking nail, a cleft weapon of Hallownest. The blade is exquisitly balanced.");
		}

		public override void SetDefaults()
		{
			item.damage = 25;
			item.melee = true;
			item.width = 20;
			item.height = 12;
			item.useTime = 10;
			item.useAnimation = 10;
			item.reuseDelay = 30;
			item.channel = false;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 8;
			item.value = Item.buyPrice(0, 0, 0, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			//item.CloneDefaults(ItemID.Arkhalis);
			item.shoot = ModContent.ProjectileType<ChanelledNailBladeProj>();
			item.shootSpeed = 20f;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if(projCount == 5)
            {
				Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<NailSpell1Proj>(), damage, knockBack, Main.myPlayer);
				projCount = 0;
            }
			projCount++;
			return true;
        }

        public override void HoldItem(Player player)
		{
			player.GetModPlayer<BlPlayer>().NailSpell1UP = true;
		}
	}
}
