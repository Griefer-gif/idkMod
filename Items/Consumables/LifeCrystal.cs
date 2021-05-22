using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Items.Consumables
{
    public class LifeCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Blue Flame");
            Tooltip.SetDefault("Rests of the Kings spirit, increases hp by 25");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.LifeFruit);
            item.value = Item.sellPrice(0,2,50,0);
            item.maxStack = 99;
        }

        public override bool CanUseItem(Player player)
        {
            return player.statLifeMax == 500 && player.GetModPlayer<idkPlayer>().LifeCrystal < idkPlayer.maxUses;
        }

        public override bool UseItem(Player player)
        {
            player.statLifeMax2 += 30;
            player.statLife += 30;
            player.GetModPlayer<idkPlayer>().LifeCrystal += 1;
            if (Main.myPlayer == player.whoAmI)
            {
                player.HealEffect(30, true);
            }
            return true;
        }
    }
}
