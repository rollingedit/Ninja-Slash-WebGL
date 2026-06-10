using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterAnimation : MonoBehaviour
{
	public float blendValue = 0.1f;

	private int attackIndex;

	private GameObject parentObject;

	private bool isJumping;

	private AnimationState animState;

	private bool isSliding;

	private bool isTumbling;

	private void Start()
	{
		GetComponent<Animation>()["attack_001"].layer = 1;
		GetComponent<Animation>()["attack_002"].layer = 1;
		GetComponent<Animation>()["attack_003"].layer = 1;
		GetComponent<Animation>()["skill_wind_tagged"].layer = 1;
		GetComponent<Animation>()["sliding_wind_tagged"].layer = 1;
		GetComponent<Animation>()["sliding attack"].layer = 1;
		GetComponent<Animation>()["left"].layer = 1;
		GetComponent<Animation>()["right"].layer = 1;
		GetComponent<Animation>()["jump left"].layer = 1;
		GetComponent<Animation>()["jump right"].layer = 1;
		GetComponent<Animation>()["jump attack"].layer = 1;
	}

	private void OnCreatePlayerModel(GameObject parentObjectIn)
	{
		parentObject = parentObjectIn;
	}

	private void Update()
	{
	}

	private void OnTapToStartButtonClicked()
	{
		PlayAnimation("start jump");
	}

	private void OnGameStart()
	{
		BackToRun();
	}

	private void OnRestart()
	{
		BackToRun();
	}

	private void OnPutObjectIntoPool()
	{
		((MonoBehaviour)this).StopAllCoroutines();
		isJumping = false;
		isSliding = false;
		isTumbling = false;
	}

	private void OnFalling()
	{
		isJumping = true;
		PlayAnimation("falling");
	}

	private void OnStomp()
	{
		PlayAnimation("stomp");
	}

	private void OnJump()
	{
		isJumping = true;
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			PlayAnimation("jump");
		}
		else
		{
			PlayAnimation("jump_002");
		}
	}

	private void OnLand()
	{
		isJumping = false;
		BackToRun();
	}

	private void OnDash(bool isLeft)
	{
		if (isLeft)
		{
			if (!isJumping)
			{
				PlayAnimation("left");
			}
			else
			{
				PlayAnimation("jump left");
			}
		}
		else if (!isJumping)
		{
			PlayAnimation("right");
		}
		else
		{
			PlayAnimation("jump right");
		}
	}

	private void OnLeftDash()
	{
		OnDash(true);
	}

	private void OnRightDash()
	{
		OnDash(false);
	}

	private void OnDoRightBump()
	{
		if (!isJumping)
		{
			PlayAnimation("right");
		}
		else
		{
			PlayAnimation("jump right");
		}
	}

	private void OnDoLeftBump()
	{
		if (!isJumping)
		{
			PlayAnimation("left");
		}
		else
		{
			PlayAnimation("jump left");
		}
	}

	private void OnSlide()
	{
		if (!isSliding && !isTumbling)
		{
			PlayAnimation("sliding");
			((MonoBehaviour)this).StartCoroutine("WaitForSlide");
			isSliding = true;
		}
	}

	private void OnTumble()
	{
		isJumping = false;
		PlayAnimation("tumbling");
		((MonoBehaviour)this).StartCoroutine("WaitForTumble");
		isTumbling = true;
	}

	private void OnDamaged()
	{
		PlayAnimation("damage");
	}

	private void OnKilledZombie(ZombieType zName)
	{
		if (((global::System.Enum)zName).Equals((object)ZombieType.zombling))
		{
			if (isSliding)
			{
				PlayAnimation("sliding attack");
			}
			else if (!isTumbling)
			{
				PlayAnimation(string.Concat((object)"attack_00", (object)(attackIndex + 1)));
				attackIndex = (attackIndex + 1) % 3;
			}
		}
		else if (((global::System.Enum)zName).Equals((object)ZombieType.goliath))
		{
			PlayAnimation("sliding attack");
		}
		else if (((global::System.Enum)zName).Equals((object)ZombieType.fatso))
		{
			isJumping = true;
			PlayAnimation("stomp jump");
		}
		else if (((global::System.Enum)zName).Equals((object)ZombieType.sweeper_head))
		{
			PlayAnimation("jump attack");
		}
		else if (((global::System.Enum)zName).Equals((object)ZombieType.sweeper_body))
		{
			PlayAnimation("attack_001");
		}
		else if (((global::System.Enum)zName).Equals((object)ZombieType.berserker))
		{
			PlayAnimation("attack_001");
		}
	}

	private void OnEnterLuckyBox(GameObject box)
	{
		PlayAnimation("attack_001");
	}

	private void OnSlashLeft()
	{
		PlayAnimation("attack_003");
	}

	private void OnSlashRight()
	{
		PlayAnimation("attack_002");
	}

	private void OnThrowShuriken()
	{
		if (isSliding)
		{
			PlayAnimation("sliding_wind_tagged");
		}
		else if (!isTumbling)
		{
			PlayAnimation("skill_wind_tagged");
		}
	}

	private void PlayAnimation(string animName)
	{
		if (GetComponent<Animation>()[animName].layer == 0)
		{
			if (isSliding)
			{
				((MonoBehaviour)this).StopCoroutine("WaitForSlide");
				parentObject.BroadcastMessage("OnEndSlide");
				isSliding = false;
			}
			else if (isTumbling)
			{
				((MonoBehaviour)this).StopCoroutine("WaitForTumble");
				parentObject.BroadcastMessage("OnEndTumble");
				isTumbling = false;
			}
		}
		GetComponent<Animation>().CrossFade(animName, blendValue, (PlayMode)0);
	}

	private void BackToRun()
	{
		if (!isJumping)
		{
			PlayAnimation("run");
		}
		else
		{
			PlayAnimation("falling");
		}
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator WaitForSlide()
	{
		yield return (object)new WaitForSeconds(GetComponent<Animation>()["sliding"].length);
		parentObject.BroadcastMessage("OnEndSlide");
		isSliding = false;
		BackToRun();
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator WaitForTumble()
	{
		yield return (object)new WaitForSeconds(GetComponent<Animation>()["tumbling"].length);
		parentObject.BroadcastMessage("OnEndTumble");
		isTumbling = false;
		BackToRun();
	}

	public void SendAnimationEvent(string tagName)
	{
		parentObject.BroadcastMessage(tagName, (SendMessageOptions)1);
	}

	private void OnEnterGateDoor(GameObject door)
	{
		PlayAnimation("attack_001");
	}
}
