using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class iTween : MonoBehaviour
{
	public enum EaseType
	{
		easeInQuad = 0,
		easeOutQuad = 1,
		easeInOutQuad = 2,
		easeInCubic = 3,
		easeOutCubic = 4,
		easeInOutCubic = 5,
		easeInQuart = 6,
		easeOutQuart = 7,
		easeInOutQuart = 8,
		easeInQuint = 9,
		easeOutQuint = 10,
		easeInOutQuint = 11,
		easeInSine = 12,
		easeOutSine = 13,
		easeInOutSine = 14,
		easeInExpo = 15,
		easeOutExpo = 16,
		easeInOutExpo = 17,
		easeInCirc = 18,
		easeOutCirc = 19,
		easeInOutCirc = 20,
		linear = 21,
		spring = 22,
		easeInBounce = 23,
		easeOutBounce = 24,
		easeInOutBounce = 25,
		easeInBack = 26,
		easeOutBack = 27,
		easeInOutBack = 28,
		easeInElastic = 29,
		easeOutElastic = 30,
		easeInOutElastic = 31,
		punch = 32
	}

	public enum LoopType
	{
		none = 0,
		loop = 1,
		pingPong = 2
	}

	public enum NamedValueColor
	{
		_Color = 0,
		_SpecColor = 1,
		_Emission = 2,
		_ReflectColor = 3
	}

	public static class Defaults
	{
		public static float time = 1f;

		public static float delay = 0f;

		public static NamedValueColor namedColorValue = NamedValueColor._Color;

		public static LoopType loopType = LoopType.none;

		public static EaseType easeType = EaseType.easeOutExpo;

		public static float lookSpeed = 3f;

		public static bool isLocal = false;

		public static Space space = (Space)1;

		public static bool orientToPath = false;

		public static Color color = Color.white;

		public static float updateTimePercentage = 0.05f;

		public static float updateTime = 1f * updateTimePercentage;

		public static int cameraFadeDepth = 999999;

		public static float lookAhead = 0.05f;

		public static bool useRealTime = false;

		public static Vector3 up = Vector3.up;
	}

	private class CRSpline
	{
		public Vector3[] pts;

		public CRSpline(params Vector3[] pts)
		{
			this.pts = (Vector3[])(object)new Vector3[pts.Length];
			global::System.Array.Copy((global::System.Array)pts, (global::System.Array)this.pts, pts.Length);
		}

		public Vector3 Interp(float t)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			int num = pts.Length - 3;
			int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
			float num3 = t * (float)num - (float)num2;
			Vector3 val = pts[num2];
			Vector3 val2 = pts[num2 + 1];
			Vector3 val3 = pts[num2 + 2];
			Vector3 val4 = pts[num2 + 3];
			return 0.5f * ((-val + 3f * val2 - 3f * val3 + val4) * (num3 * num3 * num3) + (2f * val - 5f * val2 + 4f * val3 - val4) * (num3 * num3) + (-val + val3) * num3 + 2f * val2);
		}
	}

	private delegate float EasingFunction(float start, float end, float value);

	private delegate void ApplyTween();

	public static ArrayList tweens = new ArrayList();

	private static GameObject cameraFade;

	public string id;

	public string type;

	public string method;

	public EaseType easeType;

	public float time;

	public float delay;

	public LoopType loopType;

	public bool isRunning;

	public bool isPaused;

	public string _name;

	private float runningTime;

	private float percentage;

	private float delayStarted;

	private bool kinematic;

	private bool isLocal;

	private bool loop;

	private bool reverse;

	private bool wasPaused;

	private bool physics;

	private Hashtable tweenArguments;

	private Space space;

	private EasingFunction ease;

	private ApplyTween apply;

	private AudioSource audioSource;

	private Vector3[] vector3s;

	private Vector2[] vector2s;

	private Color[,] colors;

	private float[] floats;

	private Rect[] rects;

	private CRSpline path;

	private Vector3 preUpdate;

	private Vector3 postUpdate;

	private NamedValueColor namedcolorvalue;

	private float lastRealTime;

	private bool useRealTime;

	public static void Init(GameObject target)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		MoveBy(target, Vector3.zero, 0f);
	}

	public static void CameraFadeFrom(float amount, float time)
	{
		if (((Object)(object)cameraFade != (Object)null))
		{
			CameraFadeFrom(Hash("amount", amount, "time", time));
		}
		else
		{
			Debug.LogError((object)"iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static void CameraFadeFrom(Hashtable args)
	{
		if (((Object)(object)cameraFade != (Object)null))
		{
			ColorFrom(cameraFade, args);
		}
		else
		{
			Debug.LogError((object)"iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static void CameraFadeTo(float amount, float time)
	{
		if (((Object)(object)cameraFade != (Object)null))
		{
			CameraFadeTo(Hash("amount", amount, "time", time));
		}
		else
		{
			Debug.LogError((object)"iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static void CameraFadeTo(Hashtable args)
	{
		if (((Object)(object)cameraFade != (Object)null))
		{
			ColorTo(cameraFade, args);
		}
		else
		{
			Debug.LogError((object)"iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static void ValueTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (!args.Contains((object)"onupdate") || !args.Contains((object)"from") || !args.Contains((object)"to"))
		{
			Debug.LogError((object)"iTween Error: ValueTo() requires an 'onupdate' callback function and a 'from' and 'to' property.  The supplied 'onupdate' callback must accept a single argument that is the same type as the supplied 'from' and 'to' properties!");
			return;
		}
		args[(object)"type"] = "value";
		if (args[(object)"from"].GetType() == typeof(Vector2))
		{
			args[(object)"method"] = "vector2";
		}
		else if (args[(object)"from"].GetType() == typeof(Vector3))
		{
			args[(object)"method"] = "vector3";
		}
		else if (args[(object)"from"].GetType() == typeof(Rect))
		{
			args[(object)"method"] = "rect";
		}
		else if (args[(object)"from"].GetType() == typeof(float))
		{
			args[(object)"method"] = "float";
		}
		else
		{
			if (args[(object)"from"].GetType() != typeof(Color))
			{
				Debug.LogError((object)"iTween Error: ValueTo() only works with interpolating Vector3s, Vector2s, floats, ints, Rects and Colors!");
				return;
			}
			args[(object)"method"] = "color";
		}
		if (!args.Contains((object)"easetype"))
		{
			args.Add((object)"easetype", (object)EaseType.linear);
		}
		Launch(target, args);
	}

	public static void FadeFrom(GameObject target, float alpha, float time)
	{
		FadeFrom(target, Hash("alpha", alpha, "time", time));
	}

	public static void FadeFrom(GameObject target, Hashtable args)
	{
		ColorFrom(target, args);
	}

	public static void FadeTo(GameObject target, float alpha, float time)
	{
		FadeTo(target, Hash("alpha", alpha, "time", time));
	}

	public static void FadeTo(GameObject target, Hashtable args)
	{
		ColorTo(target, args);
	}

	public static void ColorFrom(GameObject target, Color color, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ColorFrom(target, Hash("color", color, "time", time));
	}

	public static void ColorFrom(GameObject target, Hashtable args)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		Color color = default(Color);
		Color val = default(Color);
		args = CleanArgs(args);
		if (!args.Contains((object)"includechildren") || (bool)args[(object)"includechildren"])
		{
			foreach (Transform item in target.transform)
			{
				Transform val2 = item;
				Hashtable val3 = (Hashtable)args.Clone();
				val3[(object)"ischild"] = true;
				ColorFrom(((Component)val2).gameObject, val3);
			}
		}
		if (!args.Contains((object)"easetype"))
		{
			args.Add((object)"easetype", (object)EaseType.linear);
		}
		if (((Object)(object)target.GetComponent<GUITexture>() != (Object)null))
		{
			val = (color = target.GetComponent<GUITexture>().color);
		}
		else if (((Object)(object)target.GetComponent<GUIText>() != (Object)null))
		{
			val = (color = target.GetComponent<GUIText>().material.color);
		}
		else if (((Object)(object)target.GetComponent<Renderer>() != (Object)null))
		{
			val = (color = target.GetComponent<Renderer>().material.color);
		}
		else if (((Object)(object)target.GetComponent<Light>() != (Object)null))
		{
			val = (color = target.GetComponent<Light>().color);
		}
		if (args.Contains((object)"color"))
		{
			color = (Color)args[(object)"color"];
		}
		else
		{
			if (args.Contains((object)"r"))
			{
				color.r = (float)args[(object)"r"];
			}
			if (args.Contains((object)"g"))
			{
				color.g = (float)args[(object)"g"];
			}
			if (args.Contains((object)"b"))
			{
				color.b = (float)args[(object)"b"];
			}
			if (args.Contains((object)"a"))
			{
				color.a = (float)args[(object)"a"];
			}
		}
		if (args.Contains((object)"amount"))
		{
			color.a = (float)args[(object)"amount"];
			args.Remove((object)"amount");
		}
		else if (args.Contains((object)"alpha"))
		{
			color.a = (float)args[(object)"alpha"];
			args.Remove((object)"alpha");
		}
		if (((Object)(object)target.GetComponent<GUITexture>() != (Object)null))
		{
			target.GetComponent<GUITexture>().color = color;
		}
		else if (((Object)(object)target.GetComponent<GUIText>() != (Object)null))
		{
			target.GetComponent<GUIText>().material.color = color;
		}
		else if (((Object)(object)target.GetComponent<Renderer>() != (Object)null))
		{
			target.GetComponent<Renderer>().material.color = color;
		}
		else if (((Object)(object)target.GetComponent<Light>() != (Object)null))
		{
			target.GetComponent<Light>().color = color;
		}
		args[(object)"color"] = val;
		args[(object)"type"] = "color";
		args[(object)"method"] = "to";
		Launch(target, args);
	}

	public static void ColorTo(GameObject target, Color color, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ColorTo(target, Hash("color", color, "time", time));
	}

	public static void ColorTo(GameObject target, Hashtable args)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		args = CleanArgs(args);
		if (!args.Contains((object)"includechildren") || (bool)args[(object)"includechildren"])
		{
			foreach (Transform item in target.transform)
			{
				Transform val = item;
				Hashtable val2 = (Hashtable)args.Clone();
				val2[(object)"ischild"] = true;
				ColorTo(((Component)val).gameObject, val2);
			}
		}
		if (!args.Contains((object)"easetype"))
		{
			args.Add((object)"easetype", (object)EaseType.linear);
		}
		args[(object)"type"] = "color";
		args[(object)"method"] = "to";
		Launch(target, args);
	}

	public static void AudioFrom(GameObject target, float volume, float pitch, float time)
	{
		AudioFrom(target, Hash("volume", volume, "pitch", pitch, "time", time));
	}

	public static void AudioFrom(GameObject target, Hashtable args)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		args = CleanArgs(args);
		AudioSource val;
		if (args.Contains((object)"audiosource"))
		{
			val = (AudioSource)args[(object)"audiosource"];
		}
		else
		{
			if (((Object)(object)target.GetComponent<AudioSource>() == (Object)null))
			{
				Debug.LogError((object)"iTween Error: AudioFrom requires an AudioSource.");
				return;
			}
			val = target.GetComponent<AudioSource>();
		}
		Vector2 val2 = default(Vector2);
		Vector2 val3 = default(Vector2);
		val2.x = (val3.x = val.volume);
		val2.y = (val3.y = val.pitch);
		if (args.Contains((object)"volume"))
		{
			val3.x = (float)args[(object)"volume"];
		}
		if (args.Contains((object)"pitch"))
		{
			val3.y = (float)args[(object)"pitch"];
		}
		val.volume = val3.x;
		val.pitch = val3.y;
		args[(object)"volume"] = val2.x;
		args[(object)"pitch"] = val2.y;
		if (!args.Contains((object)"easetype"))
		{
			args.Add((object)"easetype", (object)EaseType.linear);
		}
		args[(object)"type"] = "audio";
		args[(object)"method"] = "to";
		Launch(target, args);
	}

	public static void AudioTo(GameObject target, float volume, float pitch, float time)
	{
		AudioTo(target, Hash("volume", volume, "pitch", pitch, "time", time));
	}

	public static void AudioTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (!args.Contains((object)"easetype"))
		{
			args.Add((object)"easetype", (object)EaseType.linear);
		}
		args[(object)"type"] = "audio";
		args[(object)"method"] = "to";
		Launch(target, args);
	}

	public static void Stab(GameObject target, AudioClip audioclip, float delay)
	{
		Stab(target, Hash("audioclip", audioclip, "delay", delay));
	}

	public static void Stab(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args[(object)"type"] = "stab";
		Launch(target, args);
	}

	public static void LookFrom(GameObject target, Vector3 looktarget, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		LookFrom(target, Hash("looktarget", looktarget, "time", time));
	}

	public static void LookFrom(GameObject target, Hashtable args)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		Vector3 eulerAngles = target.transform.eulerAngles;
		if (args[(object)"looktarget"].GetType() == typeof(Transform))
		{
			Transform transform = target.transform;
			Transform val = (Transform)args[(object)"looktarget"];
			Vector3? val2 = (Vector3?)args[(object)"up"];
			transform.LookAt(val, (!val2.HasValue) ? Defaults.up : val2.Value);
		}
		else if (args[(object)"looktarget"].GetType() == typeof(Vector3))
		{
			Transform transform2 = target.transform;
			Vector3 val3 = (Vector3)args[(object)"looktarget"];
			Vector3? val4 = (Vector3?)args[(object)"up"];
			transform2.LookAt(val3, (!val4.HasValue) ? Defaults.up : val4.Value);
		}
		if (args.Contains((object)"axis"))
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			switch ((string)args[(object)"axis"])
			{
			case "x":
				eulerAngles2.y = eulerAngles.y;
				eulerAngles2.z = eulerAngles.z;
				break;
			case "y":
				eulerAngles2.x = eulerAngles.x;
				eulerAngles2.z = eulerAngles.z;
				break;
			case "z":
				eulerAngles2.x = eulerAngles.x;
				eulerAngles2.y = eulerAngles.y;
				break;
			}
			target.transform.eulerAngles = eulerAngles2;
		}
		args[(object)"rotation"] = eulerAngles;
		args[(object)"type"] = "rotate";
		args[(object)"method"] = "to";
		Launch(target, args);
	}

	public static void LookTo(GameObject target, Vector3 looktarget, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		LookTo(target, Hash("looktarget", looktarget, "time", time));
	}

	public static void LookTo(GameObject target, Hashtable args)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		if (args.Contains((object)"looktarget") && args[(object)"looktarget"].GetType() == typeof(Transform))
		{
			Transform val = (Transform)args[(object)"looktarget"];
			args[(object)"position"] = (object)new Vector3(val.position.x, val.position.y, val.position.z);
			args[(object)"rotation"] = (object)new Vector3(val.eulerAngles.x, val.eulerAngles.y, val.eulerAngles.z);
		}
		args[(object)"type"] = "look";
		args[(object)"method"] = "to";
		Launch(target, args);
	}

	public static void MoveTo(GameObject target, Vector3 position, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		MoveTo(target, Hash("position", position, "time", time));
	}

	public static void MoveTo(GameObject target, Hashtable args)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		if (args.Contains((object)"position") && args[(object)"position"].GetType() == typeof(Transform))
		{
			Transform val = (Transform)args[(object)"position"];
			args[(object)"position"] = (object)new Vector3(val.position.x, val.position.y, val.position.z);
			args[(object)"rotation"] = (object)new Vector3(val.eulerAngles.x, val.eulerAngles.y, val.eulerAngles.z);
			args[(object)"scale"] = (object)new Vector3(val.localScale.x, val.localScale.y, val.localScale.z);
		}
		args[(object)"type"] = "move";
		args[(object)"method"] = "to";
		Launch(target, args);
	}

	public static void MoveFrom(GameObject target, Vector3 position, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		MoveFrom(target, Hash("position", position, "time", time));
	}

	public static void MoveFrom(GameObject target, Hashtable args)
	{
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		bool flag = ((!args.Contains((object)"islocal")) ? Defaults.isLocal : ((bool)args[(object)"islocal"]));
		if (args.Contains((object)"path"))
		{
			Vector3[] array2;
			if (args[(object)"path"].GetType() == typeof(Vector3[]))
			{
				Vector3[] array = (Vector3[])args[(object)"path"];
				array2 = (Vector3[])(object)new Vector3[array.Length];
				global::System.Array.Copy((global::System.Array)array, (global::System.Array)array2, array.Length);
			}
			else
			{
				Transform[] array3 = (Transform[])args[(object)"path"];
				array2 = (Vector3[])(object)new Vector3[array3.Length];
				for (int i = 0; i < array3.Length; i++)
				{
					array2[i] = array3[i].position;
				}
			}
			if (array2[array2.Length - 1] != target.transform.position)
			{
				Vector3[] array4 = (Vector3[])(object)new Vector3[array2.Length + 1];
				global::System.Array.Copy((global::System.Array)array2, (global::System.Array)array4, array2.Length);
				if (flag)
				{
					array4[array4.Length - 1] = target.transform.localPosition;
					target.transform.localPosition = array4[0];
				}
				else
				{
					array4[array4.Length - 1] = target.transform.position;
					target.transform.position = array4[0];
				}
				args[(object)"path"] = array4;
			}
			else
			{
				if (flag)
				{
					target.transform.localPosition = array2[0];
				}
				else
				{
					target.transform.position = array2[0];
				}
				args[(object)"path"] = array2;
			}
		}
		else
		{
			Vector3 val2;
			Vector3 val = ((!flag) ? (val2 = target.transform.position) : (val2 = target.transform.localPosition));
			if (args.Contains((object)"position"))
			{
				if (args[(object)"position"].GetType() == typeof(Transform))
				{
					Transform val3 = (Transform)args[(object)"position"];
					val2 = val3.position;
				}
				else if (args[(object)"position"].GetType() == typeof(Vector3))
				{
					val2 = (Vector3)args[(object)"position"];
				}
			}
			else
			{
				if (args.Contains((object)"x"))
				{
					val2.x = (float)args[(object)"x"];
				}
				if (args.Contains((object)"y"))
				{
					val2.y = (float)args[(object)"y"];
				}
				if (args.Contains((object)"z"))
				{
					val2.z = (float)args[(object)"z"];
				}
			}
			if (flag)
			{
				target.transform.localPosition = val2;
			}
			else
			{
				target.transform.position = val2;
			}
			args[(object)"position"] = val;
		}
		args[(object)"type"] = "move";
		args[(object)"method"] = "to";
		Launch(target, args);
	}

	public static void MoveAdd(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		MoveAdd(target, Hash("amount", amount, "time", time));
	}

	public static void MoveAdd(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args[(object)"type"] = "move";
		args[(object)"method"] = "add";
		Launch(target, args);
	}

	public static void MoveBy(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		MoveBy(target, Hash("amount", amount, "time", time));
	}

	public static void MoveBy(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args[(object)"type"] = "move";
		args[(object)"method"] = "by";
		Launch(target, args);
	}

	public static void ScaleTo(GameObject target, Vector3 scale, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ScaleTo(target, Hash("scale", scale, "time", time));
	}

	public static void ScaleTo(GameObject target, Hashtable args)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		if (args.Contains((object)"scale") && args[(object)"scale"].GetType() == typeof(Transform))
		{
			Transform val = (Transform)args[(object)"scale"];
			args[(object)"position"] = (object)new Vector3(val.position.x, val.position.y, val.position.z);
			args[(object)"rotation"] = (object)new Vector3(val.eulerAngles.x, val.eulerAngles.y, val.eulerAngles.z);
			args[(object)"scale"] = (object)new Vector3(val.localScale.x, val.localScale.y, val.localScale.z);
		}
		args[(object)"type"] = "scale";
		args[(object)"method"] = "to";
		Launch(target, args);
	}

	public static void ScaleFrom(GameObject target, Vector3 scale, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ScaleFrom(target, Hash("scale", scale, "time", time));
	}

	public static void ScaleFrom(GameObject target, Hashtable args)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		Vector3 localScale;
		Vector3 val = (localScale = target.transform.localScale);
		if (args.Contains((object)"scale"))
		{
			if (args[(object)"scale"].GetType() == typeof(Transform))
			{
				Transform val2 = (Transform)args[(object)"scale"];
				localScale = val2.localScale;
			}
			else if (args[(object)"scale"].GetType() == typeof(Vector3))
			{
				localScale = (Vector3)args[(object)"scale"];
			}
		}
		else
		{
			if (args.Contains((object)"x"))
			{
				localScale.x = (float)args[(object)"x"];
			}
			if (args.Contains((object)"y"))
			{
				localScale.y = (float)args[(object)"y"];
			}
			if (args.Contains((object)"z"))
			{
				localScale.z = (float)args[(object)"z"];
			}
		}
		target.transform.localScale = localScale;
		args[(object)"scale"] = val;
		args[(object)"type"] = "scale";
		args[(object)"method"] = "to";
		Launch(target, args);
	}

	public static void ScaleAdd(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ScaleAdd(target, Hash("amount", amount, "time", time));
	}

	public static void ScaleAdd(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args[(object)"type"] = "scale";
		args[(object)"method"] = "add";
		Launch(target, args);
	}

	public static void ScaleBy(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ScaleBy(target, Hash("amount", amount, "time", time));
	}

	public static void ScaleBy(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args[(object)"type"] = "scale";
		args[(object)"method"] = "by";
		Launch(target, args);
	}

	public static void RotateTo(GameObject target, Vector3 rotation, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		RotateTo(target, Hash("rotation", rotation, "time", time));
	}

	public static void RotateTo(GameObject target, Hashtable args)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		if (args.Contains((object)"rotation") && args[(object)"rotation"].GetType() == typeof(Transform))
		{
			Transform val = (Transform)args[(object)"rotation"];
			args[(object)"position"] = (object)new Vector3(val.position.x, val.position.y, val.position.z);
			args[(object)"rotation"] = (object)new Vector3(val.eulerAngles.x, val.eulerAngles.y, val.eulerAngles.z);
			args[(object)"scale"] = (object)new Vector3(val.localScale.x, val.localScale.y, val.localScale.z);
		}
		args[(object)"type"] = "rotate";
		args[(object)"method"] = "to";
		Launch(target, args);
	}

	public static void RotateFrom(GameObject target, Vector3 rotation, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		RotateFrom(target, Hash("rotation", rotation, "time", time));
	}

	public static void RotateFrom(GameObject target, Hashtable args)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		bool flag = ((!args.Contains((object)"islocal")) ? Defaults.isLocal : ((bool)args[(object)"islocal"]));
		Vector3 val2;
		Vector3 val = ((!flag) ? (val2 = target.transform.eulerAngles) : (val2 = target.transform.localEulerAngles));
		if (args.Contains((object)"rotation"))
		{
			if (args[(object)"rotation"].GetType() == typeof(Transform))
			{
				Transform val3 = (Transform)args[(object)"rotation"];
				val2 = val3.eulerAngles;
			}
			else if (args[(object)"rotation"].GetType() == typeof(Vector3))
			{
				val2 = (Vector3)args[(object)"rotation"];
			}
		}
		else
		{
			if (args.Contains((object)"x"))
			{
				val2.x = (float)args[(object)"x"];
			}
			if (args.Contains((object)"y"))
			{
				val2.y = (float)args[(object)"y"];
			}
			if (args.Contains((object)"z"))
			{
				val2.z = (float)args[(object)"z"];
			}
		}
		if (flag)
		{
			target.transform.localEulerAngles = val2;
		}
		else
		{
			target.transform.eulerAngles = val2;
		}
		args[(object)"rotation"] = val;
		args[(object)"type"] = "rotate";
		args[(object)"method"] = "to";
		Launch(target, args);
	}

	public static void RotateAdd(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		RotateAdd(target, Hash("amount", amount, "time", time));
	}

	public static void RotateAdd(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args[(object)"type"] = "rotate";
		args[(object)"method"] = "add";
		Launch(target, args);
	}

	public static void RotateBy(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		RotateBy(target, Hash("amount", amount, "time", time));
	}

	public static void RotateBy(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args[(object)"type"] = "rotate";
		args[(object)"method"] = "by";
		Launch(target, args);
	}

	public static void ShakePosition(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ShakePosition(target, Hash("amount", amount, "time", time));
	}

	public static void ShakePosition(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args[(object)"type"] = "shake";
		args[(object)"method"] = "position";
		Launch(target, args);
	}

	public static void ShakeScale(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ShakeScale(target, Hash("amount", amount, "time", time));
	}

	public static void ShakeScale(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args[(object)"type"] = "shake";
		args[(object)"method"] = "scale";
		Launch(target, args);
	}

	public static void ShakeRotation(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ShakeRotation(target, Hash("amount", amount, "time", time));
	}

	public static void ShakeRotation(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args[(object)"type"] = "shake";
		args[(object)"method"] = "rotation";
		Launch(target, args);
	}

	public static void PunchPosition(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PunchPosition(target, Hash("amount", amount, "time", time));
	}

	public static void PunchPosition(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args[(object)"type"] = "punch";
		args[(object)"method"] = "position";
		args[(object)"easetype"] = EaseType.punch;
		Launch(target, args);
	}

	public static void PunchRotation(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PunchRotation(target, Hash("amount", amount, "time", time));
	}

	public static void PunchRotation(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args[(object)"type"] = "punch";
		args[(object)"method"] = "rotation";
		args[(object)"easetype"] = EaseType.punch;
		Launch(target, args);
	}

	public static void PunchScale(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PunchScale(target, Hash("amount", amount, "time", time));
	}

	public static void PunchScale(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args[(object)"type"] = "punch";
		args[(object)"method"] = "scale";
		args[(object)"easetype"] = EaseType.punch;
		Launch(target, args);
	}

	private void GenerateTargets()
	{
		switch (type)
		{
		case "value":
			switch (method)
			{
			case "float":
				GenerateFloatTargets();
				apply = ApplyFloatTargets;
				break;
			case "vector2":
				GenerateVector2Targets();
				apply = ApplyVector2Targets;
				break;
			case "vector3":
				GenerateVector3Targets();
				apply = ApplyVector3Targets;
				break;
			case "color":
				GenerateColorTargets();
				apply = ApplyColorTargets;
				break;
			case "rect":
				GenerateRectTargets();
				apply = ApplyRectTargets;
				break;
			}
			break;
		case "color":
			switch (method)
			{
			case "to":
				GenerateColorToTargets();
				apply = ApplyColorToTargets;
				break;
			}
			break;
		case "audio":
			switch (method)
			{
			case "to":
				GenerateAudioToTargets();
				apply = ApplyAudioToTargets;
				break;
			}
			break;
		case "move":
			switch (method)
			{
			case "to":
				if (tweenArguments.Contains((object)"path"))
				{
					GenerateMoveToPathTargets();
					apply = ApplyMoveToPathTargets;
				}
				else
				{
					GenerateMoveToTargets();
					apply = ApplyMoveToTargets;
				}
				break;
			case "by":
			case "add":
				GenerateMoveByTargets();
				apply = ApplyMoveByTargets;
				break;
			}
			break;
		case "scale":
			switch (method)
			{
			case "to":
				GenerateScaleToTargets();
				apply = ApplyScaleToTargets;
				break;
			case "by":
				GenerateScaleByTargets();
				apply = ApplyScaleToTargets;
				break;
			case "add":
				GenerateScaleAddTargets();
				apply = ApplyScaleToTargets;
				break;
			}
			break;
		case "rotate":
			switch (method)
			{
			case "to":
				GenerateRotateToTargets();
				apply = ApplyRotateToTargets;
				break;
			case "add":
				GenerateRotateAddTargets();
				apply = ApplyRotateAddTargets;
				break;
			case "by":
				GenerateRotateByTargets();
				apply = ApplyRotateAddTargets;
				break;
			}
			break;
		case "shake":
			switch (method)
			{
			case "position":
				GenerateShakePositionTargets();
				apply = ApplyShakePositionTargets;
				break;
			case "scale":
				GenerateShakeScaleTargets();
				apply = ApplyShakeScaleTargets;
				break;
			case "rotation":
				GenerateShakeRotationTargets();
				apply = ApplyShakeRotationTargets;
				break;
			}
			break;
		case "punch":
			switch (method)
			{
			case "position":
				GeneratePunchPositionTargets();
				apply = ApplyPunchPositionTargets;
				break;
			case "rotation":
				GeneratePunchRotationTargets();
				apply = ApplyPunchRotationTargets;
				break;
			case "scale":
				GeneratePunchScaleTargets();
				apply = ApplyPunchScaleTargets;
				break;
			}
			break;
		case "look":
			switch (method)
			{
			case "to":
				GenerateLookToTargets();
				apply = ApplyLookToTargets;
				break;
			}
			break;
		case "stab":
			GenerateStabTargets();
			apply = ApplyStabTargets;
			break;
		}
	}

	private void GenerateRectTargets()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		rects = (Rect[])(object)new Rect[3];
		rects[0] = (Rect)tweenArguments[(object)"from"];
		rects[1] = (Rect)tweenArguments[(object)"to"];
	}

	private void GenerateColorTargets()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		colors = new Color[1, 3];
		colors[0, 0] = (Color)tweenArguments[(object)"from"];
		colors[0, 1] = (Color)tweenArguments[(object)"to"];
	}

	private void GenerateVector3Targets()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = (Vector3)tweenArguments[(object)"from"];
		vector3s[1] = (Vector3)tweenArguments[(object)"to"];
		if (tweenArguments.Contains((object)"speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments[(object)"speed"];
		}
	}

	private void GenerateVector2Targets()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		vector2s = (Vector2[])(object)new Vector2[3];
		vector2s[0] = (Vector2)tweenArguments[(object)"from"];
		vector2s[1] = (Vector2)tweenArguments[(object)"to"];
		if (tweenArguments.Contains((object)"speed"))
		{
			Vector3 val = default(Vector3);
			val = new Vector3(vector2s[0].x, vector2s[0].y, 0f);
			Vector3 val2 = default(Vector3);
			val2 = new Vector3(vector2s[1].x, vector2s[1].y, 0f);
			float num = Math.Abs(Vector3.Distance(val, val2));
			time = num / (float)tweenArguments[(object)"speed"];
		}
	}

	private void GenerateFloatTargets()
	{
		floats = new float[3];
		floats[0] = (float)tweenArguments[(object)"from"];
		floats[1] = (float)tweenArguments[(object)"to"];
		if (tweenArguments.Contains((object)"speed"))
		{
			float num = Math.Abs(floats[0] - floats[1]);
			time = num / (float)tweenArguments[(object)"speed"];
		}
	}

	private void GenerateColorToTargets()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		if (((Object)(object)GetComponent<GUITexture>() != (Object)null))
		{
			colors = new Color[1, 3];
			colors[0, 0] = (colors[0, 1] = GetComponent<GUITexture>().color);
		}
		else if (((Object)(object)GetComponent<GUIText>() != (Object)null))
		{
			colors = new Color[1, 3];
			colors[0, 0] = (colors[0, 1] = GetComponent<GUIText>().material.color);
		}
		else if (((Object)(object)GetComponent<Renderer>() != (Object)null))
		{
			colors = new Color[GetComponent<Renderer>().materials.Length, 3];
			for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++)
			{
				colors[i, 0] = GetComponent<Renderer>().materials[i].GetColor(((global::System.Enum)namedcolorvalue).ToString());
				colors[i, 1] = GetComponent<Renderer>().materials[i].GetColor(((global::System.Enum)namedcolorvalue).ToString());
			}
		}
		else if (((Object)(object)GetComponent<Light>() != (Object)null))
		{
			colors = new Color[1, 3];
			colors[0, 0] = (colors[0, 1] = GetComponent<Light>().color);
		}
		else
		{
			colors = new Color[1, 3];
		}
		if (tweenArguments.Contains((object)"color"))
		{
			for (int j = 0; j < ((global::System.Array)colors).GetLength(0); j++)
			{
				colors[j, 1] = (Color)tweenArguments[(object)"color"];
			}
		}
		else
		{
			if (tweenArguments.Contains((object)"r"))
			{
				for (int k = 0; k < ((global::System.Array)colors).GetLength(0); k++)
				{
					colors[k, 1].r = (float)tweenArguments[(object)"r"];
				}
			}
			if (tweenArguments.Contains((object)"g"))
			{
				for (int l = 0; l < ((global::System.Array)colors).GetLength(0); l++)
				{
					colors[l, 1].g = (float)tweenArguments[(object)"g"];
				}
			}
			if (tweenArguments.Contains((object)"b"))
			{
				for (int m = 0; m < ((global::System.Array)colors).GetLength(0); m++)
				{
					colors[m, 1].b = (float)tweenArguments[(object)"b"];
				}
			}
			if (tweenArguments.Contains((object)"a"))
			{
				for (int n = 0; n < ((global::System.Array)colors).GetLength(0); n++)
				{
					colors[n, 1].a = (float)tweenArguments[(object)"a"];
				}
			}
		}
		if (tweenArguments.Contains((object)"amount"))
		{
			for (int num = 0; num < ((global::System.Array)colors).GetLength(0); num++)
			{
				colors[num, 1].a = (float)tweenArguments[(object)"amount"];
			}
		}
		else if (tweenArguments.Contains((object)"alpha"))
		{
			for (int num2 = 0; num2 < ((global::System.Array)colors).GetLength(0); num2++)
			{
				colors[num2, 1].a = (float)tweenArguments[(object)"alpha"];
			}
		}
	}

	private void GenerateAudioToTargets()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		vector2s = (Vector2[])(object)new Vector2[3];
		if (tweenArguments.Contains((object)"audiosource"))
		{
			audioSource = (AudioSource)tweenArguments[(object)"audiosource"];
		}
		else if (((Object)(object)GetComponent<AudioSource>() != (Object)null))
		{
			audioSource = GetComponent<AudioSource>();
		}
		else
		{
			Debug.LogError((object)"iTween Error: AudioTo requires an AudioSource.");
			Dispose();
		}
		vector2s[0] = (vector2s[1] = new Vector2(audioSource.volume, audioSource.pitch));
		if (tweenArguments.Contains((object)"volume"))
		{
			vector2s[1].x = (float)tweenArguments[(object)"volume"];
		}
		if (tweenArguments.Contains((object)"pitch"))
		{
			vector2s[1].y = (float)tweenArguments[(object)"pitch"];
		}
	}

	private void GenerateStabTargets()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		if (tweenArguments.Contains((object)"audiosource"))
		{
			audioSource = (AudioSource)tweenArguments[(object)"audiosource"];
		}
		else if (((Object)(object)GetComponent<AudioSource>() != (Object)null))
		{
			audioSource = GetComponent<AudioSource>();
		}
		else
		{
			((Component)this).gameObject.AddComponent(typeof(AudioSource));
			audioSource = GetComponent<AudioSource>();
			audioSource.playOnAwake = false;
		}
		audioSource.clip = (AudioClip)tweenArguments[(object)"audioclip"];
		if (tweenArguments.Contains((object)"pitch"))
		{
			audioSource.pitch = (float)tweenArguments[(object)"pitch"];
		}
		if (tweenArguments.Contains((object)"volume"))
		{
			audioSource.volume = (float)tweenArguments[(object)"volume"];
		}
		time = audioSource.clip.length / audioSource.pitch;
	}

	private void GenerateLookToTargets()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = ((Component)this).transform.eulerAngles;
		if (tweenArguments.Contains((object)"looktarget"))
		{
			if (tweenArguments[(object)"looktarget"].GetType() == typeof(Transform))
			{
				Transform transform = ((Component)this).transform;
				Transform val = (Transform)tweenArguments[(object)"looktarget"];
				Vector3? val2 = (Vector3?)tweenArguments[(object)"up"];
				transform.LookAt(val, (!val2.HasValue) ? Defaults.up : val2.Value);
			}
			else if (tweenArguments[(object)"looktarget"].GetType() == typeof(Vector3))
			{
				Transform transform2 = ((Component)this).transform;
				Vector3 val3 = (Vector3)tweenArguments[(object)"looktarget"];
				Vector3? val4 = (Vector3?)tweenArguments[(object)"up"];
				transform2.LookAt(val3, (!val4.HasValue) ? Defaults.up : val4.Value);
			}
		}
		else
		{
			Debug.LogError((object)"iTween Error: LookTo needs a 'looktarget' property!");
			Dispose();
		}
		vector3s[1] = ((Component)this).transform.eulerAngles;
		((Component)this).transform.eulerAngles = vector3s[0];
		if (tweenArguments.Contains((object)"axis"))
		{
			switch ((string)tweenArguments[(object)"axis"])
			{
			case "x":
				vector3s[1].y = vector3s[0].y;
				vector3s[1].z = vector3s[0].z;
				break;
			case "y":
				vector3s[1].x = vector3s[0].x;
				vector3s[1].z = vector3s[0].z;
				break;
			case "z":
				vector3s[1].x = vector3s[0].x;
				vector3s[1].y = vector3s[0].y;
				break;
			}
		}
		vector3s[1] = new Vector3(clerp(vector3s[0].x, vector3s[1].x, 1f), clerp(vector3s[0].y, vector3s[1].y, 1f), clerp(vector3s[0].z, vector3s[1].z, 1f));
		if (tweenArguments.Contains((object)"speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments[(object)"speed"];
		}
	}

	private void GenerateMoveToPathTargets()
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		Vector3[] array2;
		if (tweenArguments[(object)"path"].GetType() == typeof(Vector3[]))
		{
			Vector3[] array = (Vector3[])tweenArguments[(object)"path"];
			if (array.Length == 1)
			{
				Debug.LogError((object)"iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
				Dispose();
			}
			array2 = (Vector3[])(object)new Vector3[array.Length];
			global::System.Array.Copy((global::System.Array)array, (global::System.Array)array2, array.Length);
		}
		else
		{
			Transform[] array3 = (Transform[])tweenArguments[(object)"path"];
			if (array3.Length == 1)
			{
				Debug.LogError((object)"iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
				Dispose();
			}
			array2 = (Vector3[])(object)new Vector3[array3.Length];
			for (int i = 0; i < array3.Length; i++)
			{
				array2[i] = array3[i].position;
			}
		}
		bool flag;
		int num;
		if (((Component)this).transform.position != array2[0])
		{
			if (!tweenArguments.Contains((object)"movetopath") || (bool)tweenArguments[(object)"movetopath"])
			{
				flag = true;
				num = 3;
			}
			else
			{
				flag = false;
				num = 2;
			}
		}
		else
		{
			flag = false;
			num = 2;
		}
		vector3s = (Vector3[])(object)new Vector3[array2.Length + num];
		if (flag)
		{
			vector3s[1] = ((Component)this).transform.position;
			num = 2;
		}
		else
		{
			num = 1;
		}
		global::System.Array.Copy((global::System.Array)array2, 0, (global::System.Array)vector3s, num, array2.Length);
		vector3s[0] = vector3s[1] + (vector3s[1] - vector3s[2]);
		vector3s[vector3s.Length - 1] = vector3s[vector3s.Length - 2] + (vector3s[vector3s.Length - 2] - vector3s[vector3s.Length - 3]);
		if (vector3s[1] == vector3s[vector3s.Length - 2])
		{
			Vector3[] array4 = (Vector3[])(object)new Vector3[vector3s.Length];
			global::System.Array.Copy((global::System.Array)vector3s, (global::System.Array)array4, vector3s.Length);
			array4[0] = array4[array4.Length - 3];
			array4[array4.Length - 1] = array4[2];
			vector3s = (Vector3[])(object)new Vector3[array4.Length];
			global::System.Array.Copy((global::System.Array)array4, (global::System.Array)vector3s, array4.Length);
		}
		path = new CRSpline(vector3s);
		if (tweenArguments.Contains((object)"speed"))
		{
			float num2 = PathLength(vector3s);
			time = num2 / (float)tweenArguments[(object)"speed"];
		}
	}

	private void GenerateMoveToTargets()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		if (isLocal)
		{
			vector3s[0] = (vector3s[1] = ((Component)this).transform.localPosition);
		}
		else
		{
			vector3s[0] = (vector3s[1] = ((Component)this).transform.position);
		}
		if (tweenArguments.Contains((object)"position"))
		{
			if (tweenArguments[(object)"position"].GetType() == typeof(Transform))
			{
				Transform val = (Transform)tweenArguments[(object)"position"];
				vector3s[1] = val.position;
			}
			else if (tweenArguments[(object)"position"].GetType() == typeof(Vector3))
			{
				vector3s[1] = (Vector3)tweenArguments[(object)"position"];
			}
		}
		else
		{
			if (tweenArguments.Contains((object)"x"))
			{
				vector3s[1].x = (float)tweenArguments[(object)"x"];
			}
			if (tweenArguments.Contains((object)"y"))
			{
				vector3s[1].y = (float)tweenArguments[(object)"y"];
			}
			if (tweenArguments.Contains((object)"z"))
			{
				vector3s[1].z = (float)tweenArguments[(object)"z"];
			}
		}
		if (tweenArguments.Contains((object)"orienttopath") && (bool)tweenArguments[(object)"orienttopath"])
		{
			tweenArguments[(object)"looktarget"] = vector3s[1];
		}
		if (tweenArguments.Contains((object)"speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments[(object)"speed"];
		}
	}

	private void GenerateMoveByTargets()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[6];
		vector3s[4] = ((Component)this).transform.eulerAngles;
		vector3s[0] = (vector3s[1] = (vector3s[3] = ((Component)this).transform.position));
		if (tweenArguments.Contains((object)"amount"))
		{
			vector3s[1] = vector3s[0] + (Vector3)tweenArguments[(object)"amount"];
		}
		else
		{
			if (tweenArguments.Contains((object)"x"))
			{
				vector3s[1].x = vector3s[0].x + (float)tweenArguments[(object)"x"];
			}
			if (tweenArguments.Contains((object)"y"))
			{
				vector3s[1].y = vector3s[0].y + (float)tweenArguments[(object)"y"];
			}
			if (tweenArguments.Contains((object)"z"))
			{
				vector3s[1].z = vector3s[0].z + (float)tweenArguments[(object)"z"];
			}
		}
		((Component)this).transform.Translate(vector3s[1], space);
		vector3s[5] = ((Component)this).transform.position;
		((Component)this).transform.position = vector3s[0];
		if (tweenArguments.Contains((object)"orienttopath") && (bool)tweenArguments[(object)"orienttopath"])
		{
			tweenArguments[(object)"looktarget"] = vector3s[1];
		}
		if (tweenArguments.Contains((object)"speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments[(object)"speed"];
		}
	}

	private void GenerateScaleToTargets()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = (vector3s[1] = ((Component)this).transform.localScale);
		if (tweenArguments.Contains((object)"scale"))
		{
			if (tweenArguments[(object)"scale"].GetType() == typeof(Transform))
			{
				Transform val = (Transform)tweenArguments[(object)"scale"];
				vector3s[1] = val.localScale;
			}
			else if (tweenArguments[(object)"scale"].GetType() == typeof(Vector3))
			{
				vector3s[1] = (Vector3)tweenArguments[(object)"scale"];
			}
		}
		else
		{
			if (tweenArguments.Contains((object)"x"))
			{
				vector3s[1].x = (float)tweenArguments[(object)"x"];
			}
			if (tweenArguments.Contains((object)"y"))
			{
				vector3s[1].y = (float)tweenArguments[(object)"y"];
			}
			if (tweenArguments.Contains((object)"z"))
			{
				vector3s[1].z = (float)tweenArguments[(object)"z"];
			}
		}
		if (tweenArguments.Contains((object)"speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments[(object)"speed"];
		}
	}

	private void GenerateScaleByTargets()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = (vector3s[1] = ((Component)this).transform.localScale);
		if (tweenArguments.Contains((object)"amount"))
		{
			vector3s[1] = Vector3.Scale(vector3s[1], (Vector3)tweenArguments[(object)"amount"]);
		}
		else
		{
			if (tweenArguments.Contains((object)"x"))
			{
				Vector3[] array = vector3s;
				int num = 1;
				array[num].x = array[num].x * (float)tweenArguments[(object)"x"];
			}
			if (tweenArguments.Contains((object)"y"))
			{
				Vector3[] array2 = vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y * (float)tweenArguments[(object)"y"];
			}
			if (tweenArguments.Contains((object)"z"))
			{
				Vector3[] array3 = vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z * (float)tweenArguments[(object)"z"];
			}
		}
		if (tweenArguments.Contains((object)"speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num4 / (float)tweenArguments[(object)"speed"];
		}
	}

	private void GenerateScaleAddTargets()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = (vector3s[1] = ((Component)this).transform.localScale);
		if (tweenArguments.Contains((object)"amount"))
		{
			Vector3[] array = vector3s;
			int num = 1;
			array[num] += (Vector3)tweenArguments[(object)"amount"];
		}
		else
		{
			if (tweenArguments.Contains((object)"x"))
			{
				Vector3[] array2 = vector3s;
				int num2 = 1;
				array2[num2].x = array2[num2].x + (float)tweenArguments[(object)"x"];
			}
			if (tweenArguments.Contains((object)"y"))
			{
				Vector3[] array3 = vector3s;
				int num3 = 1;
				array3[num3].y = array3[num3].y + (float)tweenArguments[(object)"y"];
			}
			if (tweenArguments.Contains((object)"z"))
			{
				Vector3[] array4 = vector3s;
				int num4 = 1;
				array4[num4].z = array4[num4].z + (float)tweenArguments[(object)"z"];
			}
		}
		if (tweenArguments.Contains((object)"speed"))
		{
			float num5 = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num5 / (float)tweenArguments[(object)"speed"];
		}
	}

	private void GenerateRotateToTargets()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		if (isLocal)
		{
			vector3s[0] = (vector3s[1] = ((Component)this).transform.localEulerAngles);
		}
		else
		{
			vector3s[0] = (vector3s[1] = ((Component)this).transform.eulerAngles);
		}
		if (tweenArguments.Contains((object)"rotation"))
		{
			if (tweenArguments[(object)"rotation"].GetType() == typeof(Transform))
			{
				Transform val = (Transform)tweenArguments[(object)"rotation"];
				vector3s[1] = val.eulerAngles;
			}
			else if (tweenArguments[(object)"rotation"].GetType() == typeof(Vector3))
			{
				vector3s[1] = (Vector3)tweenArguments[(object)"rotation"];
			}
		}
		else
		{
			if (tweenArguments.Contains((object)"x"))
			{
				vector3s[1].x = (float)tweenArguments[(object)"x"];
			}
			if (tweenArguments.Contains((object)"y"))
			{
				vector3s[1].y = (float)tweenArguments[(object)"y"];
			}
			if (tweenArguments.Contains((object)"z"))
			{
				vector3s[1].z = (float)tweenArguments[(object)"z"];
			}
		}
		vector3s[1] = new Vector3(clerp(vector3s[0].x, vector3s[1].x, 1f), clerp(vector3s[0].y, vector3s[1].y, 1f), clerp(vector3s[0].z, vector3s[1].z, 1f));
		if (tweenArguments.Contains((object)"speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments[(object)"speed"];
		}
	}

	private void GenerateRotateAddTargets()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[5];
		vector3s[0] = (vector3s[1] = (vector3s[3] = ((Component)this).transform.eulerAngles));
		if (tweenArguments.Contains((object)"amount"))
		{
			Vector3[] array = vector3s;
			int num = 1;
			array[num] += (Vector3)tweenArguments[(object)"amount"];
		}
		else
		{
			if (tweenArguments.Contains((object)"x"))
			{
				Vector3[] array2 = vector3s;
				int num2 = 1;
				array2[num2].x = array2[num2].x + (float)tweenArguments[(object)"x"];
			}
			if (tweenArguments.Contains((object)"y"))
			{
				Vector3[] array3 = vector3s;
				int num3 = 1;
				array3[num3].y = array3[num3].y + (float)tweenArguments[(object)"y"];
			}
			if (tweenArguments.Contains((object)"z"))
			{
				Vector3[] array4 = vector3s;
				int num4 = 1;
				array4[num4].z = array4[num4].z + (float)tweenArguments[(object)"z"];
			}
		}
		if (tweenArguments.Contains((object)"speed"))
		{
			float num5 = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num5 / (float)tweenArguments[(object)"speed"];
		}
	}

	private void GenerateRotateByTargets()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[4];
		vector3s[0] = (vector3s[1] = (vector3s[3] = ((Component)this).transform.eulerAngles));
		if (tweenArguments.Contains((object)"amount"))
		{
			Vector3[] array = vector3s;
			int num = 1;
			array[num] += Vector3.Scale((Vector3)tweenArguments[(object)"amount"], new Vector3(360f, 360f, 360f));
		}
		else
		{
			if (tweenArguments.Contains((object)"x"))
			{
				Vector3[] array2 = vector3s;
				int num2 = 1;
				array2[num2].x = array2[num2].x + 360f * (float)tweenArguments[(object)"x"];
			}
			if (tweenArguments.Contains((object)"y"))
			{
				Vector3[] array3 = vector3s;
				int num3 = 1;
				array3[num3].y = array3[num3].y + 360f * (float)tweenArguments[(object)"y"];
			}
			if (tweenArguments.Contains((object)"z"))
			{
				Vector3[] array4 = vector3s;
				int num4 = 1;
				array4[num4].z = array4[num4].z + 360f * (float)tweenArguments[(object)"z"];
			}
		}
		if (tweenArguments.Contains((object)"speed"))
		{
			float num5 = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num5 / (float)tweenArguments[(object)"speed"];
		}
	}

	private void GenerateShakePositionTargets()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[4];
		vector3s[3] = ((Component)this).transform.eulerAngles;
		vector3s[0] = ((Component)this).transform.position;
		if (tweenArguments.Contains((object)"amount"))
		{
			vector3s[1] = (Vector3)tweenArguments[(object)"amount"];
			return;
		}
		if (tweenArguments.Contains((object)"x"))
		{
			vector3s[1].x = (float)tweenArguments[(object)"x"];
		}
		if (tweenArguments.Contains((object)"y"))
		{
			vector3s[1].y = (float)tweenArguments[(object)"y"];
		}
		if (tweenArguments.Contains((object)"z"))
		{
			vector3s[1].z = (float)tweenArguments[(object)"z"];
		}
	}

	private void GenerateShakeScaleTargets()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = ((Component)this).transform.localScale;
		if (tweenArguments.Contains((object)"amount"))
		{
			vector3s[1] = (Vector3)tweenArguments[(object)"amount"];
			return;
		}
		if (tweenArguments.Contains((object)"x"))
		{
			vector3s[1].x = (float)tweenArguments[(object)"x"];
		}
		if (tweenArguments.Contains((object)"y"))
		{
			vector3s[1].y = (float)tweenArguments[(object)"y"];
		}
		if (tweenArguments.Contains((object)"z"))
		{
			vector3s[1].z = (float)tweenArguments[(object)"z"];
		}
	}

	private void GenerateShakeRotationTargets()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = ((Component)this).transform.eulerAngles;
		if (tweenArguments.Contains((object)"amount"))
		{
			vector3s[1] = (Vector3)tweenArguments[(object)"amount"];
			return;
		}
		if (tweenArguments.Contains((object)"x"))
		{
			vector3s[1].x = (float)tweenArguments[(object)"x"];
		}
		if (tweenArguments.Contains((object)"y"))
		{
			vector3s[1].y = (float)tweenArguments[(object)"y"];
		}
		if (tweenArguments.Contains((object)"z"))
		{
			vector3s[1].z = (float)tweenArguments[(object)"z"];
		}
	}

	private void GeneratePunchPositionTargets()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[5];
		vector3s[4] = ((Component)this).transform.eulerAngles;
		vector3s[0] = ((Component)this).transform.position;
		vector3s[1] = (vector3s[3] = Vector3.zero);
		if (tweenArguments.Contains((object)"amount"))
		{
			vector3s[1] = (Vector3)tweenArguments[(object)"amount"];
			return;
		}
		if (tweenArguments.Contains((object)"x"))
		{
			vector3s[1].x = (float)tweenArguments[(object)"x"];
		}
		if (tweenArguments.Contains((object)"y"))
		{
			vector3s[1].y = (float)tweenArguments[(object)"y"];
		}
		if (tweenArguments.Contains((object)"z"))
		{
			vector3s[1].z = (float)tweenArguments[(object)"z"];
		}
	}

	private void GeneratePunchRotationTargets()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[4];
		vector3s[0] = ((Component)this).transform.eulerAngles;
		vector3s[1] = (vector3s[3] = Vector3.zero);
		if (tweenArguments.Contains((object)"amount"))
		{
			vector3s[1] = (Vector3)tweenArguments[(object)"amount"];
			return;
		}
		if (tweenArguments.Contains((object)"x"))
		{
			vector3s[1].x = (float)tweenArguments[(object)"x"];
		}
		if (tweenArguments.Contains((object)"y"))
		{
			vector3s[1].y = (float)tweenArguments[(object)"y"];
		}
		if (tweenArguments.Contains((object)"z"))
		{
			vector3s[1].z = (float)tweenArguments[(object)"z"];
		}
	}

	private void GeneratePunchScaleTargets()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = ((Component)this).transform.localScale;
		vector3s[1] = Vector3.zero;
		if (tweenArguments.Contains((object)"amount"))
		{
			vector3s[1] = (Vector3)tweenArguments[(object)"amount"];
			return;
		}
		if (tweenArguments.Contains((object)"x"))
		{
			vector3s[1].x = (float)tweenArguments[(object)"x"];
		}
		if (tweenArguments.Contains((object)"y"))
		{
			vector3s[1].y = (float)tweenArguments[(object)"y"];
		}
		if (tweenArguments.Contains((object)"z"))
		{
			vector3s[1].z = (float)tweenArguments[(object)"z"];
		}
	}

	private void ApplyRectTargets()
	{
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		rects[2].x = ease(rects[0].x, rects[1].x, percentage);
		rects[2].y = ease(rects[0].y, rects[1].y, percentage);
		rects[2].width = ease(rects[0].width, rects[1].width, percentage);
		rects[2].height = ease(rects[0].height, rects[1].height, percentage);
		tweenArguments[(object)"onupdateparams"] = rects[2];
		if (percentage == 1f)
		{
			tweenArguments[(object)"onupdateparams"] = rects[1];
		}
	}

	private void ApplyColorTargets()
	{
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		colors[0, 2].r = ease(colors[0, 0].r, colors[0, 1].r, percentage);
		colors[0, 2].g = ease(colors[0, 0].g, colors[0, 1].g, percentage);
		colors[0, 2].b = ease(colors[0, 0].b, colors[0, 1].b, percentage);
		colors[0, 2].a = ease(colors[0, 0].a, colors[0, 1].a, percentage);
		tweenArguments[(object)"onupdateparams"] = colors[0, 2];
		if (percentage == 1f)
		{
			tweenArguments[(object)"onupdateparams"] = colors[0, 1];
		}
	}

	private void ApplyVector3Targets()
	{
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		tweenArguments[(object)"onupdateparams"] = vector3s[2];
		if (percentage == 1f)
		{
			tweenArguments[(object)"onupdateparams"] = vector3s[1];
		}
	}

	private void ApplyVector2Targets()
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		vector2s[2].x = ease(vector2s[0].x, vector2s[1].x, percentage);
		vector2s[2].y = ease(vector2s[0].y, vector2s[1].y, percentage);
		tweenArguments[(object)"onupdateparams"] = vector2s[2];
		if (percentage == 1f)
		{
			tweenArguments[(object)"onupdateparams"] = vector2s[1];
		}
	}

	private void ApplyFloatTargets()
	{
		floats[2] = ease(floats[0], floats[1], percentage);
		tweenArguments[(object)"onupdateparams"] = floats[2];
		if (percentage == 1f)
		{
			tweenArguments[(object)"onupdateparams"] = floats[1];
		}
	}

	private void ApplyColorToTargets()
	{
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < ((global::System.Array)colors).GetLength(0); i++)
		{
			colors[i, 2].r = ease(colors[i, 0].r, colors[i, 1].r, percentage);
			colors[i, 2].g = ease(colors[i, 0].g, colors[i, 1].g, percentage);
			colors[i, 2].b = ease(colors[i, 0].b, colors[i, 1].b, percentage);
			colors[i, 2].a = ease(colors[i, 0].a, colors[i, 1].a, percentage);
		}
		if (((Object)(object)GetComponent<GUITexture>() != (Object)null))
		{
			GetComponent<GUITexture>().color = colors[0, 2];
		}
		else if (((Object)(object)GetComponent<GUIText>() != (Object)null))
		{
			GetComponent<GUIText>().material.color = colors[0, 2];
		}
		else if (((Object)(object)GetComponent<Renderer>() != (Object)null))
		{
			for (int j = 0; j < ((global::System.Array)colors).GetLength(0); j++)
			{
				GetComponent<Renderer>().materials[j].SetColor(((global::System.Enum)namedcolorvalue).ToString(), colors[j, 2]);
			}
		}
		else if (((Object)(object)GetComponent<Light>() != (Object)null))
		{
			GetComponent<Light>().color = colors[0, 2];
		}
		if (percentage != 1f)
		{
			return;
		}
		if (((Object)(object)GetComponent<GUITexture>() != (Object)null))
		{
			GetComponent<GUITexture>().color = colors[0, 1];
		}
		else if (((Object)(object)GetComponent<GUIText>() != (Object)null))
		{
			GetComponent<GUIText>().material.color = colors[0, 1];
		}
		else if (((Object)(object)GetComponent<Renderer>() != (Object)null))
		{
			for (int k = 0; k < ((global::System.Array)colors).GetLength(0); k++)
			{
				GetComponent<Renderer>().materials[k].SetColor(((global::System.Enum)namedcolorvalue).ToString(), colors[k, 1]);
			}
		}
		else if (((Object)(object)GetComponent<Light>() != (Object)null))
		{
			GetComponent<Light>().color = colors[0, 1];
		}
	}

	private void ApplyAudioToTargets()
	{
		vector2s[2].x = ease(vector2s[0].x, vector2s[1].x, percentage);
		vector2s[2].y = ease(vector2s[0].y, vector2s[1].y, percentage);
		audioSource.volume = vector2s[2].x;
		audioSource.pitch = vector2s[2].y;
		if (percentage == 1f)
		{
			audioSource.volume = vector2s[1].x;
			audioSource.pitch = vector2s[1].y;
		}
	}

	private void ApplyStabTargets()
	{
	}

	private void ApplyMoveToPathTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).transform.position;
		float num = ease(0f, 1f, percentage);
		if (isLocal)
		{
			((Component)this).transform.localPosition = path.Interp(Mathf.Clamp(num, 0f, 1f));
		}
		else
		{
			((Component)this).transform.position = path.Interp(Mathf.Clamp(num, 0f, 1f));
		}
		if (tweenArguments.Contains((object)"orienttopath") && (bool)tweenArguments[(object)"orienttopath"])
		{
			float num2 = ((!tweenArguments.Contains((object)"lookahead")) ? Defaults.lookAhead : ((float)tweenArguments[(object)"lookahead"]));
			float num3 = ease(0f, 1f, Mathf.Min(1f, percentage + num2));
			tweenArguments[(object)"looktarget"] = path.Interp(Mathf.Clamp(num3, 0f, 1f));
		}
		postUpdate = ((Component)this).transform.position;
		if (physics)
		{
			((Component)this).transform.position = preUpdate;
			GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	private void ApplyMoveToTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).transform.position;
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		if (isLocal)
		{
			((Component)this).transform.localPosition = vector3s[2];
		}
		else
		{
			((Component)this).transform.position = vector3s[2];
		}
		if (percentage == 1f)
		{
			if (isLocal)
			{
				((Component)this).transform.localPosition = vector3s[1];
			}
			else
			{
				((Component)this).transform.position = vector3s[1];
			}
		}
		postUpdate = ((Component)this).transform.position;
		if (physics)
		{
			((Component)this).transform.position = preUpdate;
			GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	private void ApplyMoveByTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).transform.position;
		Vector3 eulerAngles = default(Vector3);
		if (tweenArguments.Contains((object)"looktarget"))
		{
			eulerAngles = ((Component)this).transform.eulerAngles;
			((Component)this).transform.eulerAngles = vector3s[4];
		}
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		((Component)this).transform.Translate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		if (tweenArguments.Contains((object)"looktarget"))
		{
			((Component)this).transform.eulerAngles = eulerAngles;
		}
		postUpdate = ((Component)this).transform.position;
		if (physics)
		{
			((Component)this).transform.position = preUpdate;
			GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	private void ApplyScaleToTargets()
	{
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		((Component)this).transform.localScale = vector3s[2];
		if (percentage == 1f)
		{
			((Component)this).transform.localScale = vector3s[1];
		}
	}

	private void ApplyLookToTargets()
	{
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		if (isLocal)
		{
			((Component)this).transform.localRotation = Quaternion.Euler(vector3s[2]);
		}
		else
		{
			((Component)this).transform.rotation = Quaternion.Euler(vector3s[2]);
		}
	}

	private void ApplyRotateToTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).transform.eulerAngles;
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		if (isLocal)
		{
			((Component)this).transform.localRotation = Quaternion.Euler(vector3s[2]);
		}
		else
		{
			((Component)this).transform.rotation = Quaternion.Euler(vector3s[2]);
		}
		if (percentage == 1f)
		{
			if (isLocal)
			{
				((Component)this).transform.localRotation = Quaternion.Euler(vector3s[1]);
			}
			else
			{
				((Component)this).transform.rotation = Quaternion.Euler(vector3s[1]);
			}
		}
		postUpdate = ((Component)this).transform.eulerAngles;
		if (physics)
		{
			((Component)this).transform.eulerAngles = preUpdate;
			GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	private void ApplyRotateAddTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).transform.eulerAngles;
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		((Component)this).transform.Rotate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		postUpdate = ((Component)this).transform.eulerAngles;
		if (physics)
		{
			((Component)this).transform.eulerAngles = preUpdate;
			GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	private void ApplyShakePositionTargets()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		if (isLocal)
		{
			preUpdate = ((Component)this).transform.localPosition;
		}
		else
		{
			preUpdate = ((Component)this).transform.position;
		}
		Vector3 eulerAngles = default(Vector3);
		if (tweenArguments.Contains((object)"looktarget"))
		{
			eulerAngles = ((Component)this).transform.eulerAngles;
			((Component)this).transform.eulerAngles = vector3s[3];
		}
		if (percentage == 0f)
		{
			((Component)this).transform.Translate(vector3s[1], space);
		}
		if (isLocal)
		{
			((Component)this).transform.localPosition = vector3s[0];
		}
		else
		{
			((Component)this).transform.position = vector3s[0];
		}
		float num = 1f - percentage;
		vector3s[2].x = UnityEngine.Random.Range((0f - vector3s[1].x) * num, vector3s[1].x * num);
		vector3s[2].y = UnityEngine.Random.Range((0f - vector3s[1].y) * num, vector3s[1].y * num);
		vector3s[2].z = UnityEngine.Random.Range((0f - vector3s[1].z) * num, vector3s[1].z * num);
		if (isLocal)
		{
			Transform transform = ((Component)this).transform;
			transform.localPosition += vector3s[2];
		}
		else
		{
			Transform transform2 = ((Component)this).transform;
			transform2.position += vector3s[2];
		}
		if (tweenArguments.Contains((object)"looktarget"))
		{
			((Component)this).transform.eulerAngles = eulerAngles;
		}
		postUpdate = ((Component)this).transform.position;
		if (physics)
		{
			((Component)this).transform.position = preUpdate;
			GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	private void ApplyShakeScaleTargets()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (percentage == 0f)
		{
			((Component)this).transform.localScale = vector3s[1];
		}
		((Component)this).transform.localScale = vector3s[0];
		float num = 1f - percentage;
		vector3s[2].x = UnityEngine.Random.Range((0f - vector3s[1].x) * num, vector3s[1].x * num);
		vector3s[2].y = UnityEngine.Random.Range((0f - vector3s[1].y) * num, vector3s[1].y * num);
		vector3s[2].z = UnityEngine.Random.Range((0f - vector3s[1].z) * num, vector3s[1].z * num);
		Transform transform = ((Component)this).transform;
		transform.localScale += vector3s[2];
	}

	private void ApplyShakeRotationTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).transform.eulerAngles;
		if (percentage == 0f)
		{
			((Component)this).transform.Rotate(vector3s[1], space);
		}
		((Component)this).transform.eulerAngles = vector3s[0];
		float num = 1f - percentage;
		vector3s[2].x = UnityEngine.Random.Range((0f - vector3s[1].x) * num, vector3s[1].x * num);
		vector3s[2].y = UnityEngine.Random.Range((0f - vector3s[1].y) * num, vector3s[1].y * num);
		vector3s[2].z = UnityEngine.Random.Range((0f - vector3s[1].z) * num, vector3s[1].z * num);
		((Component)this).transform.Rotate(vector3s[2], space);
		postUpdate = ((Component)this).transform.eulerAngles;
		if (physics)
		{
			((Component)this).transform.eulerAngles = preUpdate;
			GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	private void ApplyPunchPositionTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).transform.position;
		Vector3 eulerAngles = default(Vector3);
		if (tweenArguments.Contains((object)"looktarget"))
		{
			eulerAngles = ((Component)this).transform.eulerAngles;
			((Component)this).transform.eulerAngles = vector3s[4];
		}
		if (vector3s[1].x > 0f)
		{
			vector3s[2].x = punch(vector3s[1].x, percentage);
		}
		else if (vector3s[1].x < 0f)
		{
			vector3s[2].x = 0f - punch(Mathf.Abs(vector3s[1].x), percentage);
		}
		if (vector3s[1].y > 0f)
		{
			vector3s[2].y = punch(vector3s[1].y, percentage);
		}
		else if (vector3s[1].y < 0f)
		{
			vector3s[2].y = 0f - punch(Mathf.Abs(vector3s[1].y), percentage);
		}
		if (vector3s[1].z > 0f)
		{
			vector3s[2].z = punch(vector3s[1].z, percentage);
		}
		else if (vector3s[1].z < 0f)
		{
			vector3s[2].z = 0f - punch(Mathf.Abs(vector3s[1].z), percentage);
		}
		((Component)this).transform.Translate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		if (tweenArguments.Contains((object)"looktarget"))
		{
			((Component)this).transform.eulerAngles = eulerAngles;
		}
		postUpdate = ((Component)this).transform.position;
		if (physics)
		{
			((Component)this).transform.position = preUpdate;
			GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	private void ApplyPunchRotationTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).transform.eulerAngles;
		if (vector3s[1].x > 0f)
		{
			vector3s[2].x = punch(vector3s[1].x, percentage);
		}
		else if (vector3s[1].x < 0f)
		{
			vector3s[2].x = 0f - punch(Mathf.Abs(vector3s[1].x), percentage);
		}
		if (vector3s[1].y > 0f)
		{
			vector3s[2].y = punch(vector3s[1].y, percentage);
		}
		else if (vector3s[1].y < 0f)
		{
			vector3s[2].y = 0f - punch(Mathf.Abs(vector3s[1].y), percentage);
		}
		if (vector3s[1].z > 0f)
		{
			vector3s[2].z = punch(vector3s[1].z, percentage);
		}
		else if (vector3s[1].z < 0f)
		{
			vector3s[2].z = 0f - punch(Mathf.Abs(vector3s[1].z), percentage);
		}
		((Component)this).transform.Rotate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		postUpdate = ((Component)this).transform.eulerAngles;
		if (physics)
		{
			((Component)this).transform.eulerAngles = preUpdate;
			GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	private void ApplyPunchScaleTargets()
	{
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		if (vector3s[1].x > 0f)
		{
			vector3s[2].x = punch(vector3s[1].x, percentage);
		}
		else if (vector3s[1].x < 0f)
		{
			vector3s[2].x = 0f - punch(Mathf.Abs(vector3s[1].x), percentage);
		}
		if (vector3s[1].y > 0f)
		{
			vector3s[2].y = punch(vector3s[1].y, percentage);
		}
		else if (vector3s[1].y < 0f)
		{
			vector3s[2].y = 0f - punch(Mathf.Abs(vector3s[1].y), percentage);
		}
		if (vector3s[1].z > 0f)
		{
			vector3s[2].z = punch(vector3s[1].z, percentage);
		}
		else if (vector3s[1].z < 0f)
		{
			vector3s[2].z = 0f - punch(Mathf.Abs(vector3s[1].z), percentage);
		}
		((Component)this).transform.localScale = vector3s[0] + vector3s[2];
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator TweenDelay()
	{
		delayStarted = Time.time;
		yield return (object)new WaitForSeconds(delay);
		if (wasPaused)
		{
			wasPaused = false;
			TweenStart();
		}
	}

	private void TweenStart()
	{
		CallBack("onstart");
		if (!loop)
		{
			ConflictCheck();
			GenerateTargets();
		}
		if (type == "stab")
		{
			audioSource.PlayOneShot(audioSource.clip);
		}
		if (type == "move" || type == "scale" || type == "rotate" || type == "punch" || type == "shake" || type == "curve" || type == "look")
		{
			EnableKinematic();
		}
		isRunning = true;
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator TweenRestart()
	{
		if (delay > 0f)
		{
			delayStarted = Time.time;
			yield return (object)new WaitForSeconds(delay);
		}
		loop = true;
		TweenStart();
	}

	private void TweenUpdate()
	{
		apply();
		CallBack("onupdate");
		UpdatePercentage();
	}

	private void TweenComplete()
	{
		isRunning = false;
		if (percentage > 0.5f)
		{
			percentage = 1f;
		}
		else
		{
			percentage = 0f;
		}
		apply();
		if (type == "value")
		{
			CallBack("onupdate");
		}
		if (loopType == LoopType.none)
		{
			Dispose();
		}
		else
		{
			TweenLoop();
		}
		CallBack("oncomplete");
	}

	private void TweenLoop()
	{
		DisableKinematic();
		switch (loopType)
		{
		case LoopType.loop:
			percentage = 0f;
			runningTime = 0f;
			apply();
			((MonoBehaviour)this).StartCoroutine("TweenRestart");
			break;
		case LoopType.pingPong:
			reverse = !reverse;
			runningTime = 0f;
			((MonoBehaviour)this).StartCoroutine("TweenRestart");
			break;
		}
	}

	public static Rect RectUpdate(Rect currentValue, Rect targetValue, float speed)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Rect result = default(Rect);
		result = new Rect(FloatUpdate(currentValue.x, targetValue.x, speed), FloatUpdate(currentValue.y, targetValue.y, speed), FloatUpdate(currentValue.width, targetValue.width, speed), FloatUpdate(currentValue.height, targetValue.height, speed));
		return result;
	}

	public static Vector3 Vector3Update(Vector3 currentValue, Vector3 targetValue, float speed)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = targetValue - currentValue;
		currentValue += val * speed * Time.deltaTime;
		return currentValue;
	}

	public static Vector2 Vector2Update(Vector2 currentValue, Vector2 targetValue, float speed)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = targetValue - currentValue;
		currentValue += val * speed * Time.deltaTime;
		return currentValue;
	}

	public static float FloatUpdate(float currentValue, float targetValue, float speed)
	{
		float num = targetValue - currentValue;
		currentValue += num * speed * Time.deltaTime;
		return currentValue;
	}

	public static void FadeUpdate(GameObject target, Hashtable args)
	{
		args[(object)"a"] = args[(object)"alpha"];
		ColorUpdate(target, args);
	}

	public static void FadeUpdate(GameObject target, float alpha, float time)
	{
		FadeUpdate(target, Hash("alpha", alpha, "time", time));
	}

	public static void ColorUpdate(GameObject target, Hashtable args)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		CleanArgs(args);
		Color[] array = (Color[])(object)new Color[4];
		if (!args.Contains((object)"includechildren") || (bool)args[(object)"includechildren"])
		{
			foreach (Transform item in target.transform)
			{
				Transform val = item;
				ColorUpdate(((Component)val).gameObject, args);
			}
		}
		float num;
		if (args.Contains((object)"time"))
		{
			num = (float)args[(object)"time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		if (((Object)(object)target.GetComponent<GUITexture>() != (Object)null))
		{
			array[0] = (array[1] = target.GetComponent<GUITexture>().color);
		}
		else if (((Object)(object)target.GetComponent<GUIText>() != (Object)null))
		{
			array[0] = (array[1] = target.GetComponent<GUIText>().material.color);
		}
		else if (((Object)(object)target.GetComponent<Renderer>() != (Object)null))
		{
			array[0] = (array[1] = target.GetComponent<Renderer>().material.color);
		}
		else if (((Object)(object)target.GetComponent<Light>() != (Object)null))
		{
			array[0] = (array[1] = target.GetComponent<Light>().color);
		}
		if (args.Contains((object)"color"))
		{
			array[1] = (Color)args[(object)"color"];
		}
		else
		{
			if (args.Contains((object)"r"))
			{
				array[1].r = (float)args[(object)"r"];
			}
			if (args.Contains((object)"g"))
			{
				array[1].g = (float)args[(object)"g"];
			}
			if (args.Contains((object)"b"))
			{
				array[1].b = (float)args[(object)"b"];
			}
			if (args.Contains((object)"a"))
			{
				array[1].a = (float)args[(object)"a"];
			}
		}
		array[3].r = Mathf.SmoothDamp(array[0].r, array[1].r, ref array[2].r, num);
		array[3].g = Mathf.SmoothDamp(array[0].g, array[1].g, ref array[2].g, num);
		array[3].b = Mathf.SmoothDamp(array[0].b, array[1].b, ref array[2].b, num);
		array[3].a = Mathf.SmoothDamp(array[0].a, array[1].a, ref array[2].a, num);
		if (((Object)(object)target.GetComponent<GUITexture>() != (Object)null))
		{
			target.GetComponent<GUITexture>().color = array[3];
		}
		else if (((Object)(object)target.GetComponent<GUIText>() != (Object)null))
		{
			target.GetComponent<GUIText>().material.color = array[3];
		}
		else if (((Object)(object)target.GetComponent<Renderer>() != (Object)null))
		{
			target.GetComponent<Renderer>().material.color = array[3];
		}
		else if (((Object)(object)target.GetComponent<Light>() != (Object)null))
		{
			target.GetComponent<Light>().color = array[3];
		}
	}

	public static void ColorUpdate(GameObject target, Color color, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ColorUpdate(target, Hash("color", color, "time", time));
	}

	public static void AudioUpdate(GameObject target, Hashtable args)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		CleanArgs(args);
		Vector2[] array = (Vector2[])(object)new Vector2[4];
		float num;
		if (args.Contains((object)"time"))
		{
			num = (float)args[(object)"time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		AudioSource val;
		if (args.Contains((object)"audiosource"))
		{
			val = (AudioSource)args[(object)"audiosource"];
		}
		else
		{
			if (((Object)(object)target.GetComponent<AudioSource>() == (Object)null))
			{
				Debug.LogError((object)"iTween Error: AudioUpdate requires an AudioSource.");
				return;
			}
			val = target.GetComponent<AudioSource>();
		}
		array[0] = (array[1] = new Vector2(val.volume, val.pitch));
		if (args.Contains((object)"volume"))
		{
			array[1].x = (float)args[(object)"volume"];
		}
		if (args.Contains((object)"pitch"))
		{
			array[1].y = (float)args[(object)"pitch"];
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		val.volume = array[3].x;
		val.pitch = array[3].y;
	}

	public static void AudioUpdate(GameObject target, float volume, float pitch, float time)
	{
		AudioUpdate(target, Hash("volume", volume, "pitch", pitch, "time", time));
	}

	public static void RotateUpdate(GameObject target, Hashtable args)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		CleanArgs(args);
		Vector3[] array = (Vector3[])(object)new Vector3[4];
		Vector3 eulerAngles = target.transform.eulerAngles;
		float num;
		if (args.Contains((object)"time"))
		{
			num = (float)args[(object)"time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		bool flag = ((!args.Contains((object)"islocal")) ? Defaults.isLocal : ((bool)args[(object)"islocal"]));
		if (flag)
		{
			array[0] = target.transform.localEulerAngles;
		}
		else
		{
			array[0] = target.transform.eulerAngles;
		}
		if (args.Contains((object)"rotation"))
		{
			if (args[(object)"rotation"].GetType() == typeof(Transform))
			{
				Transform val = (Transform)args[(object)"rotation"];
				array[1] = val.eulerAngles;
			}
			else if (args[(object)"rotation"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args[(object)"rotation"];
			}
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
		if (flag)
		{
			target.transform.localEulerAngles = array[3];
		}
		else
		{
			target.transform.eulerAngles = array[3];
		}
		if ((Object)(object)target.GetComponent<Rigidbody>() != (Object)null)
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			target.transform.eulerAngles = eulerAngles;
			target.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(eulerAngles2));
		}
	}

	public static void RotateUpdate(GameObject target, Vector3 rotation, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		RotateUpdate(target, Hash("rotation", rotation, "time", time));
	}

	public static void ScaleUpdate(GameObject target, Hashtable args)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		CleanArgs(args);
		Vector3[] array = (Vector3[])(object)new Vector3[4];
		float num;
		if (args.Contains((object)"time"))
		{
			num = (float)args[(object)"time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		array[0] = (array[1] = target.transform.localScale);
		if (args.Contains((object)"scale"))
		{
			if (args[(object)"scale"].GetType() == typeof(Transform))
			{
				Transform val = (Transform)args[(object)"scale"];
				array[1] = val.localScale;
			}
			else if (args[(object)"scale"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args[(object)"scale"];
			}
		}
		else
		{
			if (args.Contains((object)"x"))
			{
				array[1].x = (float)args[(object)"x"];
			}
			if (args.Contains((object)"y"))
			{
				array[1].y = (float)args[(object)"y"];
			}
			if (args.Contains((object)"z"))
			{
				array[1].z = (float)args[(object)"z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		target.transform.localScale = array[3];
	}

	public static void ScaleUpdate(GameObject target, Vector3 scale, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ScaleUpdate(target, Hash("scale", scale, "time", time));
	}

	public static void MoveUpdate(GameObject target, Hashtable args)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		CleanArgs(args);
		Vector3[] array = (Vector3[])(object)new Vector3[4];
		Vector3 position = target.transform.position;
		float num;
		if (args.Contains((object)"time"))
		{
			num = (float)args[(object)"time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		bool flag = ((!args.Contains((object)"islocal")) ? Defaults.isLocal : ((bool)args[(object)"islocal"]));
		if (flag)
		{
			array[0] = (array[1] = target.transform.localPosition);
		}
		else
		{
			array[0] = (array[1] = target.transform.position);
		}
		if (args.Contains((object)"position"))
		{
			if (args[(object)"position"].GetType() == typeof(Transform))
			{
				Transform val = (Transform)args[(object)"position"];
				array[1] = val.position;
			}
			else if (args[(object)"position"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args[(object)"position"];
			}
		}
		else
		{
			if (args.Contains((object)"x"))
			{
				array[1].x = (float)args[(object)"x"];
			}
			if (args.Contains((object)"y"))
			{
				array[1].y = (float)args[(object)"y"];
			}
			if (args.Contains((object)"z"))
			{
				array[1].z = (float)args[(object)"z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		if (args.Contains((object)"orienttopath") && (bool)args[(object)"orienttopath"])
		{
			args[(object)"looktarget"] = array[3];
		}
		if (args.Contains((object)"looktarget"))
		{
			LookUpdate(target, args);
		}
		if (flag)
		{
			target.transform.localPosition = array[3];
		}
		else
		{
			target.transform.position = array[3];
		}
		if ((Object)(object)target.GetComponent<Rigidbody>() != (Object)null)
		{
			Vector3 position2 = target.transform.position;
			target.transform.position = position;
			target.GetComponent<Rigidbody>().MovePosition(position2);
		}
	}

	public static void MoveUpdate(GameObject target, Vector3 position, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		MoveUpdate(target, Hash("position", position, "time", time));
	}

	public static void LookUpdate(GameObject target, Hashtable args)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		CleanArgs(args);
		Vector3[] array = (Vector3[])(object)new Vector3[5];
		float num;
		if (args.Contains((object)"looktime"))
		{
			num = (float)args[(object)"looktime"];
			num *= Defaults.updateTimePercentage;
		}
		else if (args.Contains((object)"time"))
		{
			num = (float)args[(object)"time"] * 0.15f;
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		array[0] = target.transform.eulerAngles;
		if (args.Contains((object)"looktarget"))
		{
			if (args[(object)"looktarget"].GetType() == typeof(Transform))
			{
				Transform transform = target.transform;
				Transform val = (Transform)args[(object)"looktarget"];
				Vector3? val2 = (Vector3?)args[(object)"up"];
				transform.LookAt(val, (!val2.HasValue) ? Defaults.up : val2.Value);
			}
			else if (args[(object)"looktarget"].GetType() == typeof(Vector3))
			{
				Transform transform2 = target.transform;
				Vector3 val3 = (Vector3)args[(object)"looktarget"];
				Vector3? val4 = (Vector3?)args[(object)"up"];
				transform2.LookAt(val3, (!val4.HasValue) ? Defaults.up : val4.Value);
			}
			array[1] = target.transform.eulerAngles;
			target.transform.eulerAngles = array[0];
			array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
			array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
			array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
			target.transform.eulerAngles = array[3];
			if (args.Contains((object)"axis"))
			{
				array[4] = target.transform.eulerAngles;
				switch ((string)args[(object)"axis"])
				{
				case "x":
					array[4].y = array[0].y;
					array[4].z = array[0].z;
					break;
				case "y":
					array[4].x = array[0].x;
					array[4].z = array[0].z;
					break;
				case "z":
					array[4].x = array[0].x;
					array[4].y = array[0].y;
					break;
				}
				target.transform.eulerAngles = array[4];
			}
		}
		else
		{
			Debug.LogError((object)"iTween Error: LookUpdate needs a 'looktarget' property!");
		}
	}

	public static void LookUpdate(GameObject target, Vector3 looktarget, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		LookUpdate(target, Hash("looktarget", looktarget, "time", time));
	}

	public static float PathLength(Transform[] path)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
		float num = 0f;
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		Vector3[] pts = PathControlPointGenerator(array);
		Vector3 val = Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int j = 1; j <= num2; j++)
		{
			float t = (float)j / (float)num2;
			Vector3 val2 = Interp(pts, t);
			num += Vector3.Distance(val, val2);
			val = val2;
		}
		return num;
	}

	public static float PathLength(Vector3[] path)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		Vector3[] pts = PathControlPointGenerator(path);
		Vector3 val = Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int i = 1; i <= num2; i++)
		{
			float t = (float)i / (float)num2;
			Vector3 val2 = Interp(pts, t);
			num += Vector3.Distance(val, val2);
			val = val2;
		}
		return num;
	}

	public static Texture2D CameraTexture(Color color)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		Texture2D val = new Texture2D(Screen.width, Screen.height, (TextureFormat)5, false);
		Color[] array = (Color[])(object)new Color[Screen.width * Screen.height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = color;
		}
		val.SetPixels(array);
		val.Apply();
		return val;
	}

	public static void PutOnPath(GameObject target, Vector3[] path, float percent)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		target.transform.position = Interp(PathControlPointGenerator(path), percent);
	}

	public static void PutOnPath(Transform target, Vector3[] path, float percent)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		target.position = Interp(PathControlPointGenerator(path), percent);
	}

	public static void PutOnPath(GameObject target, Transform[] path, float percent)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.transform.position = Interp(PathControlPointGenerator(array), percent);
	}

	public static void PutOnPath(Transform target, Transform[] path, float percent)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.position = Interp(PathControlPointGenerator(array), percent);
	}

	public static Vector3 PointOnPath(Transform[] path, float percent)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		return Interp(PathControlPointGenerator(array), percent);
	}

	public static void DrawLine(Vector3[] line)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length > 0)
		{
			DrawLineHelper(line, Defaults.color, "gizmos");
		}
	}

	public static void DrawLine(Vector3[] line, Color color)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length > 0)
		{
			DrawLineHelper(line, color, "gizmos");
		}
	}

	public static void DrawLine(Transform[] line)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length > 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawLine(Transform[] line, Color color)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length > 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Vector3[] line)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length > 0)
		{
			DrawLineHelper(line, Defaults.color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Vector3[] line, Color color)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length > 0)
		{
			DrawLineHelper(line, color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Transform[] line)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length > 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Transform[] line, Color color)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length > 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, color, "gizmos");
		}
	}

	public static void DrawLineHandles(Vector3[] line)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length > 0)
		{
			DrawLineHelper(line, Defaults.color, "handles");
		}
	}

	public static void DrawLineHandles(Vector3[] line, Color color)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length > 0)
		{
			DrawLineHelper(line, color, "handles");
		}
	}

	public static void DrawLineHandles(Transform[] line)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length > 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, Defaults.color, "handles");
		}
	}

	public static void DrawLineHandles(Transform[] line, Color color)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length > 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, color, "handles");
		}
	}

	public static Vector3 PointOnPath(Vector3[] path, float percent)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Interp(PathControlPointGenerator(path), percent);
	}

	public static void DrawPath(Vector3[] path)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length > 0)
		{
			DrawPathHelper(path, Defaults.color, "gizmos");
		}
	}

	public static void DrawPath(Vector3[] path, Color color)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length > 0)
		{
			DrawPathHelper(path, color, "gizmos");
		}
	}

	public static void DrawPath(Transform[] path)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length > 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawPath(Transform[] path, Color color)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length > 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Vector3[] path)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length > 0)
		{
			DrawPathHelper(path, Defaults.color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Vector3[] path, Color color)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length > 0)
		{
			DrawPathHelper(path, color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Transform[] path)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length > 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Transform[] path, Color color)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length > 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, color, "gizmos");
		}
	}

	public static void DrawPathHandles(Vector3[] path)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length > 0)
		{
			DrawPathHelper(path, Defaults.color, "handles");
		}
	}

	public static void DrawPathHandles(Vector3[] path, Color color)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length > 0)
		{
			DrawPathHelper(path, color, "handles");
		}
	}

	public static void DrawPathHandles(Transform[] path)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length > 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, Defaults.color, "handles");
		}
	}

	public static void DrawPathHandles(Transform[] path, Color color)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length > 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, color, "handles");
		}
	}

	public static void CameraFadeDepth(int depth)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		if (((Object)(object)cameraFade != (Object)null))
		{
			cameraFade.transform.position = new Vector3(cameraFade.transform.position.x, cameraFade.transform.position.y, (float)depth);
		}
	}

	public static void CameraFadeDestroy()
	{
		if (((Object)(object)cameraFade != (Object)null))
		{
			Object.Destroy((Object)(object)cameraFade);
		}
	}

	public static void CameraFadeSwap(Texture2D texture)
	{
		if (((Object)(object)cameraFade != (Object)null))
		{
			cameraFade.GetComponent<GUITexture>().texture = (Texture)(object)texture;
		}
	}

	public static GameObject CameraFadeAdd(Texture2D texture, int depth)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		if (((Object)(object)cameraFade != (Object)null))
		{
			return null;
		}
		cameraFade = new GameObject("iTween Camera Fade");
		cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)depth);
		cameraFade.AddComponent<GUITexture>();
		cameraFade.GetComponent<GUITexture>().texture = (Texture)(object)texture;
		cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return cameraFade;
	}

	public static GameObject CameraFadeAdd(Texture2D texture)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		if (((Object)(object)cameraFade != (Object)null))
		{
			return null;
		}
		cameraFade = new GameObject("iTween Camera Fade");
		cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)Defaults.cameraFadeDepth);
		cameraFade.AddComponent<GUITexture>();
		cameraFade.GetComponent<GUITexture>().texture = (Texture)(object)texture;
		cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return cameraFade;
	}

	public static GameObject CameraFadeAdd()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		if (((Object)(object)cameraFade != (Object)null))
		{
			return null;
		}
		cameraFade = new GameObject("iTween Camera Fade");
		cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)Defaults.cameraFadeDepth);
		cameraFade.AddComponent<GUITexture>();
		cameraFade.GetComponent<GUITexture>().texture = (Texture)(object)CameraTexture(Color.black);
		cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return cameraFade;
	}

	public static void Resume(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		Component[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			iTween iTween2 = (iTween)(object)array[i];
			((Behaviour)iTween2).enabled = true;
		}
	}

	public static void Resume(GameObject target, bool includechildren)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		Resume(target);
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Transform val = item;
			Resume(((Component)val).gameObject, true);
		}
	}

	public static void Resume(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		Component[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			iTween iTween2 = (iTween)(object)array[i];
			string text = iTween2.type + iTween2.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				((Behaviour)iTween2).enabled = true;
			}
		}
	}

	public static void Resume(GameObject target, string type, bool includechildren)
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		Component[] components = target.GetComponents(typeof(iTween));
		Component[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			iTween iTween2 = (iTween)(object)array[i];
			string text = iTween2.type + iTween2.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				((Behaviour)iTween2).enabled = true;
			}
		}
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Transform val = item;
			Resume(((Component)val).gameObject, type, true);
		}
	}

	public static void Resume()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable val = (Hashtable)tweens[i];
			GameObject target = (GameObject)val[(object)"target"];
			Resume(target);
		}
	}

	public static void Resume(string type)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		ArrayList val = new ArrayList();
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable val2 = (Hashtable)tweens[i];
			GameObject val3 = (GameObject)val2[(object)"target"];
			val.Insert(val.Count, (object)val3);
		}
		for (int j = 0; j < val.Count; j++)
		{
			Resume((GameObject)val[j], type);
		}
	}

	public static void Pause(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		Component[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			iTween iTween2 = (iTween)(object)array[i];
			if (iTween2.delay > 0f)
			{
				iTween2.delay -= Time.time - iTween2.delayStarted;
				((MonoBehaviour)iTween2).StopCoroutine("TweenDelay");
			}
			iTween2.isPaused = true;
			((Behaviour)iTween2).enabled = false;
		}
	}

	public static void Pause(GameObject target, bool includechildren)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		Pause(target);
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Transform val = item;
			Pause(((Component)val).gameObject, true);
		}
	}

	public static void Pause(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		Component[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			iTween iTween2 = (iTween)(object)array[i];
			string text = iTween2.type + iTween2.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				if (iTween2.delay > 0f)
				{
					iTween2.delay -= Time.time - iTween2.delayStarted;
					((MonoBehaviour)iTween2).StopCoroutine("TweenDelay");
				}
				iTween2.isPaused = true;
				((Behaviour)iTween2).enabled = false;
			}
		}
	}

	public static void Pause(GameObject target, string type, bool includechildren)
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		Component[] components = target.GetComponents(typeof(iTween));
		Component[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			iTween iTween2 = (iTween)(object)array[i];
			string text = iTween2.type + iTween2.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				if (iTween2.delay > 0f)
				{
					iTween2.delay -= Time.time - iTween2.delayStarted;
					((MonoBehaviour)iTween2).StopCoroutine("TweenDelay");
				}
				iTween2.isPaused = true;
				((Behaviour)iTween2).enabled = false;
			}
		}
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Transform val = item;
			Pause(((Component)val).gameObject, type, true);
		}
	}

	public static void Pause()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable val = (Hashtable)tweens[i];
			GameObject target = (GameObject)val[(object)"target"];
			Pause(target);
		}
	}

	public static void Pause(string type)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		ArrayList val = new ArrayList();
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable val2 = (Hashtable)tweens[i];
			GameObject val3 = (GameObject)val2[(object)"target"];
			val.Insert(val.Count, (object)val3);
		}
		for (int j = 0; j < val.Count; j++)
		{
			Pause((GameObject)val[j], type);
		}
	}

	public static int Count()
	{
		return tweens.Count;
	}

	public static int Count(string type)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		int num = 0;
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable val = (Hashtable)tweens[i];
			string text = (string)val[(object)"type"] + (string)val[(object)"method"];
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	public static int Count(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		return components.Length;
	}

	public static int Count(GameObject target, string type)
	{
		int num = 0;
		Component[] components = target.GetComponents(typeof(iTween));
		Component[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			iTween iTween2 = (iTween)(object)array[i];
			string text = iTween2.type + iTween2.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	public static void Stop()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable val = (Hashtable)tweens[i];
			GameObject target = (GameObject)val[(object)"target"];
			Stop(target);
		}
		tweens.Clear();
	}

	public static void Stop(string type)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		ArrayList val = new ArrayList();
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable val2 = (Hashtable)tweens[i];
			GameObject val3 = (GameObject)val2[(object)"target"];
			val.Insert(val.Count, (object)val3);
		}
		for (int j = 0; j < val.Count; j++)
		{
			Stop((GameObject)val[j], type);
		}
	}

	public static void StopByName(string name)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		ArrayList val = new ArrayList();
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable val2 = (Hashtable)tweens[i];
			GameObject val3 = (GameObject)val2[(object)"target"];
			val.Insert(val.Count, (object)val3);
		}
		for (int j = 0; j < val.Count; j++)
		{
			StopByName((GameObject)val[j], name);
		}
	}

	public static void Stop(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		Component[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			iTween iTween2 = (iTween)(object)array[i];
			iTween2.Dispose();
		}
	}

	public static void Stop(GameObject target, bool includechildren)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		Stop(target);
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Transform val = item;
			Stop(((Component)val).gameObject, true);
		}
	}

	public static void Stop(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		Component[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			iTween iTween2 = (iTween)(object)array[i];
			string text = iTween2.type + iTween2.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				iTween2.Dispose();
			}
		}
	}

	public static void StopByName(GameObject target, string name)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		Component[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			iTween iTween2 = (iTween)(object)array[i];
			if (iTween2._name == name)
			{
				iTween2.Dispose();
			}
		}
	}

	public static void Stop(GameObject target, string type, bool includechildren)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		Component[] components = target.GetComponents(typeof(iTween));
		Component[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			iTween iTween2 = (iTween)(object)array[i];
			string text = iTween2.type + iTween2.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				iTween2.Dispose();
			}
		}
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Transform val = item;
			Stop(((Component)val).gameObject, type, true);
		}
	}

	public static void StopByName(GameObject target, string name, bool includechildren)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		Component[] components = target.GetComponents(typeof(iTween));
		Component[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			iTween iTween2 = (iTween)(object)array[i];
			if (iTween2._name == name)
			{
				iTween2.Dispose();
			}
		}
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Transform val = item;
			StopByName(((Component)val).gameObject, name, true);
		}
	}

	public static Hashtable Hash(params object[] args)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		Hashtable val = new Hashtable(args.Length / 2);
		if (args.Length % 2 != 0)
		{
			Debug.LogError((object)"Tween Error: Hash requires an even number of arguments!");
			return null;
		}
		for (int i = 0; i < args.Length - 1; i += 2)
		{
			val.Add(args[i], args[i + 1]);
		}
		return val;
	}

	private void Awake()
	{
		RetrieveArgs();
		lastRealTime = Time.realtimeSinceStartup;
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator Start()
	{
		if (delay > 0f)
		{
			yield return ((MonoBehaviour)this).StartCoroutine("TweenDelay");
		}
		TweenStart();
	}

	private void Update()
	{
		if (!isRunning || physics)
		{
			return;
		}
		if (!reverse)
		{
			if (percentage < 1f)
			{
				TweenUpdate();
			}
			else
			{
				TweenComplete();
			}
		}
		else if (percentage > 0f)
		{
			TweenUpdate();
		}
		else
		{
			TweenComplete();
		}
	}

	private void FixedUpdate()
	{
		if (!isRunning || !physics)
		{
			return;
		}
		if (!reverse)
		{
			if (percentage < 1f)
			{
				TweenUpdate();
			}
			else
			{
				TweenComplete();
			}
		}
		else if (percentage > 0f)
		{
			TweenUpdate();
		}
		else
		{
			TweenComplete();
		}
	}

	private void LateUpdate()
	{
		if (tweenArguments.Contains((object)"looktarget") && isRunning && (type == "move" || type == "shake" || type == "punch"))
		{
			LookUpdate(((Component)this).gameObject, tweenArguments);
		}
	}

	private void OnEnable()
	{
		if (isRunning)
		{
			EnableKinematic();
		}
		if (isPaused)
		{
			isPaused = false;
			if (delay > 0f)
			{
				wasPaused = true;
				ResumeDelay();
			}
		}
	}

	private void OnDisable()
	{
		DisableKinematic();
	}

	private static void DrawLineHelper(Vector3[] line, Color color, string method)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		Gizmos.color = color;
		for (int i = 0; i < line.Length - 1; i++)
		{
			if (method == "gizmos")
			{
				Gizmos.DrawLine(line[i], line[i + 1]);
			}
			else if (method == "handles")
			{
				Debug.LogError((object)"iTween Error: Drawing a line with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
			}
		}
	}

	private static void DrawPathHelper(Vector3[] path, Color color, string method)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		Vector3[] pts = PathControlPointGenerator(path);
		Vector3 val = Interp(pts, 0f);
		Gizmos.color = color;
		int num = path.Length * 20;
		for (int i = 1; i <= num; i++)
		{
			float t = (float)i / (float)num;
			Vector3 val2 = Interp(pts, t);
			if (method == "gizmos")
			{
				Gizmos.DrawLine(val2, val);
			}
			else if (method == "handles")
			{
				Debug.LogError((object)"iTween Error: Drawing a path with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
			}
			val = val2;
		}
	}

	private static Vector3[] PathControlPointGenerator(Vector3[] path)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		int num = 2;
		Vector3[] array = (Vector3[])(object)new Vector3[path.Length + num];
		global::System.Array.Copy((global::System.Array)path, 0, (global::System.Array)array, 1, path.Length);
		array[0] = array[1] + (array[1] - array[2]);
		array[array.Length - 1] = array[array.Length - 2] + (array[array.Length - 2] - array[array.Length - 3]);
		if (array[1] == array[array.Length - 2])
		{
			Vector3[] array2 = (Vector3[])(object)new Vector3[array.Length];
			global::System.Array.Copy((global::System.Array)array, (global::System.Array)array2, array.Length);
			array2[0] = array2[array2.Length - 3];
			array2[array2.Length - 1] = array2[2];
			array = (Vector3[])(object)new Vector3[array2.Length];
			global::System.Array.Copy((global::System.Array)array2, (global::System.Array)array, array2.Length);
		}
		return array;
	}

	private static Vector3 Interp(Vector3[] pts, float t)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 val = pts[num2];
		Vector3 val2 = pts[num2 + 1];
		Vector3 val3 = pts[num2 + 2];
		Vector3 val4 = pts[num2 + 3];
		return 0.5f * ((-val + 3f * val2 - 3f * val3 + val4) * (num3 * num3 * num3) + (2f * val - 5f * val2 + 4f * val3 - val4) * (num3 * num3) + (-val + val3) * num3 + 2f * val2);
	}

	private static void Launch(GameObject target, Hashtable args)
	{
		if (!args.Contains((object)"id"))
		{
			args[(object)"id"] = GenerateID();
		}
		if (!args.Contains((object)"target"))
		{
			args[(object)"target"] = target;
		}
		tweens.Insert(0, (object)args);
		target.AddComponent<iTween>();
	}

	private static Hashtable CleanArgs(Hashtable args)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		Hashtable val = new Hashtable(args.Count);
		Hashtable val2 = new Hashtable(args.Count);
		IDictionaryEnumerator enumerator = args.GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				DictionaryEntry val3 = (DictionaryEntry)((global::System.Collections.IEnumerator)enumerator).Current;
				val.Add(val3.Key, val3.Value);
			}
		}
		finally
		{
			global::System.IDisposable disposable = enumerator as global::System.IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		IDictionaryEnumerator enumerator2 = val.GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator2).MoveNext())
			{
				DictionaryEntry val4 = (DictionaryEntry)((global::System.Collections.IEnumerator)enumerator2).Current;
				if (val4.Value.GetType() == typeof(int))
				{
					int num = (int)val4.Value;
					float num2 = num;
					args[val4.Key] = num2;
				}
				if (val4.Value.GetType() == typeof(double))
				{
					double num3 = (double)val4.Value;
					float num4 = (float)num3;
					args[val4.Key] = num4;
				}
			}
		}
		finally
		{
			global::System.IDisposable disposable2 = enumerator2 as global::System.IDisposable;
			if (disposable2 != null)
			{
				disposable2.Dispose();
			}
		}
		IDictionaryEnumerator enumerator3 = args.GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator3).MoveNext())
			{
				DictionaryEntry val5 = (DictionaryEntry)((global::System.Collections.IEnumerator)enumerator3).Current;
				val2.Add((object)val5.Key.ToString().ToLower(), val5.Value);
			}
		}
		finally
		{
			global::System.IDisposable disposable3 = enumerator3 as global::System.IDisposable;
			if (disposable3 != null)
			{
				disposable3.Dispose();
			}
		}
		args = val2;
		return args;
	}

	private static string GenerateID()
	{
		int num = 15;
		char[] array = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
		char[] array2 = array;
		int num2 = array2.Length - 1;
		string text = string.Empty;
		for (int i = 0; i < num; i++)
		{
			text = string.Concat((object)text, (object)array2[(int)Mathf.Floor((float)UnityEngine.Random.Range(0, num2))]);
		}
		return text;
	}

	private void RetrieveArgs()
	{
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		foreach (Hashtable tween in tweens)
		{
			Hashtable val = tween;
			if ((Object)(GameObject)val[(object)"target"] == (Object)(object)((Component)this).gameObject)
			{
				tweenArguments = val;
				break;
			}
		}
		id = (string)tweenArguments[(object)"id"];
		type = (string)tweenArguments[(object)"type"];
		_name = (string)tweenArguments[(object)"name"];
		method = (string)tweenArguments[(object)"method"];
		if (tweenArguments.Contains((object)"time"))
		{
			time = (float)tweenArguments[(object)"time"];
		}
		else
		{
			time = Defaults.time;
		}
		if ((Object)(object)GetComponent<Rigidbody>() != (Object)null)
		{
			physics = true;
		}
		if (tweenArguments.Contains((object)"delay"))
		{
			delay = (float)tweenArguments[(object)"delay"];
		}
		else
		{
			delay = Defaults.delay;
		}
		if (tweenArguments.Contains((object)"namedcolorvalue"))
		{
			if (tweenArguments[(object)"namedcolorvalue"].GetType() == typeof(NamedValueColor))
			{
				namedcolorvalue = (NamedValueColor)(int)tweenArguments[(object)"namedcolorvalue"];
			}
			else
			{
				try
				{
					namedcolorvalue = (NamedValueColor)(int)global::System.Enum.Parse(typeof(NamedValueColor), (string)tweenArguments[(object)"namedcolorvalue"], true);
				}
				catch
				{
					Debug.LogWarning((object)"iTween: Unsupported namedcolorvalue supplied! Default will be used.");
					namedcolorvalue = NamedValueColor._Color;
				}
			}
		}
		else
		{
			namedcolorvalue = Defaults.namedColorValue;
		}
		if (tweenArguments.Contains((object)"looptype"))
		{
			if (tweenArguments[(object)"looptype"].GetType() == typeof(LoopType))
			{
				loopType = (LoopType)(int)tweenArguments[(object)"looptype"];
			}
			else
			{
				try
				{
					loopType = (LoopType)(int)global::System.Enum.Parse(typeof(LoopType), (string)tweenArguments[(object)"looptype"], true);
				}
				catch
				{
					Debug.LogWarning((object)"iTween: Unsupported loopType supplied! Default will be used.");
					loopType = LoopType.none;
				}
			}
		}
		else
		{
			loopType = LoopType.none;
		}
		if (tweenArguments.Contains((object)"easetype"))
		{
			if (tweenArguments[(object)"easetype"].GetType() == typeof(EaseType))
			{
				easeType = (EaseType)(int)tweenArguments[(object)"easetype"];
			}
			else
			{
				try
				{
					easeType = (EaseType)(int)global::System.Enum.Parse(typeof(EaseType), (string)tweenArguments[(object)"easetype"], true);
				}
				catch
				{
					Debug.LogWarning((object)"iTween: Unsupported easeType supplied! Default will be used.");
					easeType = Defaults.easeType;
				}
			}
		}
		else
		{
			easeType = Defaults.easeType;
		}
		if (tweenArguments.Contains((object)"space"))
		{
			if (tweenArguments[(object)"space"].GetType() == typeof(Space))
			{
				space = (Space)(int)tweenArguments[(object)"space"];
			}
			else
			{
				try
				{
					space = (Space)(int)global::System.Enum.Parse(typeof(Space), (string)tweenArguments[(object)"space"], true);
				}
				catch
				{
					Debug.LogWarning((object)"iTween: Unsupported space supplied! Default will be used.");
					space = Defaults.space;
				}
			}
		}
		else
		{
			space = Defaults.space;
		}
		if (tweenArguments.Contains((object)"islocal"))
		{
			isLocal = (bool)tweenArguments[(object)"islocal"];
		}
		else
		{
			isLocal = Defaults.isLocal;
		}
		if (tweenArguments.Contains((object)"ignoretimescale"))
		{
			useRealTime = (bool)tweenArguments[(object)"ignoretimescale"];
		}
		else
		{
			useRealTime = Defaults.useRealTime;
		}
		GetEasingFunction();
	}

	private void GetEasingFunction()
	{
		switch (easeType)
		{
		case EaseType.easeInQuad:
			ease = easeInQuad;
			break;
		case EaseType.easeOutQuad:
			ease = easeOutQuad;
			break;
		case EaseType.easeInOutQuad:
			ease = easeInOutQuad;
			break;
		case EaseType.easeInCubic:
			ease = easeInCubic;
			break;
		case EaseType.easeOutCubic:
			ease = easeOutCubic;
			break;
		case EaseType.easeInOutCubic:
			ease = easeInOutCubic;
			break;
		case EaseType.easeInQuart:
			ease = easeInQuart;
			break;
		case EaseType.easeOutQuart:
			ease = easeOutQuart;
			break;
		case EaseType.easeInOutQuart:
			ease = easeInOutQuart;
			break;
		case EaseType.easeInQuint:
			ease = easeInQuint;
			break;
		case EaseType.easeOutQuint:
			ease = easeOutQuint;
			break;
		case EaseType.easeInOutQuint:
			ease = easeInOutQuint;
			break;
		case EaseType.easeInSine:
			ease = easeInSine;
			break;
		case EaseType.easeOutSine:
			ease = easeOutSine;
			break;
		case EaseType.easeInOutSine:
			ease = easeInOutSine;
			break;
		case EaseType.easeInExpo:
			ease = easeInExpo;
			break;
		case EaseType.easeOutExpo:
			ease = easeOutExpo;
			break;
		case EaseType.easeInOutExpo:
			ease = easeInOutExpo;
			break;
		case EaseType.easeInCirc:
			ease = easeInCirc;
			break;
		case EaseType.easeOutCirc:
			ease = easeOutCirc;
			break;
		case EaseType.easeInOutCirc:
			ease = easeInOutCirc;
			break;
		case EaseType.linear:
			ease = linear;
			break;
		case EaseType.spring:
			ease = spring;
			break;
		case EaseType.easeInBounce:
			ease = easeInBounce;
			break;
		case EaseType.easeOutBounce:
			ease = easeOutBounce;
			break;
		case EaseType.easeInOutBounce:
			ease = easeInOutBounce;
			break;
		case EaseType.easeInBack:
			ease = easeInBack;
			break;
		case EaseType.easeOutBack:
			ease = easeOutBack;
			break;
		case EaseType.easeInOutBack:
			ease = easeInOutBack;
			break;
		case EaseType.easeInElastic:
			ease = easeInElastic;
			break;
		case EaseType.easeOutElastic:
			ease = easeOutElastic;
			break;
		case EaseType.easeInOutElastic:
			ease = easeInOutElastic;
			break;
		}
	}

	private void UpdatePercentage()
	{
		if (useRealTime)
		{
			runningTime += Time.realtimeSinceStartup - lastRealTime;
		}
		else
		{
			runningTime += Time.deltaTime;
		}
		if (reverse)
		{
			percentage = 1f - runningTime / time;
		}
		else
		{
			percentage = runningTime / time;
		}
		lastRealTime = Time.realtimeSinceStartup;
	}

	private void CallBack(string callbackType)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		if (tweenArguments.Contains((object)callbackType) && !tweenArguments.Contains((object)"ischild"))
		{
			GameObject val = (GameObject)((!tweenArguments.Contains((object)(callbackType + "target"))) ? ((object)((Component)this).gameObject) : ((object)(GameObject)tweenArguments[(object)(callbackType + "target")]));
			if (tweenArguments[(object)callbackType].GetType() == typeof(string))
			{
				val.SendMessage((string)tweenArguments[(object)callbackType], tweenArguments[(object)(callbackType + "params")], (SendMessageOptions)1);
				return;
			}
			Debug.LogError((object)"iTween Error: Callback method references must be passed as a String!");
			Object.Destroy((Object)(object)this);
		}
	}

	private void Dispose()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable val = (Hashtable)tweens[i];
			if ((string)val[(object)"id"] == id)
			{
				tweens.RemoveAt(i);
				break;
			}
		}
		Object.Destroy((Object)(object)this);
	}

	private void ConflictCheck()
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		Component[] components = ((Component)this).GetComponents(typeof(iTween));
		Component[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			iTween iTween2 = (iTween)(object)array[i];
			if (iTween2.type == "value")
			{
				break;
			}
			if (!iTween2.isRunning || !(iTween2.type == type))
			{
				continue;
			}
			if (iTween2.method != method)
			{
				break;
			}
			if (iTween2.tweenArguments.Count != tweenArguments.Count)
			{
				iTween2.Dispose();
				break;
			}
			IDictionaryEnumerator enumerator = tweenArguments.GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					DictionaryEntry val = (DictionaryEntry)((global::System.Collections.IEnumerator)enumerator).Current;
					if (!iTween2.tweenArguments.Contains(val.Key))
					{
						iTween2.Dispose();
						return;
					}
					if (!iTween2.tweenArguments[val.Key].Equals(tweenArguments[val.Key]) && (string)val.Key != "id")
					{
						iTween2.Dispose();
						return;
					}
				}
			}
			finally
			{
				global::System.IDisposable disposable = enumerator as global::System.IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			Dispose();
		}
	}

	private void EnableKinematic()
	{
	}

	private void DisableKinematic()
	{
	}

	private void ResumeDelay()
	{
		((MonoBehaviour)this).StartCoroutine("TweenDelay");
	}

	private float linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	private float clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float num4 = 0f;
		float num5 = 0f;
		if (end - start < 0f - num3)
		{
			num5 = (num2 - start + end) * value;
			return start + num5;
		}
		if (end - start > num3)
		{
			num5 = (0f - (num2 - end + start)) * value;
			return start + num5;
		}
		return start + (end - start) * value;
	}

	private float spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * (float)Math.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	private float easeInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	private float easeOutQuad(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * value * (value - 2f) + start;
	}

	private float easeInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value + start;
		}
		value -= 1f;
		return (0f - end) / 2f * (value * (value - 2f) - 1f) + start;
	}

	private float easeInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	private float easeOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	private float easeInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value + 2f) + start;
	}

	private float easeInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	private float easeOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return (0f - end) * (value * value * value * value - 1f) + start;
	}

	private float easeInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value + start;
		}
		value -= 2f;
		return (0f - end) / 2f * (value * value * value * value - 2f) + start;
	}

	private float easeInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	private float easeOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	private float easeInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value * value * value + 2f) + start;
	}

	private float easeInSine(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * Mathf.Cos(value / 1f * ((float)Math.PI / 2f)) + end + start;
	}

	private float easeOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value / 1f * ((float)Math.PI / 2f)) + start;
	}

	private float easeInOutSine(float start, float end, float value)
	{
		end -= start;
		return (0f - end) / 2f * (Mathf.Cos((float)Math.PI * value / 1f) - 1f) + start;
	}

	private float easeInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value / 1f - 1f)) + start;
	}

	private float easeOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (0f - Mathf.Pow(2f, -10f * value / 1f) + 1f) + start;
	}

	private float easeInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end / 2f * (0f - Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	private float easeInCirc(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	private float easeOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	private float easeInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return (0f - end) / 2f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end / 2f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	private float easeInBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return end - easeOutBounce(0f, end, num - value) + start;
	}

	private float easeOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.36363637f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.72727275f)
		{
			value -= 0.54545456f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 21f / 22f;
		return end * (7.5625f * value * value + 63f / 64f) + start;
	}

	private float easeInOutBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num / 2f)
		{
			return easeInBounce(0f, end, value * 2f) * 0.5f + start;
		}
		return easeOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
	}

	private float easeInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	private float easeOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value = value / 1f - 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	private float easeInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end / 2f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end / 2f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	private float punch(float amplitude, float value)
	{
		float num = 9f;
		if (value == 0f)
		{
			return 0f;
		}
		if (value == 1f)
		{
			return 0f;
		}
		float num2 = 0.3f;
		num = num2 / ((float)Math.PI * 2f) * Mathf.Asin(0f);
		return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num) * ((float)Math.PI * 2f) / num2);
	}

	private float easeInElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		if (num4 == 0f || num4 < Mathf.Abs(end))
		{
			num4 = end;
			num3 = num2 / 4f;
		}
		else
		{
			num3 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num4);
		}
		return 0f - num4 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num3) * ((float)Math.PI * 2f) / num2) + start;
	}

	private float easeOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		if (num4 == 0f || num4 < Mathf.Abs(end))
		{
			num4 = end;
			num3 = num2 / 4f;
		}
		else
		{
			num3 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num4);
		}
		return num4 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num3) * ((float)Math.PI * 2f) / num2) + end + start;
	}

	private float easeInOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num / 2f) == 2f)
		{
			return start + end;
		}
		if (num4 == 0f || num4 < Mathf.Abs(end))
		{
			num4 = end;
			num3 = num2 / 4f;
		}
		else
		{
			num3 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num4);
		}
		if (value < 1f)
		{
			return -0.5f * (num4 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num3) * ((float)Math.PI * 2f) / num2)) + start;
		}
		return num4 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num3) * ((float)Math.PI * 2f) / num2) * 0.5f + end + start;
	}
}
