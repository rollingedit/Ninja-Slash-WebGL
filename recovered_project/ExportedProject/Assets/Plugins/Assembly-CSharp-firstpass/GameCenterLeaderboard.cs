using System;
using System.Collections.Generic;
using Prime31;

public class GameCenterLeaderboard
{
	public string leaderboardId;

	public string title;

	public GameCenterLeaderboard(string leaderboardId, string title)
	{
		this.leaderboardId = leaderboardId;
		this.title = title;
	}

	public static List<GameCenterLeaderboard> fromJSON(string json)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		List<GameCenterLeaderboard> val = new List<GameCenterLeaderboard>();
		Dictionary<string, object> val2 = json.dictionaryFromJson();
		var enumerator = val2.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, object> current = enumerator.Current;
				val.Add(new GameCenterLeaderboard(current.Value as string, current.Key));
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
		return string.Format("<Leaderboard> leaderboardId: {0}, title: {1}", (object)leaderboardId, (object)title);
	}
}
