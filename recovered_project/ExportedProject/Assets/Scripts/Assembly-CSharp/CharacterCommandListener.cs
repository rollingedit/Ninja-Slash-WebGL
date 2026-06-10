using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterCommandListener : MonoBehaviour
{
	[Serializable]
	public class Command
	{
		[SerializeField]
		public float ignoreTime;

		[SerializeField]
		public string eventToSend;

		[SerializeField]
		public List<string> endEvents;

		[HideInInspector]
		public bool isDisabled;

		[HideInInspector]
		public int bufferedCount;

		[HideInInspector]
		public float leftTime;

		public void FlushReservation()
		{
			isDisabled = false;
			bufferedCount = 0;
			leftTime = 0f;
		}
	}

	public Command jumpCommand;

	public Command slideCommand;

	public Command stompCommand;

	public Command leftDashCommand;

	public Command rightDashCommand;

	private List<Command> commands;

	private CharacterController controller;

	private void Start()
	{
		commands = new List<Command>();
		commands.Add(jumpCommand);
		commands.Add(slideCommand);
		commands.Add(stompCommand);
		commands.Add(leftDashCommand);
		commands.Add(rightDashCommand);
		controller = ((Component)this).GetComponent<CharacterController>();
	}

	private void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		var enumerator = commands.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Command current = enumerator.Current;
				if (current.leftTime > 0f)
				{
					current.leftTime = Mathf.Clamp(current.leftTime - Time.deltaTime, 0f, current.leftTime);
				}
				if (!current.isDisabled && current.bufferedCount > 0)
				{
					current.isDisabled = true;
					current.bufferedCount--;
					current.leftTime = current.ignoreTime;
					((Component)this).BroadcastMessage(current.eventToSend);
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
	}

	private void OnDoCommand(Command command)
	{
		if (command.leftTime == 0f)
		{
			command.bufferedCount++;
		}
	}

	private void OnEndEvent(string eventName)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		var enumerator = commands.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Command current = enumerator.Current;
				if (current.endEvents.Contains(eventName))
				{
					current.isDisabled = false;
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
	}

	private void OnDoJump()
	{
		OnDoCommand(jumpCommand);
	}

	private void OnDoLeftDash()
	{
		OnDoCommand(leftDashCommand);
		rightDashCommand.isDisabled = true;
	}

	private void OnDoRightDash()
	{
		OnDoCommand(rightDashCommand);
		leftDashCommand.isDisabled = true;
	}

	private void OnDoSlide()
	{
		if (controller.isGrounded)
		{
			OnDoCommand(slideCommand);
		}
		else
		{
			OnDoCommand(stompCommand);
		}
	}

	private void OnEndDash()
	{
		OnEndEvent(((MemberInfo)MethodBase.GetCurrentMethod()).Name);
	}

	private void OnEndSlide()
	{
		OnEndEvent(((MemberInfo)MethodBase.GetCurrentMethod()).Name);
	}

	private void OnLand()
	{
		OnEndEvent(((MemberInfo)MethodBase.GetCurrentMethod()).Name);
	}

	private void OnBounce()
	{
		OnEndEvent(((MemberInfo)MethodBase.GetCurrentMethod()).Name);
		jumpCommand.isDisabled = true;
		jumpCommand.leftTime = jumpCommand.ignoreTime;
	}

	private void OnTumble()
	{
		OnEndEvent(((MemberInfo)MethodBase.GetCurrentMethod()).Name);
		slideCommand.isDisabled = true;
		jumpCommand.FlushReservation();
	}

	private void OnEndTumble()
	{
		OnEndEvent(((MemberInfo)MethodBase.GetCurrentMethod()).Name);
	}

	private void OnFalling()
	{
		jumpCommand.isDisabled = true;
	}

	private void Reset()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		var enumerator = commands.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Command current = enumerator.Current;
				current.FlushReservation();
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
	}

	private void OnGameStart()
	{
		Reset();
	}

	private void OnPutObjectIntoPool()
	{
		Reset();
	}
}
