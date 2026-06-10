using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterSound : MonoBehaviour
{
	public List<AudioClip> coinSoundClips;

	public AudioClip coinSoundClipOnMagnet;

	public AudioClip zomblingAttackClip;

	public AudioClip lankieAttackClip;

	public AudioClip fatsoAttackClip;

	public AudioClip goliathAttackClip;

	public AudioClip sweeperAttackClip;

	public AudioClip berserkerAttackClip;

	public AudioClip thunderAttackSoundClip;

	public AudioClip flameAttackSoundClip;

	public AudioClip getItemSoundClip;

	public AudioClip cutBambooSoundClip;

	public AudioClip dashSoundClip;

	public AudioClip jumpSoundClip;

	public AudioClip stompSoundClip;

	public AudioClip slideSoundClip;

	public AudioClip throwShurikenSoundClip;

	public AudioClip deadSoundClip;

	public AudioClip footStepSoundClip;

	public AudioClip shieldActivateSoundClip;

	public AudioClip shieldUsedSoundClip;

	public AudioClip gateSoundClip;

	public AudioClip multiplierBoosterSoundClip;

	public float coinSoundUpTime;

	private int coinSoundIndex;

	private float coinLeftTime;

	private bool isGoldBuffOn;

	private void OnEnterGateDoor(GameObject doorIn)
	{
		PlaySound(gateSoundClip);
	}

	private void Update()
	{
		if (coinLeftTime > 0f)
		{
			coinLeftTime = Mathf.Clamp(coinLeftTime - Time.deltaTime, 0f, coinLeftTime);
			if (coinLeftTime == 0f)
			{
				coinSoundIndex = 0;
			}
		}
	}

	private void OnWalk()
	{
		PlaySound(footStepSoundClip);
	}

	private void OnGetCoin()
	{
		if (!isGoldBuffOn)
		{
			PlaySound(coinSoundClips[coinSoundIndex]);
			coinLeftTime = coinSoundUpTime;
			coinSoundIndex = Mathf.Clamp(coinSoundIndex + 1, 0, coinSoundClips.Count - 1);
		}
		else
		{
			PlaySound(coinSoundClipOnMagnet);
		}
	}

	private void OnGetScroll(ScrollElement scrollElement)
	{
		PlaySound(getItemSoundClip);
	}

	private void OnEnterLuckyBox(GameObject box)
	{
		PlaySound(getItemSoundClip);
	}

	private void OnEnterBamboo(GameObject bamboo)
	{
		PlaySound(cutBambooSoundClip);
	}

	private void OnKilledZombie(ZombieType zombieName)
	{
		if (((global::System.Enum)zombieName).Equals((object)ZombieType.zombling))
		{
			PlaySound(zomblingAttackClip);
		}
		else if (((global::System.Enum)zombieName).Equals((object)ZombieType.lankie))
		{
			PlaySound(lankieAttackClip);
		}
		else if (((global::System.Enum)zombieName).Equals((object)ZombieType.fatso))
		{
			PlaySound(fatsoAttackClip);
		}
		else if (((global::System.Enum)zombieName).Equals((object)ZombieType.goliath))
		{
			PlaySound(goliathAttackClip);
		}
		else if (((global::System.Enum)zombieName).Equals((object)ZombieType.sweeper_body) || ((global::System.Enum)zombieName).Equals((object)ZombieType.sweeper_head))
		{
			PlaySound(sweeperAttackClip);
		}
		else if (((global::System.Enum)zombieName).Equals((object)ZombieType.berserker))
		{
			PlaySound(berserkerAttackClip);
		}
	}

	private void OnKilledZombieWithThunder()
	{
		PlaySound(thunderAttackSoundClip);
	}

	private void OnKilledZombieWithFlame()
	{
		PlaySound(flameAttackSoundClip);
	}

	private void OnLeftDash()
	{
		PlaySound(dashSoundClip);
	}

	private void OnRightDash()
	{
		PlaySound(dashSoundClip);
	}

	private void OnJump()
	{
		PlaySound(jumpSoundClip);
	}

	private void OnStomp()
	{
		PlaySound(stompSoundClip);
	}

	private void OnSlide()
	{
		PlaySound(slideSoundClip);
	}

	private void OnThrowShuriken()
	{
		PlaySound(throwShurikenSoundClip);
	}

	private void OnDead()
	{
		PlaySound(deadSoundClip);
	}

	private void PlaySound(AudioClip clipIn)
	{
		MonoSingleton<SoundManager>.instance.PlaySound(clipIn);
	}

	private void OnStartBuff(ScrollElement elementIn)
	{
		if (((global::System.Enum)elementIn).Equals((object)ScrollElement.Gold))
		{
			isGoldBuffOn = true;
		}
	}

	private void OnFinishBuff(ScrollElement elementIn)
	{
		if (((global::System.Enum)elementIn).Equals((object)ScrollElement.Gold))
		{
			isGoldBuffOn = false;
		}
	}

	private void OnShieldStart()
	{
		PlaySound(shieldActivateSoundClip);
	}

	private void OnShieldUsed()
	{
		PlaySound(shieldUsedSoundClip);
	}

	private void OnUseMultiplierBooster()
	{
		PlaySound(multiplierBoosterSoundClip);
	}
}
