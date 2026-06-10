using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class DojoPanelButton : MonoBehaviour
{
	public DojoPanelType panelType;

	public GameObject selectedMark;

	private void OnClick()
	{
		((Component)this).SendMessageUpwards("OnClickButton", (object)panelType);
	}

	public void OnSetDojoPanelButton(DojoPanelType panelTypeIn)
	{
		if ((Object)(object)selectedMark == (Object)null)
		{
			return;
		}
		if (panelType == panelTypeIn)
		{
			selectedMark.SetActive(true);
		}
		else
		{
			selectedMark.SetActive(false);
		}
	}
}
