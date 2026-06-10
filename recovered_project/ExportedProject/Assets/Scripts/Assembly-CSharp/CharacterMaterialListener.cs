using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterMaterialListener : MonoBehaviour
{
	public Material materialToChange;

	private Material originalMaterial;

	private bool isShieldOn;

	private void OnShieldStart()
	{
		if (!isShieldOn)
		{
			isShieldOn = true;
			originalMaterial = GetComponent<Renderer>().material;
			GetComponent<Renderer>().material = materialToChange;
		}
	}

	private void OnShieldFinish()
	{
		GetComponent<Renderer>().material = originalMaterial;
		isShieldOn = false;
	}
}
