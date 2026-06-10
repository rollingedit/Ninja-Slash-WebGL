using System;
using System.Collections.Generic;
using Prime31;

public class GameCenterAchievementMetadata
{
	public string identifier;

	public string description;

	public string unachievedDescription;

	public bool isHidden;

	public int maximumPoints;

	public string title;

	public GameCenterAchievementMetadata(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("identifier"))
		{
			identifier = dict["identifier"] as string;
		}
		if (dict.ContainsKey("achievedDescription"))
		{
			description = dict["achievedDescription"] as string;
		}
		if (dict.ContainsKey("unachievedDescription"))
		{
			unachievedDescription = dict["unachievedDescription"] as string;
		}
		if (dict.ContainsKey("hidden"))
		{
			isHidden = (bool)dict["hidden"];
		}
		if (dict.ContainsKey("maximumPoints"))
		{
			maximumPoints = int.Parse(dict["maximumPoints"].ToString());
		}
		if (dict.ContainsKey("title"))
		{
			title = dict["title"] as string;
		}
	}

	public static List<GameCenterAchievementMetadata> fromJSON(string json)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		List<GameCenterAchievementMetadata> val = new List<GameCenterAchievementMetadata>();
		List<object> val2 = json.listFromJson();
		var enumerator = val2.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Dictionary<string, object> dict = (Dictionary<string, object>)enumerator.Current;
				val.Add(new GameCenterAchievementMetadata(dict));
			}
			return val;
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
	}

	public virtual string ToString()
	{
		return string.Format("<AchievementMetaData> identifier: {0}, hidden: {1}, maxPoints: {2}, title: {3} desc: {4}, unachDesc: {5}", new object[6] { identifier, isHidden, maximumPoints, title, description, unachievedDescription });
	}
}
