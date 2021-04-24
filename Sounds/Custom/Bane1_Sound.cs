using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace idkmod.Sounds.Custom
{
	public class Bane1_Sound : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			if (soundInstance.State == SoundState.Playing)
			{
				return null;
			}
			soundInstance = sound.CreateInstance();
			soundInstance.Volume = volume * .5f;
			soundInstance.Pan = pan;
			soundInstance.Pitch = Main.rand.Next(-5, 6) * .05f;
			return soundInstance;
		}
	
	}
}