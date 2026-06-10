using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class RankingBoard : MonoBehaviour
{
	public GameObject facebookButton;

	public GameObject appstoreLink;

	private void Awake()
	{
		if (Utility.IsMobile())
		{
			facebookButton.SetActive(true);
			appstoreLink.SetActive(true);
		}
		else
		{
			facebookButton.SetActive(false);
			appstoreLink.SetActive(true);
		}
	}

	private void OnGetFacebookRank(List<FacebookUser> usersIn)
	{
		if (usersIn.Count > 0)
		{
			facebookButton.SetActive(false);
		}
	}
}
