using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class Facebook : P31RestKit
{
	public string accessToken;

	public string appAccessToken;

	private static Facebook _instance;

	public static Facebook instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new Facebook();
			}
			return _instance;
		}
	}

	public Facebook()
	{
		_baseUrl = "https://graph.facebook.com/";
		forceJsonResponse = true;
	}

	protected override global::System.Collections.IEnumerator send(string path, HTTPVerb httpVerb, Dictionary<string, object> parameters, Action<string, object> onComplete)
	{
		if (parameters == null)
		{
			parameters = new Dictionary<string, object>();
		}
		if (!parameters.ContainsKey("access_token"))
		{
			parameters.Add("access_token", (object)accessToken);
		}
		return base.send(path, httpVerb, parameters, onComplete);
	}

	public void graphRequest(string path, Action<string, object> completionHandler)
	{
		get(path, null, completionHandler);
	}

	public void graphRequest(string path, HTTPVerb verb, Action<string, object> completionHandler)
	{
		graphRequest(path, verb, null, completionHandler);
	}

	public void graphRequest(string path, HTTPVerb verb, Dictionary<string, object> parameters, Action<string, object> completionHandler)
	{
		base.surrogateMonobehaviour.StartCoroutine(send(path, verb, parameters, completionHandler));
	}

	public void graphRequestBatch(global::System.Collections.Generic.IEnumerable<FacebookBatchRequest> requests, Action<string, object> completionHandler)
	{
		Dictionary<string, object> val = new Dictionary<string, object>();
		List<Dictionary<string, object>> val2 = new List<Dictionary<string, object>>();
		global::System.Collections.Generic.IEnumerator<FacebookBatchRequest> enumerator = requests.GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				FacebookBatchRequest current = enumerator.Current;
				val2.Add(current.requestDictionary());
			}
		}
		finally
		{
			if (enumerator != null)
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}
		val.Add("batch", (object)Json.jsonEncode(val2));
		base.surrogateMonobehaviour.StartCoroutine(send(string.Empty, HTTPVerb.POST, val, completionHandler));
	}

	public void postMessage(string message, Action<string, object> completionHandler)
	{
		Dictionary<string, object> val = new Dictionary<string, object>();
		val.Add("message", (object)message);
		Dictionary<string, object> parameters = val;
		post("me/feed", parameters, completionHandler);
	}

	public void postMessageWithLink(string message, string link, string linkName, Action<string, object> completionHandler)
	{
		Dictionary<string, object> val = new Dictionary<string, object>();
		val.Add("message", (object)message);
		val.Add("link", (object)link);
		val.Add("name", (object)linkName);
		Dictionary<string, object> parameters = val;
		post("me/feed", parameters, completionHandler);
	}

	public void postMessageWithLinkAndLinkToImage(string message, string link, string linkName, string linkToImage, string caption, Action<string, object> completionHandler)
	{
		Dictionary<string, object> val = new Dictionary<string, object>();
		val.Add("message", (object)message);
		val.Add("link", (object)link);
		val.Add("name", (object)linkName);
		val.Add("picture", (object)linkToImage);
		val.Add("caption", (object)caption);
		Dictionary<string, object> parameters = val;
		post("me/feed", parameters, completionHandler);
	}

	public void postImage(byte[] image, string message, Action<string, object> completionHandler)
	{
		Dictionary<string, object> val = new Dictionary<string, object>();
		val.Add("picture", (object)image);
		val.Add("message", (object)message);
		Dictionary<string, object> parameters = val;
		post("me/photos", parameters, completionHandler);
	}

	public void postImageToAlbum(byte[] image, string caption, string albumId, Action<string, object> completionHandler)
	{
		Dictionary<string, object> val = new Dictionary<string, object>();
		val.Add("picture", (object)image);
		val.Add("message", (object)caption);
		Dictionary<string, object> parameters = val;
		post(albumId, parameters, completionHandler);
	}

	public void getFriends(Action<string, object> completionHandler)
	{
		get("me/friends", completionHandler);
	}

	public void extendAccessToken(string appId, string appSecret, Action<global::System.DateTime?> completionHandler)
	{
		if (instance.accessToken == null)
		{
			Debug.LogError((object)"There is no access token to extend. The user must be autenticated before attempting to extend their access token");
			return;
		}
		Dictionary<string, object> val = new Dictionary<string, object>();
		val.Add("client_id", (object)appId);
		val.Add("client_secret", (object)appSecret);
		val.Add("grant_type", (object)"fb_exchange_token");
		val.Add("fb_exchange_token", (object)instance.accessToken);
		Dictionary<string, object> parameters = val;
		get("oauth/access_token", parameters, delegate(string error, object obj)
		{
			if (obj is string)
			{
				string text = obj as string;
				if (text.StartsWith("access_token="))
				{
					Dictionary<string, string> val2 = text.parseQueryString();
					instance.accessToken = val2["access_token"];
					double num = double.Parse(val2["expires"]);
					completionHandler.Invoke((global::System.DateTime?)global::System.DateTime.Now.AddSeconds(num));
				}
				else
				{
					Debug.LogError((object)("error extending access token: " + text));
					completionHandler.Invoke((global::System.DateTime?)null);
				}
			}
			else
			{
				Debug.LogError((object)("error extending access token: " + error));
				completionHandler.Invoke((global::System.DateTime?)null);
			}
		});
	}

	public void getAppAccessToken(string appId, string appSecret, Action<string> completionHandler)
	{
		Dictionary<string, object> val = new Dictionary<string, object>();
		val.Add("client_id", (object)appId);
		val.Add("client_secret", (object)appSecret);
		val.Add("grant_type", (object)"client_credentials");
		Dictionary<string, object> parameters = val;
		get("oauth/access_token", parameters, delegate(string error, object obj)
		{
			if (obj is string)
			{
				string text = obj as string;
				if (text.StartsWith("access_token="))
				{
					appAccessToken = text.Replace("access_token=", string.Empty);
					completionHandler.Invoke(appAccessToken);
				}
				else
				{
					completionHandler.Invoke((string)null);
				}
			}
			else
			{
				completionHandler.Invoke((string)null);
			}
		});
	}

	public void postScore(string userId, int score, Action<bool> completionHandler)
	{
		if (appAccessToken == null)
		{
			Debug.Log((object)"you must first retrieve the app access token before posting a score");
			completionHandler.Invoke(false);
			return;
		}
		if (userId == null)
		{
			Debug.Log((object)"a valid userId is required to post a score");
			completionHandler.Invoke(false);
			return;
		}
		string path = userId + "/scores";
		Dictionary<string, object> val = new Dictionary<string, object>();
		val.Add("score", (object)score.ToString());
		val.Add("app_access_token", (object)appAccessToken);
		val.Add("access_token", (object)appAccessToken);
		Dictionary<string, object> parameters = val;
		post(path, parameters, delegate(string error, object obj)
		{
			if (error == null && obj is bool)
			{
				bool flag = (bool)obj;
				completionHandler.Invoke(flag);
			}
			else
			{
				completionHandler.Invoke(false);
			}
		});
	}

	public void getScores(string userId, Action<string, object> onComplete)
	{
		string path = userId + "/scores";
		get(path, onComplete);
	}
}
