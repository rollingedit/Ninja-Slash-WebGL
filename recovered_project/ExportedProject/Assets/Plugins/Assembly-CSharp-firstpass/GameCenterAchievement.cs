using System;
using System.Collections.Generic;
using Prime31;

public class GameCenterAchievement
{
	public string identifier;

	public bool isHidden;

	public bool completed;

	public global::System.DateTime lastReportedDate;

	public float percentComplete;

	public GameCenterAchievement(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("identifier"))
		{
			identifier = dict["identifier"] as string;
		}
		if (dict.ContainsKey("hidden"))
		{
			isHidden = (bool)dict["hidden"];
		}
		if (dict.ContainsKey("completed"))
		{
			completed = (bool)dict["completed"];
		}
		if (dict.ContainsKey("percentComplete"))
		{
			percentComplete = float.Parse(dict["percentComplete"].ToString());
		}
		if (dict.ContainsKey("lastReportedDate"))
		{
			double num = double.Parse(dict["lastReportedDate"].ToString());
			lastReportedDate = new global::System.DateTime(1970, 1, 1, 0, 0, 0, (DateTimeKind)1).AddSeconds(num);
		}
	}

	public static List<GameCenterAchievement> fromJSON(string json)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		List<GameCenterAchievement> val = new List<GameCenterAchievement>();
		List<object> val2 = json.listFromJson();
		var enumerator = val2.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Dictionary<string, object> dict = (Dictionary<string, object>)enumerator.Current;
				val.Add(new GameCenterAchievement(dict));
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
		return string.Format("<Achievement> identifier: {0}, hidden: {1}, completed: {2}, percentComplete: {3}, lastReported: {4}", new object[5] { identifier, isHidden, completed, percentComplete, lastReportedDate });
	}
}
