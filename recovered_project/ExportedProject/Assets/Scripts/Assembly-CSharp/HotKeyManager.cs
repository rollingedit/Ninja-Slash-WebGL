using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class HotKeyManager : MonoBehaviour
{
	private GameObject GetPlayer()
	{
		return GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		if (Input.GetKeyDown((KeyCode)122))
		{
			GetPlayer().SendMessage("OnSetSpeedToLowest");
		}
		if (Input.GetKeyDown((KeyCode)120))
		{
			GetPlayer().SendMessage("OnSetSpeedToHighest");
		}
	}
}
