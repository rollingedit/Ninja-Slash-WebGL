using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class WaterfallAnimation : MonoBehaviour
{
	public float speed = 1f / 6f;

	public float strength = 0.15f;

	private void Start()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		AnimationClip val = new AnimationClip();
		((Object)val).name = "water";
		val.wrapMode = (WrapMode)2;
		AnimationCurve val2 = new AnimationCurve();
		AnimationCurve val3 = new AnimationCurve();
		for (int i = 0; i < 90; i++)
		{
			val2.AddKey((float)i / speed, strength * (float)i);
		}
		val2.postWrapMode = (WrapMode)2;
		val2.preWrapMode = (WrapMode)2;
		val3.postWrapMode = (WrapMode)2;
		val3.preWrapMode = (WrapMode)2;
		val.SetCurve(string.Empty, typeof(Material), "_MainTex.offset.y", val2);
		GetComponent<Animation>().AddClip(val, ((Object)val).name);
		GetComponent<Animation>().clip = val;
		GetComponent<Animation>().Play(((Object)val).name);
	}
}
