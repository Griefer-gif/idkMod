
using Idkmod.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Idkmod
{

    public class Idkmod : Mod
    {
        //idk exactly why an instance is needed, but spirit uses it to get textures, so ima yoink it
        public static Idkmod Instance;
        public static ModHotKey DarkArtsHotKey;
        public static ModHotKey NailSpell1HK;
        public static ModHotKey NailSpell2HK;
        private UserInterface _shieldHealthBar;
        internal ShieldHealthBar shieldHealthBar;

        public Idkmod()
        {
            Instance = this;
        }

        public override void Load()
        {
            Instance = this;
            if (!Main.dedServ)
            {
                shieldHealthBar = new ShieldHealthBar();
                _shieldHealthBar = new UserInterface();
                _shieldHealthBar.SetState(shieldHealthBar);
            }

            DarkArtsHotKey = RegisterHotKey("Dark Arts", "F");
            NailSpell1HK = RegisterHotKey("Nail Spell 1", "R");
            NailSpell2HK = RegisterHotKey("Nail Spell 2", "G");
        }

        public override void UpdateUI(GameTime gameTime)
        {
            _shieldHealthBar?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "idkmod: Shield Recharge Bar",
                    delegate {
                        _shieldHealthBar.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }


        public override void Unload()
        {
            Instance = null;
            DarkArtsHotKey = null;
            NailSpell1HK = null;
            NailSpell2HK = null;
        }
    }
}