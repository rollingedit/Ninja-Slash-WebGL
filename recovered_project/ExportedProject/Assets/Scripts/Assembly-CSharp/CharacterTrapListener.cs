using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterTrapListener : MonoBehaviour
{
	private CharacterController controller;

	private bool isSliding;

	private bool isTumbling;

	private bool isDashing;

	private bool isDashLeft;

	private bool isThunderBuffOn;

	public GameObject deadEffect;

	private bool isShieldOn;

	[HideInInspector]
	public bool isBlockTriggerCheck = true;

	private void Start()
	{
		Component component = ((Component)this).GetComponent(typeof(CharacterController));
		controller = (CharacterController)(object)((component is CharacterController) ? component : null);
	}

	private void SendDeadEvent(bool isDeadByFall)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		if (isShieldOn)
		{
			isBlockTriggerCheck = false;
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnShieldUsedEvent());
			return;
		}
		if (!isDeadByFall)
		{
			PoolManager.Spawn(deadEffect, ((Component)this).transform.position);
		}
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnDeadEvent());
	}

	private void OnEndDash()
	{
		isDashing = false;
	}

	private void OnEndSlide()
	{
		isSliding = false;
	}

	private void OnEndTumble()
	{
		isTumbling = false;
	}

	private void OnEnterGateDoor(GameObject door)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		door.SendMessage("OnCrashedByPlayer", (object)controller.velocity);
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnEnterGateDoorEvent());
		MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.crashGate);
	}

	private void OnEnterTrapWall()
	{
		SendDeadEvent(false);
	}

	private void OnDead()
	{
		isSliding = false;
		isTumbling = false;
	}

	private void OnPutObjectIntoPool()
	{
		isSliding = false;
		isTumbling = false;
	}

	private void OnJump()
	{
		isSliding = false;
		isTumbling = false;
	}

	private void OnDash(bool isLeft)
	{
		isSliding = false;
		isTumbling = false;
		isDashing = true;
		isDashLeft = isLeft;
	}

	private void OnLeftDash()
	{
		OnDash(true);
	}

	private void OnRightDash()
	{
		OnDash(false);
	}

	private void OnSlide()
	{
		if (!isSliding && !isTumbling)
		{
			isSliding = true;
		}
	}

	private void OnTumble()
	{
		if (!isTumbling)
		{
			isTumbling = true;
		}
	}

	private void OnEnterZombie(ZombieData zData)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		if (isThunderBuffOn)
		{
			((Component)this).BroadcastMessage("OnKilledZombie", (object)zData.zombieType);
			zData.zombieObject.SendMessage("OnCrashedByPlayer", (object)controller.velocity, (SendMessageOptions)1);
			return;
		}
		bool flag = true;
		if (((global::System.Enum)zData.zombieType).Equals((object)ZombieType.zombling))
		{
			flag = false;
		}
		else if (((global::System.Enum)zData.zombieType).Equals((object)ZombieType.lankie))
		{
			flag = false;
			if (isDashLeft)
			{
				((Component)this).BroadcastMessage("OnSlashLeft");
			}
			else
			{
				((Component)this).BroadcastMessage("OnSlashRight");
			}
		}
		else if (((global::System.Enum)zData.zombieType).Equals((object)ZombieType.goliath))
		{
			flag = ((!isSliding && !isTumbling) ? true : false);
		}
		else if (((global::System.Enum)zData.zombieType).Equals((object)ZombieType.fatso))
		{
			flag = false;
		}
		else if (((global::System.Enum)zData.zombieType).Equals((object)ZombieType.sweeper_head))
		{
			flag = false;
		}
		else if (((global::System.Enum)zData.zombieType).Equals((object)ZombieType.sweeper_body))
		{
			flag = true;
		}
		else if (((global::System.Enum)zData.zombieType).Equals((object)ZombieType.berserker))
		{
			flag = true;
		}
		if (!flag)
		{
			((Component)this).BroadcastMessage("OnKilledZombie", (object)zData.zombieType);
			zData.zombieObject.SendMessage("OnCrashedByPlayer", (object)controller.velocity, (SendMessageOptions)1);
		}
		else
		{
			SendDeadEvent(false);
		}
	}

	private void OnEnterBamboo(GameObject bamboo)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		if (isDashing)
		{
			MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.slashProp);
			if (isDashLeft)
			{
				((Component)this).BroadcastMessage("OnSlashLeft");
			}
			else
			{
				((Component)this).BroadcastMessage("OnSlashRight");
			}
			bamboo.SendMessage("OnCrashedByPlayer", (object)controller.velocity);
		}
	}

	private void OnEnterLuckyBox(GameObject box)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		box.SendMessage("OnCrashedByPlayer", (object)controller.velocity);
	}

	private void OnEnterDadamiDoor(GameObject door)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		door.SendMessage("OnCrashedByPlayer", (object)controller.velocity);
		SendDeadEvent(false);
	}

	private void OnStartBuff(ScrollElement elementIn)
	{
		if (((global::System.Enum)elementIn).Equals((object)ScrollElement.Thunder))
		{
			isThunderBuffOn = true;
		}
	}

	private void OnFinishBuff(ScrollElement elementIn)
	{
		if (((global::System.Enum)elementIn).Equals((object)ScrollElement.Thunder))
		{
			isThunderBuffOn = false;
		}
	}

	private void OnShieldStart()
	{
		isShieldOn = true;
	}

	private void OnShieldFinish()
	{
		isShieldOn = false;
	}

	private void OnShieldUsed()
	{
		isBlockTriggerCheck = true;
	}
}
