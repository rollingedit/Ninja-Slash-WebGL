using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class GUIRoot : MonoBehaviour
{
	[Serializable]
	public class MessageWindowAndEvent
	{
		[SerializeField]
		public GameObject messageWindow;

		[SerializeField]
		public string eventName;
	}

	public class MessageWindowQueueElement
	{
		public MessageWindowAndEvent data;

		public object parameter;

		public MessageWindowQueueElement(MessageWindowAndEvent dataIn, object paramIn)
		{
			data = dataIn;
			parameter = paramIn;
		}
	}

	public GameObject mainPanel;

	public MessageWindowAndEvent missionWindow;

	public MessageWindowAndEvent nextMissionWindow;

	public MessageWindowAndEvent rewardWindow;

	public MessageWindowAndEvent comboBonusWindow;

	public MessageWindowAndEvent ninjaUnlockWindow;

	public MessageWindowAndEvent coinPurchaseSuccessWindow;

	public GameObject pauseWindowPrefab;

	public GameObject optionWindowPrefab;

	public GameObject resumePanelPrefab;

	public float messageWindowShowTime;

	public GameObject downloadWindow;

	public GameObject coinPuchaseYesNoWindow;

	public GameObject notEnoughCoinWindow;

	private Queue<MessageWindowQueueElement> messageWindowQueue;

	private GameObject currentMessageWindow;

	private float leftTime;

	private float realTime;

	private bool isGameStarted;

	private void Start()
	{
		messageWindowQueue = new Queue<MessageWindowQueueElement>();
		currentMessageWindow = null;
		leftTime = 0f;
		isGameStarted = false;
	}

	private void Update()
	{
		if (leftTime > 0f)
		{
			float num = Time.realtimeSinceStartup - realTime;
			leftTime = Mathf.Clamp(leftTime - num, 0f, leftTime);
			if (leftTime == 0f)
			{
				currentMessageWindow.SendMessage("OnShowTimeFinished");
				currentMessageWindow = null;
			}
		}
		else if (messageWindowQueue.Count != 0)
		{
			MessageWindowQueueElement messageWindowQueueElement = messageWindowQueue.Dequeue();
			currentMessageWindow = NGUIUtility.AddChild(mainPanel, messageWindowQueueElement.data.messageWindow);
			if (messageWindowQueueElement.data.eventName != null && messageWindowQueueElement.data.eventName.Length > 0)
			{
				currentMessageWindow.SendMessage(messageWindowQueueElement.data.eventName, messageWindowQueueElement.parameter);
			}
			leftTime = messageWindowShowTime;
		}
		realTime = Time.realtimeSinceStartup;
	}

	private void OnGameStart()
	{
		isGameStarted = true;
	}

	private void OnGameOver(GameOverResult result)
	{
		isGameStarted = false;
	}

	private void OnHome()
	{
		isGameStarted = false;
	}

	private void OnMissionComplete(Mission missionIn)
	{
		messageWindowQueue.Enqueue(new MessageWindowQueueElement(missionWindow, missionIn));
		messageWindowQueue.Enqueue(new MessageWindowQueueElement(nextMissionWindow, MonoSingleton<MissionManager>.instance.GetCurrentMission()));
	}

	private void OnGetLuckyBox(Reward itemIn)
	{
		messageWindowQueue.Enqueue(new MessageWindowQueueElement(rewardWindow, itemIn));
	}

	private void OnBuyLuckyBox(Reward itemIn)
	{
		messageWindowQueue.Enqueue(new MessageWindowQueueElement(rewardWindow, itemIn));
	}

	private void OnComboBonus(ComboBonus bonusIn)
	{
		messageWindowQueue.Enqueue(new MessageWindowQueueElement(comboBonusWindow, bonusIn));
	}

	private void OnNinjaUnlocked(Ninja ninjaIn)
	{
		messageWindowQueue.Enqueue(new MessageWindowQueueElement(ninjaUnlockWindow, ninjaIn));
	}

	private void OnCoinPurchaseSuccess()
	{
		messageWindowQueue.Enqueue(new MessageWindowQueueElement(coinPurchaseSuccessWindow, null));
	}

	private void OnPause()
	{
		NGUIUtility.AddChild(mainPanel, pauseWindowPrefab);
	}

	private void OnEnterOption()
	{
		GameObject val = NGUIUtility.AddChild(mainPanel, optionWindowPrefab);
		val.SendMessage("OnOptionWindowOpened", (object)isGameStarted);
	}

	private void OnAttachTexture(AttachTextureData dataIn)
	{
		GameObject go = NGUIUtility.AddChild(mainPanel, dataIn.texture);
		((MonoBehaviour)this).StartCoroutine(WaitAndDestroy(go, dataIn.time));
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator WaitAndDestroy(GameObject go, float second)
	{
		yield return (object)new WaitForSeconds(second);
		NGUIUtility.Destroy(go);
	}

	private void OnDoResume()
	{
		NGUIUtility.AddChild(mainPanel, resumePanelPrefab);
	}

	private void OnStartDownloading()
	{
		downloadWindow.SetActive(true);
	}

	private void OnEndDownloading()
	{
		downloadWindow.SetActive(false);
	}

	private void OnPurchaseFail()
	{
		if (Utility.IsMobile())
		{
			coinPuchaseYesNoWindow.SetActive(true);
		}
		else
		{
			notEnoughCoinWindow.SetActive(true);
		}
	}
}
