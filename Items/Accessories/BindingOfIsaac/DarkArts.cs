using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Items.Accessories.BindingOfIsaac
{
    class DarkArts : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Arts");
            Tooltip.SetDefault($"Press F (default) to become immune to damage and mark enemies,\nPress F again to deal damage.\nDamage scales with the damage of the item you are holding.");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.scale = 0.5f;
            item.accessory = true;
            item.rare = ItemRarityID.Red;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.GetModPlayer<idkPlayer>().DANpcs.Count >= 9 && player.GetModPlayer<idkPlayer>().DarkArtsBuff)
            {
                player.velocity = new Vector2(0, 0);
            }
            player.GetModPlayer<idkPlayer>().DarkArts = true;
            //player.AddBuff(ModContent.BuffType<Buffs.DarkArtsBuff>(), 600);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var quote = new TooltipLine(mod, "", "One with the shadows");
            quote.overrideColor = Color.Gray;
            tooltips.Add(quote);
        }

    }
}
