using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Internal/Localization")]
public class Localization : MonoBehaviour
{
	private static Localization mInst;

	public string startingLanguage;

	public TextAsset[] languages;

	private Dictionary<string, string> mDictionary = new Dictionary<string, string>();

	private string mLanguage;

	public static Localization instance
	{
		get
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			if ((Object)(object)mInst == (Object)null)
			{
				mInst = Object.FindObjectOfType(typeof(Localization)) as Localization;
				if ((Object)(object)mInst == (Object)null)
				{
					GameObject val = new GameObject("_Localization");
					Object.DontDestroyOnLoad((Object)(object)val);
					mInst = val.AddComponent<Localization>();
				}
			}
			return mInst;
		}
	}

	public string currentLanguage
	{
		get
		{
			if (string.IsNullOrEmpty(mLanguage))
			{
				currentLanguage = PlayerPrefs.GetString("Language");
				if (string.IsNullOrEmpty(mLanguage))
				{
					currentLanguage = startingLanguage;
					if (string.IsNullOrEmpty(mLanguage) && languages != null && languages.Length > 0)
					{
						currentLanguage = ((Object)languages[0]).name;
					}
				}
			}
			return mLanguage;
		}
		set
		{
			if (!(mLanguage != value))
			{
				return;
			}
			startingLanguage = value;
			if (!string.IsNullOrEmpty(value))
			{
				if (languages != null)
				{
					int i = 0;
					for (int num = languages.Length; i < num; i++)
					{
						TextAsset val = languages[i];
						if ((Object)(object)val != (Object)null && ((Object)val).name == value)
						{
							Load(val);
							return;
						}
					}
				}
				Object obj = Resources.Load(value, typeof(TextAsset));
				TextAsset val2 = (TextAsset)(object)((obj is TextAsset) ? obj : null);
				if ((Object)(object)val2 != (Object)null)
				{
					Load(val2);
					return;
				}
			}
			mDictionary.Clear();
			PlayerPrefs.DeleteKey("Language");
		}
	}

	private void Awake()
	{
		if ((Object)(object)mInst == (Object)null)
		{
			mInst = this;
			Object.DontDestroyOnLoad((Object)(object)((Component)this).gameObject);
		}
		else
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
	}

	private void Start()
	{
		if (!string.IsNullOrEmpty(startingLanguage))
		{
			currentLanguage = startingLanguage;
		}
	}

	private void OnEnable()
	{
		if ((Object)(object)mInst == (Object)null)
		{
			mInst = this;
		}
	}

	private void OnDestroy()
	{
		if ((Object)(object)mInst == (Object)(object)this)
		{
			mInst = null;
		}
	}

	private void Load(TextAsset asset)
	{
		mLanguage = ((Object)asset).name;
		PlayerPrefs.SetString("Language", mLanguage);
		ByteReader byteReader = new ByteReader(asset);
		mDictionary = byteReader.ReadDictionary();
		UIRoot.Broadcast("OnLocalize", this);
	}

	public string Get(string key)
	{
		string text = default(string);
		return (!mDictionary.TryGetValue(key, out text)) ? key : text;
	}
}
