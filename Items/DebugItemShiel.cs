using idkmod.Items.Accessories.Shields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Items
{
    class DebugItemShiel : ModItem
    {   
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("tells you the current shield max health and current health");
        }

        public override void SetDefaults()
        {
			item.width = 20; // hitbox width of the item
			item.height = 20; // hitbox height of the item
			item.scale = 0.8f;
			item.useTime = 1; // The item's use time in ticks (60 ticks == 1 second.)
			item.useAnimation = 1; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			item.reuseDelay = 0;
			item.useStyle = ItemUseStyleID.HoldingOut; // how you use the item (swinging, holding out, etc)
			item.noMelee = true; //so the item's animation doesn't do damage
			item.value = 10000; // how much the item sells for (measured in copper)
			item.rare = ItemRarityID.Quest; // the color that the item's name will be in-game
			item.UseSound = SoundID.Item11; // The sound that this item plays when used.
			item.autoReuse = true;
		}

        public override bool UseItem(Player player)
        {
            //Main.NewText("list start :");

            Main.NewText(player.GetModPlayer<BlPlayer>().psyFlyQueue.Count);
            Main.NewText(player.GetModPlayer<BlPlayer>().PsyFlyBuff);

            return true;
        }
    }
}
