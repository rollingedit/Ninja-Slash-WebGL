using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterToken : MonoBehaviour
{
	public UILabel uiLabel;

	private void OnSetNinjaPanelCell(Ninja ninja)
	{
		uiLabel.text = string.Concat((object)ninja.curToken, (object)"/", (object)ninja.requiredToken);
	}
}
