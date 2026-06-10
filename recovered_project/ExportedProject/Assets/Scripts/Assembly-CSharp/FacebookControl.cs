using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class FacebookControl : MonoBehaviour
{
	public string inviteMessage;

	public string recommendMessage;

	private List<FacebookUser> friends;

	private string userId;

	private string appId = "144025099094796";

	private FacebookUser me;

	public List<FacebookUser> Friends
	{
		get
		{
			return friends;
		}
	}

	private void OnFacebookInvite()
	{
	}

	private void OnFacebookRecommend()
	{
	}

	private void SortFriends()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		friends.Sort((Comparison<FacebookUser>)((FacebookUser first, FacebookUser next) => next.Score.CompareTo(first.Score)));
		var enumerator = friends.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				FacebookUser current = enumerator.Current;
				Debug.Log((object)string.Concat((object)current.UserId, (object)":", (object)current.Score));
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
	}

	private void Start()
	{
		friends = new List<FacebookUser>();
	}

	private void MakeDefaultUser()
	{
		userId = "-1";
		AddFriend("-1", "me", MonoSingleton<UserData>.instance.HighScore);
	}

	private void MakeDummyFriends()
	{
		AddFriend("-2", "ddong", 100);
		AddFriend("-3", "ddong", 200);
		AddFriend("-4", "ddong", 300);
		SortFriends();
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator DownloadPicture(FacebookUser userIn)
	{
		WWW www = new WWW("http://graph.facebook.com/" + userIn.UserId + "/picture");
		yield return www;
		if (www.error == null)
		{
			userIn.Picture = (Texture)(object)www.texture;
		}
		else
		{
			userIn.Picture = null;
		}
	}

	private void AddFriend(string id, string name, int score)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		var enumerator = friends.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				FacebookUser current = enumerator.Current;
				if (id == current.UserId)
				{
					current.UpdateScore(score);
					return;
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
		bool flag = userId == id;
		FacebookUser facebookUser = new FacebookUser(id, name, score, flag);
		((MonoBehaviour)this).StartCoroutine("DownloadPicture", (object)facebookUser);
		if (flag)
		{
			facebookUser.UpdateScore(MonoSingleton<UserData>.instance.HighScore);
			MonoSingleton<UserData>.instance.HighScore = facebookUser.Score;
			me = facebookUser;
		}
		friends.Add(facebookUser);
	}

	private void OnGameOver(GameOverResult resultIn)
	{
		bool isHighScore = resultIn.IsHighScore;
		bool flag = isHighScore;
		if (me != null && resultIn.Score > me.Score)
		{
			flag = true;
		}
		if (isHighScore && me != null)
		{
			me.UpdateScore(resultIn.Score);
			SortFriends();
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnGetFacebookRankEvent(friends));
		}
	}
}
