using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ChangeSprite : MonoBehaviour
{
	public UISprite uiSprite;

	public SpriteData originalSprite;

	private void Start()
	{
		SetSprite(originalSprite);
	}

	public void OnChangeSprite(SpriteData targetSprite)
	{
		SetSprite(targetSprite);
	}

	public void OnRestoreSprite()
	{
		SetSprite(originalSprite);
	}

	private void SetSprite(SpriteData targetSprite)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		uiSprite.spriteName = targetSprite.spriteName;
		uiSprite.color = targetSprite.spriteColor;
	}
}
