using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class FacebookUser
{
	private string userId;

	private string name;

	private int score;

	private bool isMe;

	public Texture picture;

	public string UserId
	{
		get
		{
			return userId;
		}
	}

	public string Name
	{
		get
		{
			return name;
		}
	}

	public int Score
	{
		get
		{
			return score;
		}
	}

	public bool IsMe
	{
		get
		{
			return isMe;
		}
	}

	public Texture Picture
	{
		get
		{
			return picture;
		}
		set
		{
			picture = value;
		}
	}

	public FacebookUser(string userIdIn, string nameIn, int scoreIn, bool isMeIn)
	{
		userId = userIdIn;
		name = nameIn;
		score = scoreIn;
		isMe = isMeIn;
	}

	public void UpdateScore(int scoreIn)
	{
		if (scoreIn > score)
		{
			score = scoreIn;
		}
	}
}
