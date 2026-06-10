using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class BackButton : MonoBehaviour
{
	public GameObject targetWindow;

	private void OnClick()
	{
		targetWindow.SendMessage("OnBackButtonClicked");
	}
}
