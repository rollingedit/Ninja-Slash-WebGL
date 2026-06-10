using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour
{
	[Serializable]
	public class Sprite
	{
		public string name = "Unity Bug";

		public Rect outer = new Rect(0f, 0f, 1f, 1f);

		public Rect inner = new Rect(0f, 0f, 1f, 1f);

		public float paddingLeft;

		public float paddingRight;

		public float paddingTop;

		public float paddingBottom;

		public bool hasPadding
		{
			get
			{
				return paddingLeft != 0f || paddingRight != 0f || paddingTop != 0f || paddingBottom != 0f;
			}
		}
	}

	public enum Coordinates
	{
		Pixels = 0,
		TexCoords = 1
	}

	[HideInInspector]
	[SerializeField]
	private Material material;

	[HideInInspector]
	[SerializeField]
	private List<Sprite> sprites = new List<Sprite>();

	[HideInInspector]
	[SerializeField]
	private Coordinates mCoordinates;

	[SerializeField]
	[HideInInspector]
	private float mPixelSize = 1f;

	[HideInInspector]
	[SerializeField]
	private UIAtlas mReplacement;

	public Material spriteMaterial
	{
		get
		{
			return (!((Object)(object)mReplacement != (Object)null)) ? material : mReplacement.spriteMaterial;
		}
		set
		{
			if ((Object)(object)mReplacement != (Object)null)
			{
				mReplacement.spriteMaterial = value;
				return;
			}
			if ((Object)(object)material == (Object)null)
			{
				material = value;
				return;
			}
			MarkAsDirty();
			material = value;
			MarkAsDirty();
		}
	}

	public List<Sprite> spriteList
	{
		get
		{
			return (!((Object)(object)mReplacement != (Object)null)) ? sprites : mReplacement.spriteList;
		}
		set
		{
			if ((Object)(object)mReplacement != (Object)null)
			{
				mReplacement.spriteList = value;
			}
			else
			{
				sprites = value;
			}
		}
	}

	public Texture texture
	{
		get
		{
			return ((Object)(object)mReplacement != (Object)null) ? mReplacement.texture : ((!((Object)(object)material != (Object)null)) ? null : material.mainTexture);
		}
	}

	public Coordinates coordinates
	{
		get
		{
			return (!((Object)(object)mReplacement != (Object)null)) ? mCoordinates : mReplacement.coordinates;
		}
		set
		{
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)mReplacement != (Object)null)
			{
				mReplacement.coordinates = value;
			}
			else
			{
				if (mCoordinates == value)
				{
					return;
				}
				if ((Object)(object)material == (Object)null || (Object)(object)material.mainTexture == (Object)null)
				{
					Debug.LogError((object)"Can't switch coordinates until the atlas material has a valid texture");
					return;
				}
				mCoordinates = value;
				Texture mainTexture = material.mainTexture;
				int i = 0;
				for (int count = sprites.Count; i < count; i++)
				{
					Sprite sprite = sprites[i];
					if (mCoordinates == Coordinates.TexCoords)
					{
						sprite.outer = NGUIMath.ConvertToTexCoords(sprite.outer, mainTexture.width, mainTexture.height);
						sprite.inner = NGUIMath.ConvertToTexCoords(sprite.inner, mainTexture.width, mainTexture.height);
					}
					else
					{
						sprite.outer = NGUIMath.ConvertToPixels(sprite.outer, mainTexture.width, mainTexture.height, true);
						sprite.inner = NGUIMath.ConvertToPixels(sprite.inner, mainTexture.width, mainTexture.height, true);
					}
				}
			}
		}
	}

	public float pixelSize
	{
		get
		{
			return (!((Object)(object)mReplacement != (Object)null)) ? mPixelSize : mReplacement.pixelSize;
		}
		set
		{
			if ((Object)(object)mReplacement != (Object)null)
			{
				mReplacement.pixelSize = value;
				return;
			}
			float num = Mathf.Clamp(value, 0.25f, 4f);
			if (mPixelSize != num)
			{
				mPixelSize = num;
				MarkAsDirty();
			}
		}
	}

	public UIAtlas replacement
	{
		get
		{
			return mReplacement;
		}
		set
		{
			UIAtlas uIAtlas = value;
			if ((Object)(object)uIAtlas == (Object)(object)this)
			{
				uIAtlas = null;
			}
			if ((Object)(object)mReplacement != (Object)(object)uIAtlas)
			{
				if ((Object)(object)uIAtlas != (Object)null && (Object)(object)uIAtlas.replacement == (Object)(object)this)
				{
					uIAtlas.replacement = null;
				}
				if ((Object)(object)mReplacement != (Object)null)
				{
					MarkAsDirty();
				}
				mReplacement = uIAtlas;
				MarkAsDirty();
			}
		}
	}

	public Sprite GetSprite(string name)
	{
		if ((Object)(object)mReplacement != (Object)null)
		{
			return mReplacement.GetSprite(name);
		}
		if (!string.IsNullOrEmpty(name))
		{
			int i = 0;
			for (int count = sprites.Count; i < count; i++)
			{
				Sprite sprite = sprites[i];
				if (!string.IsNullOrEmpty(sprite.name) && name == sprite.name)
				{
					return sprite;
				}
			}
		}
		else
		{
			Debug.LogWarning((object)"Expected a valid name, found nothing");
		}
		return null;
	}

	private static int CompareString(string a, string b)
	{
		return a.CompareTo(b);
	}

	public BetterList<string> GetListOfSprites()
	{
		if ((Object)(object)mReplacement != (Object)null)
		{
			return mReplacement.GetListOfSprites();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		for (int count = sprites.Count; i < count; i++)
		{
			Sprite sprite = sprites[i];
			if (sprite != null && !string.IsNullOrEmpty(sprite.name))
			{
				betterList.Add(sprite.name);
			}
		}
		return betterList;
	}

	public BetterList<string> GetListOfSprites(string match)
	{
		if ((Object)(object)mReplacement != (Object)null)
		{
			return mReplacement.GetListOfSprites(match);
		}
		if (string.IsNullOrEmpty(match))
		{
			return GetListOfSprites();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		for (int count = sprites.Count; i < count; i++)
		{
			Sprite sprite = sprites[i];
			if (sprite != null && !string.IsNullOrEmpty(sprite.name) && string.Equals(match, sprite.name, (StringComparison)5))
			{
				betterList.Add(sprite.name);
				return betterList;
			}
		}
		string[] array = match.Split(new char[1] { ' ' }, (StringSplitOptions)1);
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = array[j].ToLower();
		}
		int k = 0;
		for (int count2 = sprites.Count; k < count2; k++)
		{
			Sprite sprite2 = sprites[k];
			if (sprite2 == null || string.IsNullOrEmpty(sprite2.name))
			{
				continue;
			}
			string text = sprite2.name.ToLower();
			int num = 0;
			for (int l = 0; l < array.Length; l++)
			{
				if (text.Contains(array[l]))
				{
					num++;
				}
			}
			if (num == array.Length)
			{
				betterList.Add(sprite2.name);
			}
		}
		return betterList;
	}

	private bool References(UIAtlas atlas)
	{
		if ((Object)(object)atlas == (Object)null)
		{
			return false;
		}
		if ((Object)(object)atlas == (Object)(object)this)
		{
			return true;
		}
		return (Object)(object)mReplacement != (Object)null && mReplacement.References(atlas);
	}

	public static bool CheckIfRelated(UIAtlas a, UIAtlas b)
	{
		if ((Object)(object)a == (Object)null || (Object)(object)b == (Object)null)
		{
			return false;
		}
		return (Object)(object)a == (Object)(object)b || a.References(b) || b.References(a);
	}

	public void MarkAsDirty()
	{
		if ((Object)(object)mReplacement != (Object)null)
		{
			mReplacement.MarkAsDirty();
			return;
		}
		UISprite[] array = NGUITools.FindActive<UISprite>();
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			UISprite uISprite = array[i];
			if (CheckIfRelated(this, uISprite.atlas))
			{
				UIAtlas atlas = uISprite.atlas;
				uISprite.atlas = null;
				uISprite.atlas = atlas;
			}
		}
		UIFont[] array2 = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];
		int j = 0;
		for (int num2 = array2.Length; j < num2; j++)
		{
			UIFont uIFont = array2[j];
			if (CheckIfRelated(this, uIFont.atlas))
			{
				UIAtlas atlas2 = uIFont.atlas;
				uIFont.atlas = null;
				uIFont.atlas = atlas2;
			}
		}
		UILabel[] array3 = NGUITools.FindActive<UILabel>();
		int k = 0;
		for (int num3 = array3.Length; k < num3; k++)
		{
			UILabel uILabel = array3[k];
			if ((Object)(object)uILabel.font != (Object)null && CheckIfRelated(this, uILabel.font.atlas))
			{
				UIFont font = uILabel.font;
				uILabel.font = null;
				uILabel.font = font;
			}
		}
	}
}
