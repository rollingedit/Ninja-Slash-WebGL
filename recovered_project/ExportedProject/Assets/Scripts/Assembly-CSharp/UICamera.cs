using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/UI/Camera")]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class UICamera : MonoBehaviour
{
	public class MouseOrTouch
	{
		public Vector2 pos;

		public Vector2 delta;

		public Vector2 totalDelta;

		public Camera pressedCam;

		public GameObject current;

		public GameObject pressed;

		public float clickTime;

		public ClickNotification clickNotification = ClickNotification.Always;

		public bool touchBegan = true;

		public bool dragStarted;
	}

	public enum ClickNotification
	{
		None = 0,
		Always = 1,
		BasedOnDelta = 2
	}

	private class Highlighted
	{
		public GameObject go;

		public int counter;
	}

	public bool useMouse = true;

	public bool useTouch = true;

	public bool allowMultiTouch = true;

	public bool useKeyboard = true;

	public bool useController = true;

	public LayerMask eventReceiverMask = -1;

	public bool clipRaycasts = true;

	public float tooltipDelay = 1f;

	public bool stickyTooltip = true;

	public float mouseDragThreshold = 4f;

	public float mouseClickThreshold = 10f;

	public float touchDragThreshold = 40f;

	public float touchClickThreshold = 40f;

	public float rangeDistance = -1f;

	public string scrollAxisName = "Mouse ScrollWheel";

	public string verticalAxisName = "Vertical";

	public string horizontalAxisName = "Horizontal";

	public KeyCode submitKey0 = (KeyCode)13;

	public KeyCode submitKey1 = KeyCode.Space;

	public KeyCode cancelKey0 = (KeyCode)27;

	public KeyCode cancelKey1 = (KeyCode)331;

	public static bool showTooltips = true;

	public static Vector2 lastTouchPosition = Vector2.zero;

	public static RaycastHit lastHit;

	public static UICamera current = null;

	public static Camera currentCamera = null;

	public static int currentTouchID = -1;

	public static MouseOrTouch currentTouch = null;

	public static bool inputHasFocus = false;

	public static GameObject genericEventHandler;

	public static GameObject fallThrough;

	private static List<UICamera> mList = new List<UICamera>();

	private static List<Highlighted> mHighlighted = new List<Highlighted>();

	private static GameObject mSel = null;

	private static MouseOrTouch[] mMouse = new MouseOrTouch[3]
	{
		new MouseOrTouch(),
		new MouseOrTouch(),
		new MouseOrTouch()
	};

	private static GameObject mHover;

	private static MouseOrTouch mController = new MouseOrTouch();

	private static float mNextEvent = 0f;

	private static float mSuppressMouseUntil = 0f;

	private Dictionary<int, MouseOrTouch> mTouches = new Dictionary<int, MouseOrTouch>();

	private GameObject mTooltip;

	private Camera mCam;

	private LayerMask mLayerMask;

	private float mTooltipTime;

	private bool mIsEditor;

	private bool handlesEvents
	{
		get
		{
			return (Object)(object)eventHandler == (Object)(object)this;
		}
	}

	public Camera cachedCamera
	{
		get
		{
			if ((Object)(object)mCam == (Object)null)
			{
				mCam = GetComponent<Camera>();
			}
			return mCam;
		}
	}

	public static GameObject hoveredObject
	{
		get
		{
			return mMouse[0].current;
		}
	}

	public static GameObject selectedObject
	{
		get
		{
			return mSel;
		}
		set
		{
			if (!((Object)(object)mSel != (Object)(object)value))
			{
				return;
			}
			if ((Object)(object)mSel != (Object)null)
			{
				UICamera uICamera = FindCameraForLayer(mSel.layer);
				if ((Object)(object)uICamera != (Object)null)
				{
					current = uICamera;
					currentCamera = uICamera.mCam;
					Notify(mSel, "OnSelect", false);
					if (uICamera.useController || uICamera.useKeyboard)
					{
						Highlight(mSel, false);
					}
					current = null;
				}
			}
			mSel = value;
			if (!((Object)(object)mSel != (Object)null))
			{
				return;
			}
			UICamera uICamera2 = FindCameraForLayer(mSel.layer);
			if ((Object)(object)uICamera2 != (Object)null)
			{
				current = uICamera2;
				currentCamera = uICamera2.mCam;
				if (uICamera2.useController || uICamera2.useKeyboard)
				{
					Highlight(mSel, true);
				}
				Notify(mSel, "OnSelect", true);
				current = null;
			}
		}
	}

	public static Camera mainCamera
	{
		get
		{
			UICamera uICamera = eventHandler;
			return (!((Object)(object)uICamera != (Object)null)) ? null : uICamera.cachedCamera;
		}
	}

	public static UICamera eventHandler
	{
		get
		{
			for (int i = 0; i < mList.Count; i++)
			{
				UICamera uICamera = mList[i];
				if (!((Object)(object)uICamera == (Object)null) && ((Behaviour)uICamera).enabled && NGUITools.GetActive(((Component)uICamera).gameObject))
				{
					return uICamera;
				}
			}
			return null;
		}
	}

	private void OnApplicationQuit()
	{
		mHighlighted.Clear();
	}

	private static int CompareFunc(UICamera a, UICamera b)
	{
		if (a.cachedCamera.depth < b.cachedCamera.depth)
		{
			return 1;
		}
		if (a.cachedCamera.depth > b.cachedCamera.depth)
		{
			return -1;
		}
		return 0;
	}

	private static bool Raycast(Vector3 inPos, ref RaycastHit hit)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < mList.Count; i++)
		{
			UICamera uICamera = mList[i];
			if (!((Behaviour)uICamera).enabled || !NGUITools.GetActive(((Component)uICamera).gameObject))
			{
				continue;
			}
			currentCamera = uICamera.cachedCamera;
			Vector3 val = currentCamera.ScreenToViewportPoint(inPos);
			if (val.x < 0f || val.x > 1f || val.y < 0f || val.y > 1f)
			{
				continue;
			}
			Ray val2 = currentCamera.ScreenPointToRay(inPos);
			int num = currentCamera.cullingMask & (uICamera.eventReceiverMask).value;
			float num2 = ((!(uICamera.rangeDistance > 0f)) ? (currentCamera.farClipPlane - currentCamera.nearClipPlane) : uICamera.rangeDistance);
			if (uICamera.clipRaycasts)
			{
				RaycastHit[] array = Physics.RaycastAll(val2, num2, num);
				if (array.Length > 1)
				{
					global::System.Array.Sort<RaycastHit>(array, (Comparison<RaycastHit>)((RaycastHit r1, RaycastHit r2) => r1.distance.CompareTo(r2.distance)));
					int num3 = 0;
					for (int num4 = array.Length; num3 < num4; num3++)
					{
						if (IsVisible(ref array[num3]))
						{
							hit = array[num3];
							return true;
						}
					}
					return false;
				}
				if (array.Length == 1 && IsVisible(ref array[0]))
				{
					hit = array[0];
					return true;
				}
			}
			if (Physics.Raycast(val2, out hit, num2, num))
			{
				return true;
			}
		}
		return false;
	}

	private static bool IsVisible(ref RaycastHit hit)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		UIPanel uIPanel = NGUITools.FindInParents<UIPanel>(((Component)hit.collider).gameObject);
		if ((Object)(object)uIPanel == (Object)null || uIPanel.clipping == UIDrawCall.Clipping.None || uIPanel.IsVisible(hit.point))
		{
			return true;
		}
		return false;
	}

	public static UICamera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		for (int i = 0; i < mList.Count; i++)
		{
			UICamera uICamera = mList[i];
			Camera val = uICamera.cachedCamera;
			if ((Object)(object)val != (Object)null && (val.cullingMask & num) != 0)
			{
				return uICamera;
			}
		}
		return null;
	}

	private static int GetDirection(KeyCode up, KeyCode down)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (Input.GetKeyDown(up))
		{
			return 1;
		}
		if (Input.GetKeyDown(down))
		{
			return -1;
		}
		return 0;
	}

	private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		if (Input.GetKeyDown(up0) || Input.GetKeyDown(up1))
		{
			return 1;
		}
		if (Input.GetKeyDown(down0) || Input.GetKeyDown(down1))
		{
			return -1;
		}
		return 0;
	}

	private static int GetDirection(string axis)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (mNextEvent < realtimeSinceStartup)
		{
			float axis2 = Input.GetAxis(axis);
			if (axis2 > 0.75f)
			{
				mNextEvent = realtimeSinceStartup + 0.25f;
				return 1;
			}
			if (axis2 < -0.75f)
			{
				mNextEvent = realtimeSinceStartup + 0.25f;
				return -1;
			}
		}
		return 0;
	}

	public static bool IsHighlighted(GameObject go)
	{
		int num = mHighlighted.Count;
		while (num > 0)
		{
			Highlighted highlighted = mHighlighted[--num];
			if ((Object)(object)highlighted.go == (Object)(object)go)
			{
				return true;
			}
		}
		return false;
	}

	private static void Highlight(GameObject go, bool highlighted)
	{
		if (!((Object)(object)go != (Object)null))
		{
			return;
		}
		int num = mHighlighted.Count;
		while (num > 0)
		{
			Highlighted highlighted2 = mHighlighted[--num];
			if (highlighted2 == null || (Object)(object)highlighted2.go == (Object)null)
			{
				mHighlighted.RemoveAt(num);
			}
			else if ((Object)(object)highlighted2.go == (Object)(object)go)
			{
				if (highlighted)
				{
					highlighted2.counter++;
				}
				else if (--highlighted2.counter < 1)
				{
					mHighlighted.Remove(highlighted2);
					Notify(go, "OnHover", false);
				}
				return;
			}
		}
		if (highlighted)
		{
			Highlighted highlighted3 = new Highlighted();
			highlighted3.go = go;
			highlighted3.counter = 1;
			mHighlighted.Add(highlighted3);
			Notify(go, "OnHover", true);
		}
	}

	private static void Notify(GameObject go, string funcName, object obj)
	{
		if ((Object)(object)go != (Object)null)
		{
			go.SendMessage(funcName, obj, (SendMessageOptions)1);
			if ((Object)(object)genericEventHandler != (Object)null && (Object)(object)genericEventHandler != (Object)(object)go)
			{
				genericEventHandler.SendMessage(funcName, obj, (SendMessageOptions)1);
			}
		}
	}

	private MouseOrTouch GetTouch(int id)
	{
		MouseOrTouch mouseOrTouch = default(MouseOrTouch);
		if (!mTouches.TryGetValue(id, out mouseOrTouch))
		{
			mouseOrTouch = new MouseOrTouch();
			mouseOrTouch.touchBegan = true;
			mTouches.Add(id, mouseOrTouch);
		}
		return mouseOrTouch;
	}

	private void RemoveTouch(int id)
	{
		mTouches.Remove(id);
	}

	private void Awake()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Invalid comparison between Unknown and I4
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		if ((int)Application.platform == 11 || (int)Application.platform == 8)
		{
			useMouse = false;
			useTouch = true;
			useKeyboard = false;
			useController = false;
		}
		else if ((int)Application.platform == 9 || (int)Application.platform == 10)
		{
			useMouse = false;
			useTouch = false;
			useKeyboard = false;
			useController = true;
		}
		else if ((int)Application.platform == 7 || (int)Application.platform == 0)
		{
			mIsEditor = true;
		}
		mMouse[0].pos.x = Input.mousePosition.x;
		mMouse[0].pos.y = Input.mousePosition.y;
		lastTouchPosition = mMouse[0].pos;
		mList.Add(this);
		mList.Sort((Comparison<UICamera>)CompareFunc);
		if ((eventReceiverMask).value == -1)
		{
			eventReceiverMask = GetComponent<Camera>().cullingMask;
		}
	}

	private void OnDestroy()
	{
		mList.Remove(this);
	}

	private void FixedUpdate()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (useMouse && Application.isPlaying && handlesEvents)
		{
			GameObject val = ((!Raycast(Input.mousePosition, ref lastHit)) ? fallThrough : ((Component)lastHit.collider).gameObject);
			if ((Object)(object)val == (Object)null)
			{
				val = genericEventHandler;
			}
			for (int i = 0; i < 3; i++)
			{
				mMouse[i].current = val;
			}
		}
	}

	private void Update()
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		if (!Application.isPlaying || !handlesEvents)
		{
			return;
		}
		current = this;
		if (useMouse || (useTouch && mIsEditor))
		{
			ProcessMouse();
		}
		if (useTouch)
		{
			ProcessTouches();
		}
		if (useMouse && (Object)(object)mSel != (Object)null && (((int)cancelKey0 != 0 && Input.GetKeyDown(cancelKey0)) || ((int)cancelKey1 != 0 && Input.GetKeyDown(cancelKey1))))
		{
			selectedObject = null;
		}
		if ((Object)(object)mSel != (Object)null)
		{
			string text = Input.inputString;
			if (useKeyboard && Input.GetKeyDown((KeyCode)127))
			{
				text += "\b";
			}
			if (text.Length > 0)
			{
				if (!stickyTooltip && (Object)(object)mTooltip != (Object)null)
				{
					ShowTooltip(false);
				}
				Notify(mSel, "OnInput", text);
			}
			ProcessOthers();
		}
		else
		{
			inputHasFocus = false;
		}
		if (useMouse && (Object)(object)mHover != (Object)null)
		{
			float axis = Input.GetAxis(scrollAxisName);
			if (axis != 0f)
			{
				Notify(mHover, "OnScroll", axis);
			}
			if (showTooltips && mTooltipTime != 0f && (mTooltipTime < Time.realtimeSinceStartup || Input.GetKey((KeyCode)304) || Input.GetKey((KeyCode)303)))
			{
				mTooltip = mHover;
				ShowTooltip(true);
			}
		}
		current = null;
	}

	private void ProcessMouse()
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		if (Time.realtimeSinceStartup < mSuppressMouseUntil)
		{
			for (int i = 0; i < 3; i++)
			{
				mMouse[i].pressed = null;
				mMouse[i].current = null;
			}
			return;
		}
		bool flag = useMouse && Time.timeScale < 0.9f;
		if (!flag)
		{
			for (int i = 0; i < 3; i++)
			{
				if (Input.GetMouseButton(i) || Input.GetMouseButtonUp(i))
				{
					flag = true;
					break;
				}
			}
		}
		mMouse[0].pos = (Vector2)(Input.mousePosition);
		mMouse[0].delta = mMouse[0].pos - lastTouchPosition;
		bool flag2 = mMouse[0].pos != lastTouchPosition;
		lastTouchPosition = mMouse[0].pos;
		if (flag)
		{
			GameObject val = ((!Raycast(Input.mousePosition, ref lastHit)) ? fallThrough : ((Component)lastHit.collider).gameObject);
			if ((Object)(object)val == (Object)null)
			{
				val = genericEventHandler;
			}
			mMouse[0].current = val;
		}
		for (int j = 1; j < 3; j++)
		{
			mMouse[j].pos = mMouse[0].pos;
			mMouse[j].delta = mMouse[0].delta;
			mMouse[j].current = mMouse[0].current;
		}
		bool flag3 = false;
		for (int k = 0; k < 3; k++)
		{
			if (Input.GetMouseButton(k))
			{
				flag3 = true;
				break;
			}
		}
		if (flag3)
		{
			mTooltipTime = 0f;
		}
		else if (useMouse && flag2 && (!stickyTooltip || (Object)(object)mHover != (Object)(object)mMouse[0].current))
		{
			if (mTooltipTime != 0f)
			{
				mTooltipTime = Time.realtimeSinceStartup + tooltipDelay;
			}
			else if ((Object)(object)mTooltip != (Object)null)
			{
				ShowTooltip(false);
			}
		}
		if (useMouse && !flag3 && (Object)(object)mHover != (Object)null && (Object)(object)mHover != (Object)(object)mMouse[0].current)
		{
			if ((Object)(object)mTooltip != (Object)null)
			{
				ShowTooltip(false);
			}
			Highlight(mHover, false);
			mHover = null;
		}
		if (useMouse)
		{
			for (int l = 0; l < 3; l++)
			{
				bool mouseButtonDown = Input.GetMouseButtonDown(l);
				bool mouseButtonUp = Input.GetMouseButtonUp(l);
				currentTouch = mMouse[l];
				currentTouchID = -1 - l;
				if (mouseButtonDown)
				{
					currentTouch.pressedCam = currentCamera;
				}
				else if ((Object)(object)currentTouch.pressed != (Object)null)
				{
					currentCamera = currentTouch.pressedCam;
				}
				ProcessTouch(mouseButtonDown, mouseButtonUp);
			}
			currentTouch = null;
		}
		if (useMouse && !flag3 && (Object)(object)mHover != (Object)(object)mMouse[0].current)
		{
			mTooltipTime = Time.realtimeSinceStartup + tooltipDelay;
			mHover = mMouse[0].current;
			Highlight(mHover, true);
		}
	}

	private void ProcessTouches()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Invalid comparison between Unknown and I4
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Invalid comparison between Unknown and I4
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			if (allowMultiTouch || touch.fingerId == 0)
			{
				mSuppressMouseUntil = Time.realtimeSinceStartup + 0.5f;
				currentTouchID = ((!allowMultiTouch) ? 1 : touch.fingerId);
				currentTouch = GetTouch(currentTouchID);
				bool flag = (int)touch.phase == 0 || currentTouch.touchBegan;
				bool flag2 = (int)touch.phase == 4 || (int)touch.phase == 3;
				currentTouch.touchBegan = false;
				if (flag)
				{
					currentTouch.delta = Vector2.zero;
				}
				else
				{
					currentTouch.delta = touch.position - currentTouch.pos;
				}
				currentTouch.pos = touch.position;
				currentTouch.current = ((!Raycast((Vector2)(currentTouch.pos), ref lastHit)) ? fallThrough : ((Component)lastHit.collider).gameObject);
				if ((Object)(object)currentTouch.current == (Object)null)
				{
					currentTouch.current = genericEventHandler;
				}
				lastTouchPosition = currentTouch.pos;
				if (flag)
				{
					currentTouch.pressedCam = currentCamera;
				}
				else if ((Object)(object)currentTouch.pressed != (Object)null)
				{
					currentCamera = currentTouch.pressedCam;
				}
				ProcessTouch(flag, flag2);
				if (flag2)
				{
					RemoveTouch(currentTouchID);
				}
				currentTouch = null;
			}
		}
	}

	private void ProcessOthers()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		currentTouchID = -100;
		currentTouch = mController;
		inputHasFocus = (Object)(object)mSel != (Object)null && (Object)(object)mSel.GetComponent<UIInput>() != (Object)null;
		bool flag = ((int)submitKey0 != 0 && Input.GetKeyDown(submitKey0)) || ((int)submitKey1 != 0 && Input.GetKeyDown(submitKey1));
		bool flag2 = ((int)submitKey0 != 0 && Input.GetKeyUp(submitKey0)) || ((int)submitKey1 != 0 && Input.GetKeyUp(submitKey1));
		if (flag || flag2)
		{
			currentTouch.current = mSel;
			ProcessTouch(flag, flag2);
		}
		int num = 0;
		int num2 = 0;
		if (useKeyboard)
		{
			if (inputHasFocus)
			{
				num += GetDirection((KeyCode)273, (KeyCode)274);
				num2 += GetDirection((KeyCode)275, (KeyCode)276);
			}
			else
			{
				num += GetDirection((KeyCode)119, (KeyCode)273, (KeyCode)115, (KeyCode)274);
				num2 += GetDirection((KeyCode)100, (KeyCode)275, (KeyCode)97, (KeyCode)276);
			}
		}
		if (useController)
		{
			if (!string.IsNullOrEmpty(verticalAxisName))
			{
				num += GetDirection(verticalAxisName);
			}
			if (!string.IsNullOrEmpty(horizontalAxisName))
			{
				num2 += GetDirection(horizontalAxisName);
			}
		}
		if (num != 0)
		{
			Notify(mSel, "OnKey", (object)(KeyCode)((num <= 0) ? 274 : 273));
		}
		if (num2 != 0)
		{
			Notify(mSel, "OnKey", (object)(KeyCode)((num2 <= 0) ? 276 : 275));
		}
		if (useKeyboard && Input.GetKeyDown((KeyCode)9))
		{
			Notify(mSel, "OnKey", (object)(KeyCode)9);
		}
		if ((int)cancelKey0 != 0 && Input.GetKeyDown(cancelKey0))
		{
			Notify(mSel, "OnKey", (object)(KeyCode)27);
		}
		if ((int)cancelKey1 != 0 && Input.GetKeyDown(cancelKey1))
		{
			Notify(mSel, "OnKey", (object)(KeyCode)27);
		}
		currentTouch = null;
	}

	private void ProcessTouch(bool pressed, bool unpressed)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		bool flag = currentTouch == mMouse[0];
		float num = ((!flag) ? touchDragThreshold : mouseDragThreshold);
		float num2 = ((!flag) ? Mathf.Max(touchClickThreshold, (float)Screen.height * 0.1f) : mouseClickThreshold);
		if (pressed)
		{
			if ((Object)(object)mTooltip != (Object)null)
			{
				ShowTooltip(false);
			}
			currentTouch.pressed = currentTouch.current;
			currentTouch.clickNotification = ClickNotification.Always;
			currentTouch.totalDelta = Vector2.zero;
			currentTouch.dragStarted = false;
			Notify(currentTouch.pressed, "OnPress", true);
			if ((Object)(object)currentTouch.pressed != (Object)(object)mSel)
			{
				if ((Object)(object)mTooltip != (Object)null)
				{
					ShowTooltip(false);
				}
				selectedObject = null;
			}
		}
		else if ((Object)(object)currentTouch.pressed != (Object)null)
		{
			float magnitude = currentTouch.delta.magnitude;
			if (magnitude != 0f)
			{
				MouseOrTouch mouseOrTouch = currentTouch;
				mouseOrTouch.totalDelta += currentTouch.delta;
				magnitude = currentTouch.totalDelta.magnitude;
				if (!currentTouch.dragStarted && num < magnitude)
				{
					currentTouch.dragStarted = true;
					currentTouch.delta = currentTouch.totalDelta;
				}
				if (currentTouch.dragStarted)
				{
					if ((Object)(object)mTooltip != (Object)null)
					{
						ShowTooltip(false);
					}
					bool flag2 = currentTouch.clickNotification == ClickNotification.None;
					Notify(currentTouch.pressed, "OnDrag", currentTouch.delta);
					if (flag2)
					{
						currentTouch.clickNotification = ClickNotification.None;
					}
					else if (currentTouch.clickNotification == ClickNotification.BasedOnDelta && num2 < magnitude)
					{
						currentTouch.clickNotification = ClickNotification.None;
					}
				}
			}
		}
		if (!unpressed)
		{
			return;
		}
		if ((Object)(object)mTooltip != (Object)null)
		{
			ShowTooltip(false);
		}
		if ((Object)(object)currentTouch.pressed != (Object)null)
		{
			Notify(currentTouch.pressed, "OnPress", false);
			if (useMouse && (Object)(object)currentTouch.pressed == (Object)(object)mHover)
			{
				Notify(currentTouch.pressed, "OnHover", true);
			}
			if ((Object)(object)currentTouch.pressed == (Object)(object)currentTouch.current || (currentTouch.clickNotification != ClickNotification.None && currentTouch.totalDelta.magnitude < num))
			{
				if ((Object)(object)currentTouch.pressed != (Object)(object)mSel)
				{
					mSel = currentTouch.pressed;
					Notify(currentTouch.pressed, "OnSelect", true);
				}
				else
				{
					mSel = currentTouch.pressed;
				}
				if (currentTouch.clickNotification != ClickNotification.None)
				{
					float realtimeSinceStartup = Time.realtimeSinceStartup;
					Notify(currentTouch.pressed, "OnClick", null);
					if (currentTouch.clickTime + 0.25f > realtimeSinceStartup)
					{
						Notify(currentTouch.pressed, "OnDoubleClick", null);
					}
					currentTouch.clickTime = realtimeSinceStartup;
				}
			}
			else
			{
				Notify(currentTouch.current, "OnDrop", currentTouch.pressed);
			}
		}
		currentTouch.dragStarted = false;
		currentTouch.pressed = null;
	}

	public void ShowTooltip(bool val)
	{
		mTooltipTime = 0f;
		Notify(mTooltip, "OnTooltip", val);
		if (!val)
		{
			mTooltip = null;
		}
	}
}
