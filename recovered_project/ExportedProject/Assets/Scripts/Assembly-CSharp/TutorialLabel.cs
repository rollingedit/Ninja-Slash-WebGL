using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class TutorialLabel : MonoBehaviour
{
	public string mobileText;

	public string webText;

	public UILabel label;

	private void Awake()
	{
		string text = null;
		text = ((!Utility.IsMobile()) ? webText : mobileText);
		label.text = text;
	}
}
