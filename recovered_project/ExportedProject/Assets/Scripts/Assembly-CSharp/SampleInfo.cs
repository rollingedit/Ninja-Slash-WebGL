using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class SampleInfo : MonoBehaviour
{
	private void OnGUI()
	{
		GUILayout.Label("iTween can spin, shake, punch, move, handle audio, fade color and transparency \nand much more with each task needing only one line of code.", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("iTween works with C#, JavaScript and Boo. For full documentation and examples visit:", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		if (GUILayout.Button("http://itween.pixelplacement.com", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			Application.OpenURL("http://itween.pixelplacement.com");
		}
		GUILayout.EndHorizontal();
	}
}
