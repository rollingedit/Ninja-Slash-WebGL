using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class TutorialShieldStage : MonoBehaviour
{
	private void OnTriggerEnter(Collider collider)
	{
		((Component)collider).gameObject.SendMessage("OnEnterShieldTutorialBlock", (SendMessageOptions)1);
	}
}
