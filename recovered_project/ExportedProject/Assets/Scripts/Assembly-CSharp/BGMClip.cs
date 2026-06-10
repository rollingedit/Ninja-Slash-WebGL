using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class BGMClip : MonoBehaviour
{
	public AudioClip BGM;

	public BGMClip[] nextBGMClips;

	public BGMClip GetNextBGMClip()
	{
		return nextBGMClips[UnityEngine.Random.Range(0, nextBGMClips.Length)];
	}
}
