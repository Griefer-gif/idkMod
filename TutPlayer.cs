using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Idkmod
{
    public class TutPlayer : ModPlayer
    {
        public const int maxUses = 1;
        public int LifeCrystal;

        public override void ResetEffects()
        {
            player.statLifeMax2 += LifeCrystal * 25;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write(LifeCrystal);
            packet.Send(toWho, fromWho);
        }
        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"LifeCrystal",LifeCrystal }
            };
        }

        public override void Load(TagCompound tag)
        {
            LifeCrystal = tag.GetInt("LifeCrystal");
        }

    }
}
