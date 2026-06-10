using System;
using System.Collections.Generic;
using Prime31;

public class GameCenterChallenge
{
	public string issuingPlayerID;

	public string receivingPlayerID;

	public GameCenterChallengeState state;

	public global::System.DateTime issueDate;

	public global::System.DateTime completionDate;

	public string message;

	public uint hash;

	public GameCenterScore score;

	public GameCenterAchievement achievement;

	public GameCenterChallenge(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("issuingPlayerID"))
		{
			issuingPlayerID = dict["issuingPlayerID"] as string;
		}
		if (dict.ContainsKey("receivingPlayerID"))
		{
			receivingPlayerID = dict["receivingPlayerID"] as string;
		}
		if (dict.ContainsKey("state"))
		{
			int num = int.Parse(dict["state"].ToString());
			state = (GameCenterChallengeState)num;
		}
		if (dict.ContainsKey("issueDate"))
		{
			double num2 = double.Parse(dict["issueDate"].ToString());
			issueDate = new global::System.DateTime(1970, 1, 1, 0, 0, 0, (DateTimeKind)1).AddSeconds(num2);
		}
		if (dict.ContainsKey("completionDate"))
		{
			double num3 = double.Parse(dict["completionDate"].ToString());
			completionDate = new global::System.DateTime(1970, 1, 1, 0, 0, 0, (DateTimeKind)1).AddSeconds(num3);
		}
		if (dict.ContainsKey("message"))
		{
			message = dict["message"] as string;
		}
		if (dict.ContainsKey("score"))
		{
			score = new GameCenterScore(dict["score"] as Dictionary<string, object>);
		}
		if (dict.ContainsKey("achievement"))
		{
			achievement = new GameCenterAchievement(dict["achievement"] as Dictionary<string, object>);
		}
		if (dict.ContainsKey("hash"))
		{
			hash = uint.Parse(dict["hash"].ToString());
		}
	}

	public static List<GameCenterChallenge> fromJson(string json)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		List<object> val = json.listFromJson();
		List<GameCenterChallenge> val2 = new List<GameCenterChallenge>();
		var enumerator = val.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Dictionary<string, object> dict = (Dictionary<string, object>)enumerator.Current;
				val2.Add(new GameCenterChallenge(dict));
			}
			return val2;
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
	}

	public virtual string ToString()
	{
		return string.Format("<Challenge> issuingPlayerID: {0}, receivingPlayerID: {1}, message: {2}, state: {3}, score: {4}, achievement: {5}, hash: {6}", new object[7] { issuingPlayerID, receivingPlayerID, message, state, score, achievement, hash });
	}
}
