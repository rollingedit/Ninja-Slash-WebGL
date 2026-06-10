using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class RankingElement : MonoBehaviour
{
	public UILabel nameLabel;

	public UILabel scoreLabel;

	public UITexture pictureTexture;

	public UILabel rankLabel;

	private void SetUser(FacebookUser userIn)
	{
		nameLabel.text = userIn.Name;
		scoreLabel.text = userIn.Score.ToString();
		if (long.Parse(userIn.UserId) > 0)
		{
			if ((Object)(object)userIn.Picture == (Object)null)
			{
				((MonoBehaviour)this).StartCoroutine("DownloadPicture", (object)userIn);
			}
			else
			{
				SetPicture(userIn.Picture);
			}
		}
	}

	private void SetRank(int rankIn)
	{
		rankLabel.text = rankIn.ToString();
	}

	private void SetPicture(Texture textureIn)
	{
		pictureTexture.mainTexture = textureIn;
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator DownloadPicture(FacebookUser userIn)
	{
		yield return userIn.Picture;
		SetPicture(userIn.Picture);
	}
}
