using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class FriendScore : MonoBehaviour
{
	public UILabel scoreLabel;

	public UITexture pictureTexture;

	public Texture defaultTexture;

	public TweenPosition tweenPosition;

	private List<FacebookUser> users;

	private FacebookUser currentTarget;

	private int score;

	private bool isGameStarted;

	private Vector3 originalPosition;

	private void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		originalPosition = ((Component)this).transform.position;
	}

	private void OnGetFacebookRank(List<FacebookUser> usersIn)
	{
		users = usersIn;
		if (isGameStarted)
		{
			SearchTarget();
		}
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator WaitAndSwitchTarget(FacebookUser userIn)
	{
		yield return (object)new WaitForSeconds(tweenPosition.duration);
		currentTarget = userIn;
		tweenPosition.Play(true);
		scoreLabel.text = currentTarget.Score.ToString();
		yield return currentTarget.Picture;
		if ((Object)(object)currentTarget.Picture != (Object)null)
		{
			pictureTexture.mainTexture = currentTarget.Picture;
		}
		else
		{
			pictureTexture.mainTexture = defaultTexture;
		}
	}

	private void SwitchTarget(FacebookUser userIn)
	{
		if (currentTarget != null)
		{
			tweenPosition.Play(false);
		}
		((MonoBehaviour)this).StartCoroutine("WaitAndSwitchTarget", (object)userIn);
	}

	private void SearchTarget()
	{
		bool flag = false;
		if (users != null && users.Count > 0)
		{
			for (int num = users.Count - 1; num >= 0; num--)
			{
				if (users[num].Score > score)
				{
					SwitchTarget(users[num]);
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			Debug.Log((object)"not found");
			currentTarget = null;
			tweenPosition.Play(false);
		}
	}

	private void OnGetScore(int scoreIn)
	{
		if (currentTarget != null && scoreIn > currentTarget.Score)
		{
			score = scoreIn;
			SearchTarget();
		}
	}

	private void MoveOut()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		isGameStarted = false;
		score = 0;
		((Component)this).transform.position = originalPosition;
	}

	private void OnGameOver(GameOverResult resultIn)
	{
		MoveOut();
	}

	private void OnGameStart()
	{
		isGameStarted = true;
		SearchTarget();
	}

	private void OnHome()
	{
		MoveOut();
	}
}
