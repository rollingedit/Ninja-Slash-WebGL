using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterEffect : MonoBehaviour
{
	public GameObject jumpEffect;

	public GameObject landEffect;

	public GameObject slidingEffect;

	public List<GameObject> hoguSwordEffects;

	public List<GameObject> hoguHitEffects;

	public List<GameObject> leftAttackEffects;

	public List<GameObject> rightAttackEffects;

	public List<GameObject> slideAttackEffects;

	public List<GameObject> stompAttackEffects;

	public List<GameObject> sweeperAttackEffects;

	public List<GameObject> tumblingAttackEffects;

	public List<GameObject> multiplierBoosterEffects;

	public GameObject itemGetEffect;

	private int hoguAttackIndex;

	private bool isSliding;

	private bool isTumbling;

	private void OnGameStart()
	{
		isSliding = false;
		isTumbling = false;
	}

	private void OnSlide()
	{
		isSliding = true;
		if (Utility.IsGoodPerformance())
		{
			PlayEffectAndAttachParent(slidingEffect);
		}
	}

	private void OnEndSlide()
	{
		isSliding = false;
	}

	private void OnTumble()
	{
		isTumbling = true;
		PlayEffect(landEffect);
	}

	private void OnEndTumble()
	{
		isTumbling = false;
	}

	private void PlayEffect(GameObject effect)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		PoolManager.Spawn(effect, ((Component)this).transform.position);
	}

	private void PlayEffectAndAttachParent(GameObject effect)
	{
		PoolManager.SpawnAndAttachToParent(effect, ((Component)this).gameObject);
	}

	private void PlayEffectAndAttachParent(List<GameObject> effects)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		var enumerator = effects.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				PoolManager.SpawnAndAttachToParent(current, ((Component)this).gameObject);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
	}

	private void OnJump()
	{
		if (Utility.IsGoodPerformance())
		{
			PlayEffect(jumpEffect);
		}
	}

	private void OnLand()
	{
		if (Utility.IsGoodPerformance())
		{
			PlayEffect(landEffect);
		}
	}

	private void OnSlashLeft()
	{
		PlayEffectAndAttachParent(leftAttackEffects);
	}

	private void OnSlashRight()
	{
		PlayEffectAndAttachParent(rightAttackEffects);
	}

	private void OnKilledZombie(ZombieType zName)
	{
		if (((global::System.Enum)zName).Equals((object)ZombieType.zombling))
		{
			if (isSliding)
			{
				PlayEffectAndAttachParent(slideAttackEffects);
				return;
			}
			if (isTumbling)
			{
				PlayEffectAndAttachParent(tumblingAttackEffects);
				return;
			}
			PlayEffectAndAttachParent(hoguSwordEffects[hoguAttackIndex]);
			PlayEffectAndAttachParent(hoguHitEffects[hoguAttackIndex]);
			hoguAttackIndex = (hoguAttackIndex + 1) % hoguSwordEffects.Count;
		}
		else if (((global::System.Enum)zName).Equals((object)ZombieType.goliath))
		{
			PlayEffectAndAttachParent(slideAttackEffects);
		}
		else if (((global::System.Enum)zName).Equals((object)ZombieType.fatso))
		{
			PlayEffectAndAttachParent(stompAttackEffects);
		}
		else if (((global::System.Enum)zName).Equals((object)ZombieType.sweeper_head))
		{
			PlayEffectAndAttachParent(sweeperAttackEffects);
		}
		else if (((global::System.Enum)zName).Equals((object)ZombieType.sweeper_body))
		{
			PlayEffectAndAttachParent(hoguSwordEffects[0]);
		}
	}

	private void OnGetScroll(ScrollElement elementIn)
	{
		PlayEffectAndAttachParent(itemGetEffect);
	}

	private void OnEnterLuckyBox(GameObject box)
	{
		PlayEffectAndAttachParent(itemGetEffect);
		PlayEffectAndAttachParent(hoguSwordEffects[0]);
	}

	private void OnUseMultiplierBooster()
	{
		PlayEffectAndAttachParent(multiplierBoosterEffects);
	}
}
