using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class BGMRandomPlayer : MonoBehaviour
{
	public AudioSource[] audioSourceList;

	public float cutInTime;

	public BGMClip curBGMClip;

	private int curAudioSourceIndex;

	private float leftTime;

	private float realTime;

	private void Start()
	{
		curAudioSourceIndex = 0;
		realTime = Time.realtimeSinceStartup;
		PlaySound();
	}

	private void Update()
	{
		if (leftTime > 0f)
		{
			leftTime = Mathf.Clamp(leftTime - (Time.realtimeSinceStartup - realTime), 0f, leftTime);
			if (leftTime == 0f)
			{
				GetRandomNextSong();
				PlaySound();
			}
		}
		realTime = Time.realtimeSinceStartup;
	}

	private void PlaySound()
	{
		audioSourceList[curAudioSourceIndex].clip = curBGMClip.BGM;
		audioSourceList[curAudioSourceIndex].Play();
		leftTime = curBGMClip.BGM.length - cutInTime;
	}

	private void GetRandomNextSong()
	{
		curBGMClip = curBGMClip.GetNextBGMClip();
		curAudioSourceIndex = (curAudioSourceIndex + 1) % 2;
	}
}
