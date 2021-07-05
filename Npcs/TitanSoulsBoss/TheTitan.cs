using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Idkmod.Npcs.TitanSoulsBoss
{
    class TheTitan : ModNPC
    {
		//--------------------------------------------------//idea//----------------------------------------------------------
		//imma write the whole boss idea here because yes
		//
		//prob a pre skeletron boss, cant be before EOC
		//when the player uses the boss spawn, it wil spawn offscreen and fall till it hits a tile or platform(idk how to make it hit platforms), and then spawn his 2 hands that hover at his sides, the hands go through walls but the main body doesnt
		//the boss hovers over the player with 2 hands(another npc with anoter ai), every 2 seconds he tries to hit the player with one of his hands, alternating between both, whn at half health it gets faster
		//
		//------------------------------------------------------------------------------------------------------------------
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Titan"); 
            Main.npcFrameCount[npc.type] = 1; // make sure to set this for your modnpcs.
        }

		public override void SetDefaults()
		{
			npc.width = 128;
			npc.height = 128;
			npc.aiStyle = -1;
			npc.damage = 7;
			npc.defense = 2;
			npc.lifeMax = 25;
			npc.friendly = false;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 25f;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Confused] = true; // npc default to being immune to the Confused debuff. Allowing confused could be a little more work depending on the AI. npc.confused is true while the npc is confused.
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Custom/Boss/TheTitanTheme.wav");
		}


		//what the ai[] positions are used for
		private const int AI_State_Slot = 0;
		private const int AI_Timer_Slot = 1;
		private const int AI_NPCTimeAlive = 2;
		private const int AI_Unused_Slot_2 = 3;

		//yep
		private const int Local_AI_Unused_Slot_0 = 0;
		private const int Local_AI_Unused_Slot_1 = 1;
		private const int Local_AI_Unused_Slot_2 = 2;
		private const int Local_AI_Unused_Slot_3 = 3;

		//the state of the npc that goes into ai[]
		private const int State_JustSpawned = 0;
		private const int State_HoveringOverTarget = 1;

		private const int HandState_HoveringAtSide = 0;
		private const int HandState_HoveringOverTarget = 1;
		private const int HandState_Stomping = 2;

		public float AI_State
		{
			get => npc.ai[AI_State_Slot];
			set => npc.ai[AI_State_Slot] = value;
		}

		public float AI_Timer
		{
			get => npc.ai[AI_Timer_Slot];
			set => npc.ai[AI_Timer_Slot] = value;
		}

		public float AI_TimeAlive
		{
			get => npc.ai[AI_NPCTimeAlive];
			set => npc.ai[AI_NPCTimeAlive] = value;
		}

		Vector2 targetHoverPos = new Vector2();
		const float NPCspeed = 7f;
		const float NPCinertia = 10f;
		Projectile leftHand;
		Projectile rightHand;
		public override void AI()
        {
			Player target = Main.player[npc.target];
			AI_TimeAlive++;

			for(int i = 0; i < Main.projectile.Count(); i++)
            {
				Projectile Proj = Main.projectile[i];
				if(Proj.type == ModContent.ProjectileType<LeftHand>() && Proj.active && Proj.ai[1] == npc.whoAmI)
                {
					leftHand = Proj;
                }
            }

            if(AI_State == State_JustSpawned)
            {
				FallDownSpawn();
            }
            else
            {
				//after the boss falls, set it as a boss and make it not affected by gravity
				npc.boss = true;
				npc.noGravity = true;
            }

			if(AI_State == State_HoveringOverTarget)
            {
				AI_Timer++;
				npc.TargetClosest(true);
				targetHoverPos = new Vector2(target.position.X, target.position.Y - 400f) - npc.Center;
				targetHoverPos.Normalize();
				targetHoverPos *= NPCspeed;
				npc.velocity = (npc.velocity * (NPCinertia - 1) + targetHoverPos) / NPCinertia;
				if(AI_Timer % 300 == 0 && leftHand.ai[0] != HandState_HoveringOverTarget && leftHand.ai[0] != HandState_Stomping)
                {
					leftHand.ai[0] = 1;
                }
			}
        }

		private void FallDownSpawn()
        {
			if (AI_TimeAlive > 10 && npc.velocity.Y == 0)
			{
				for (int i = 0; i < 50; i++)
				{
					Vector2 DustPos = new Vector2(npc.position.X, npc.position.Y + 16f);
					Dust.NewDust(DustPos, 5, 1, DustID.Smoke, 5, 0);
					Dust.NewDust(DustPos, 5, 5, DustID.Smoke, -5, 0);
				}
				
				Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode, npc.Center);
				Projectile.NewProjectile(npc.Center - new Vector2(40, 10), Vector2.Zero, ModContent.ProjectileType<LeftHand>(), npc.damage, 10, Main.myPlayer, 0, npc.whoAmI);
				AI_State = State_HoveringOverTarget;
			}
			npc.velocity.Y = 10f;
		}
    }

	public class LeftHand : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titan left hand");
		}

		public override void SetDefaults()
		{
			projectile.width = 64;
			projectile.height = 64;
			projectile.aiStyle = -1; 
			projectile.damage = 100;
			projectile.tileCollide = false;
			projectile.penetrate = 99999;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.timeLeft = 10;
		}


		//what the ai[] positions are used for
		private const int AI_State_Slot = 0;
		private const int AI_Owner_Slot = 1;
		
		//vars
		private int AI_Timer = 0;

		//yep
		private const int Local_AI_Unused_Slot_0 = 0;
		private const int Local_AI_Unused_Slot_1 = 1;
		private const int Local_AI_Unused_Slot_2 = 2;
		private const int Local_AI_Unused_Slot_3 = 3;

		//the state of the projectile that goes into ai[0]
		private const int State_HoveringAtSide = 0;
		private const int State_HoveringOverTarget = 1;
		private const int State_Stomping = 2;

		public float AI_State
		{
			get => projectile.ai[AI_State_Slot];
			set => projectile.ai[AI_State_Slot] = value;
		}

		public NPC AI_HandOwner
		{
			get => Main.npc[(int)projectile.ai[AI_Owner_Slot]];
			set => AI_HandOwner = value;
		}

		Vector2 HoverPos = Vector2.Zero;
		float speed = 7f;
		float inertia = 10f;
		public override void AI()
		{
			//first of all, check if the body is alive, if not, kill the projectile
			if (!AI_HandOwner.active)
			{
				projectile.active = false;
			}

			//so the projectile never times out
			projectile.timeLeft = 10;

			//set the idle position to the left of the body
			HoverPos = AI_HandOwner.Center - new Vector2(100, 0);
			
			//pretty self-explanatory
			if(AI_State == State_HoveringAtSide)
            {
				HoverAtBodySide();
			}

			if (AI_State == State_HoveringOverTarget && AI_HandOwner.HasPlayerTarget)
			{
				HoverAboveTarget();
			}

			if(AI_State == State_Stomping)
            {
				StompTarget();
            }
		}

		private void HoverAtBodySide()
        {
			projectile.hostile = false;
			Vector2 direction = HoverPos - projectile.Center;
			direction.Normalize();
			direction *= speed;
			//Main.NewText("projectile " + projectile.ai[AI_Owner_Slot]);
			projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
		}

		private void HoverAboveTarget()
        {
			speed = 15f;
			inertia = 5f;

			float positionOffSet = 250f;
			Vector2 direction = Main.player[AI_HandOwner.target].Center - projectile.Center;
			direction.Y -= positionOffSet;
			if (Vector2.Distance(projectile.Center, Main.player[AI_HandOwner.target].Center - new Vector2(0, positionOffSet)) <= 10f)
			{
				AI_State = State_Stomping;
			}
			direction.Normalize();
			direction *= speed;
			projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
		}

		private void StompTarget()
		{
			AI_Timer++;
			projectile.velocity.X = 0f;
			projectile.velocity.Y = 25f;
			
			if(AI_Timer > 60)
            {
				AI_State = State_HoveringAtSide;
				AI_Timer = 0;
			
			}
		}
	}

	public class RightHand : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titan left hand");
		}

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.aiStyle = -1;
			//projectile.damage = 100;
			projectile.tileCollide = false;
			projectile.penetrate = 99999;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.timeLeft = 2;
		}

        public override void AI()
        {
			Main.NewText("ai1: " + projectile.ai[1]);
			Main.NewText("ai0: " + projectile.ai[0]);
		}
    }
}
