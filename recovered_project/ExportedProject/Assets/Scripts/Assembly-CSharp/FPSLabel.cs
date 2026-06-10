using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class FPSLabel : MonoBehaviour
{
	public UILabel uiLabel;

	public float updateInterval = 0.5f;

	private float accum;

	private int frames;

	private float timeleft;

	private void Start()
	{
		if (((Object)(object)uiLabel == (Object)null))
		{
			Debug.Log((object)"FPSLabel needs a UILabel component!");
			((Behaviour)this).enabled = false;
		}
		else
		{
			timeleft = updateInterval;
		}
	}

	private void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames++;
		if ((double)timeleft <= 0.0)
		{
			float num = accum / (float)frames;
			string text = string.Format("{0:F2} FPS", (object)num);
			uiLabel.text = text;
			timeleft = updateInterval;
			accum = 0f;
			frames = 0;
		}
	}
}
