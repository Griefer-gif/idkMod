using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Idkmod.Items.Accessories
{
    class DarkArts : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
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
            player.GetModPlayer<BlPlayer>().DarkArts = true;
            //player.AddBuff(ModContent.BuffType<Buffs.DarkArtsBuff>(), 600);
        }

    }
}
