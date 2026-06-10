using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ResumePanel : MonoBehaviour
{
	public UILabel countLabel;

	private float leftTime;

	private void OnResumeCount(float leftTimeIn)
	{
		leftTime = leftTimeIn;
		if (leftTime == 0f)
		{
			countLabel.text = string.Empty;
		}
		else
		{
			countLabel.text = Mathf.CeilToInt(leftTime).ToString();
		}
	}

	private void OnResume()
	{
		PoolManager.Despawn(((Component)this).gameObject);
	}
}
