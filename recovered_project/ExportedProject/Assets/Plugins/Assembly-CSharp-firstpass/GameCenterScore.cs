using System;
using System.Collections.Generic;
using Prime31;

public class GameCenterScore
{
	public string category;

	public string formattedValue;

	public long value;

	public long context;

	public global::System.DateTime date;

	public string playerId;

	public int rank;

	public bool isFriend;

	public string alias;

	public int maxRange;

	public GameCenterScore(Dictionary<string, object> ht)
	{
		if (ht.ContainsKey("category"))
		{
			category = ht["category"] as string;
		}
		if (ht.ContainsKey("formattedValue"))
		{
			formattedValue = ht["formattedValue"] as string;
		}
		if (ht.ContainsKey("value"))
		{
			value = long.Parse(ht["value"].ToString());
		}
		if (ht.ContainsKey("context"))
		{
			context = long.Parse(ht["context"].ToString());
		}
		if (ht.ContainsKey("playerId"))
		{
			playerId = ht["playerId"] as string;
		}
		if (ht.ContainsKey("rank"))
		{
			rank = int.Parse(ht["rank"].ToString());
		}
		if (ht.ContainsKey("isFriend"))
		{
			isFriend = (bool)ht["isFriend"];
		}
		if (ht.ContainsKey("alias"))
		{
			alias = ht["alias"] as string;
		}
		else
		{
			alias = "Anonymous";
		}
		if (ht.ContainsKey("maxRange"))
		{
			maxRange = int.Parse(ht["maxRange"].ToString());
		}
		if (ht.ContainsKey("date"))
		{
			double num = double.Parse(ht["date"].ToString());
			date = new global::System.DateTime(1970, 1, 1, 0, 0, 0, (DateTimeKind)1).AddSeconds(num);
		}
	}

	public static List<GameCenterScore> fromJSON(string json)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		List<GameCenterScore> val = new List<GameCenterScore>();
		List<object> val2 = json.listFromJson();
		var enumerator = val2.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Dictionary<string, object> ht = (Dictionary<string, object>)enumerator.Current;
				val.Add(new GameCenterScore(ht));
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
		return string.Format("<Score> category: {0}, formattedValue: {1}, date: {2}, rank: {3}, alias: {4}, maxRange: {5}, value: {6}, context: {7}", new object[8] { category, formattedValue, date, rank, alias, maxRange, value, context });
	}
}
