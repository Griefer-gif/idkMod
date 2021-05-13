using idkmod.Projectiles.Minions;
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
    class PsyFly : ModItem
    {
        Vector2 position = new Vector2();
        Vector2 velocity = new Vector2(0, 0);
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Psy Fly");
            Tooltip.SetDefault("Spawns a familiar that blocks incoming projectiles");
            
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.scale = 0.5f;
            item.accessory = true;
            item.buffType = ModContent.BuffType<idkmod.Buffs.Minions.PsyFlyBuff>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //if (player.GetModPlayer<BlPlayer>().psyFlyQueue.Count > 0)
            //{
            //    player.GetModPlayer<BlPlayer>().psyFlyQueue.Peek().active = false;
            //}
            player.AddBuff(ModContent.BuffType<idkmod.Buffs.Minions.PsyFlyBuff>(), 2000);
            player.AddBuff(item.buffType, 2);
            position = player.position;
            //player.GetModPlayer<BlPlayer>().PsyFlyEquip = true;
        }
    }
}