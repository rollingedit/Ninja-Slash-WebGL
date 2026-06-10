using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterKeyInput : MonoBehaviour
{
	private bool isEnabled;

	private void Update()
	{
		if (isEnabled)
		{
			if (Input.GetKeyDown((KeyCode)273) || Input.GetKeyDown((KeyCode)119))
			{
				((Component)this).SendMessage("OnDoJump");
			}
			if (Input.GetKeyDown((KeyCode)276) || Input.GetKeyDown((KeyCode)97))
			{
				((Component)this).SendMessage("OnDoLeftDash");
			}
			if (Input.GetKeyDown((KeyCode)275) || Input.GetKeyDown((KeyCode)100))
			{
				((Component)this).SendMessage("OnDoRightDash");
			}
			if (Input.GetKeyDown((KeyCode)274) || Input.GetKeyDown((KeyCode)115))
			{
				((Component)this).SendMessage("OnDoSlide");
			}
			if (Input.GetKeyDown((KeyCode)32))
			{
				((Component)this).SendMessage("OnDoShield");
			}
		}
	}

	private void OnPutObjectIntoPool()
	{
		isEnabled = false;
	}

	private void OnGameStart()
	{
		isEnabled = true;
	}

	private void OnDead()
	{
		isEnabled = false;
	}

	private void OnRestart()
	{
		isEnabled = true;
	}

	private void OnResume()
	{
		isEnabled = true;
	}
}
