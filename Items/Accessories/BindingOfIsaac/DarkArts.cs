using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Idkmod.Items.Accessories.BindingOfIsaac
{
    class DarkArts : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Arts");
            Tooltip.SetDefault("Press the HotKey to become immune to damage and mark enemies, press it again to deal massive damage");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.scale = 0.5f;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.GetModPlayer<BlPlayer>().DANpcs.Count >= 9 && player.GetModPlayer<BlPlayer>().DarkArtsBuff)
            {
                player.velocity = new Vector2(0, 0);
            }
            player.GetModPlayer<BlPlayer>().DarkArts = true;
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
