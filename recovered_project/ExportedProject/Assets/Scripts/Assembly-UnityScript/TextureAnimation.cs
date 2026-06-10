using System;
using UnityEngine;

[Serializable]
public class TextureAnimation : MonoBehaviour
{
	public int uvAnimationTileX;

	public int uvAnimationTileY;

	public float framesPerSecond;

	public TextureAnimation()
	{
		uvAnimationTileX = 24;
		uvAnimationTileY = 1;
		framesPerSecond = 10f;
	}

	public void Update()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)(Time.time * framesPerSecond);
		num %= uvAnimationTileX * uvAnimationTileY;
		Vector2 val = new Vector2(1f / (float)uvAnimationTileX, 1f / (float)uvAnimationTileY);
		int num2 = num % uvAnimationTileX;
		int num3 = num / uvAnimationTileX;
		Vector2 val2 = new Vector2((float)num2 * val.x, 1f - val.y - (float)num3 * val.y);
		GetComponent<Renderer>().material.SetTextureOffset("_MainTex", val2);
		GetComponent<Renderer>().material.SetTextureScale("_MainTex", val);
	}

	public void Main()
	{
	}
}
