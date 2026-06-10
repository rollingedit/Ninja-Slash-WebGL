using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterStomp : MonoBehaviour
{
	private bool isStomping;

	private void OnStomp()
	{
		isStomping = true;
	}

	private void OnDead()
	{
		isStomping = false;
	}

	private void OnPutObjectIntoPool()
	{
		isStomping = false;
	}

	private void OnBounce()
	{
		isStomping = false;
	}

	private void OnGround()
	{
		if (isStomping)
		{
			isStomping = false;
			((Component)this).BroadcastMessage("OnTumble");
		}
		else
		{
			((Component)this).BroadcastMessage("OnLand");
		}
	}
}
