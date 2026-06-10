using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class MapManager : MonoSingleton<MapManager>
{
	public bool dontStartRandomConcept;

	public int startConceptIndex;

	public GameObject lastBlock;

	public Vector3 startBlockPos;

	public List<BackGroundConcept> conceptList;

	public BackGroundConcept tutorialConcept;

	public Material[] zombientMaterials;

	private BackGroundConcept curConcept;

	private int curBlockIndex;

	private float lastBlockLength;

	private int blockCreatedCount;

	private float farClipPlaneLength;

	private bool spawnBlockByOrder;

	private bool changeConceptFromNextBlock;

	private GameObject curTutorialBlock;

	private void Initialize()
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		if (MonoSingleton<UserData>.instance.IsTutorialDone)
		{
			if (dontStartRandomConcept)
			{
				curConcept = conceptList[startConceptIndex];
			}
			else
			{
				curConcept = conceptList[UnityEngine.Random.Range(0, conceptList.Count)];
			}
			spawnBlockByOrder = false;
		}
		else
		{
			curConcept = tutorialConcept;
			spawnBlockByOrder = true;
		}
		lastBlock = PoolManager.Spawn(curConcept.startBlock, startBlockPos);
		lastBlockLength = GetBlockLength(lastBlock);
		curBlockIndex = -1;
		blockCreatedCount = 0;
		changeConceptFromNextBlock = false;
		SetSkyboxAndFogColorAndSakura();
	}

	private void Start()
	{
		farClipPlaneLength = Camera.main.farClipPlane;
		Initialize();
	}

	private void Update()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (!(((Component)Camera.main).transform.position.z > lastBlock.transform.position.z + (lastBlockLength - farClipPlaneLength)))
		{
			return;
		}
		if (curConcept.type == ConceptType.tutorial)
		{
			GenerateNextBlock(curConcept.blockList);
			return;
		}
		if (changeConceptFromNextBlock)
		{
			BackGroundConcept backGroundConcept = conceptList[UnityEngine.Random.Range(0, conceptList.Count)];
			while (backGroundConcept.type == curConcept.type)
			{
				backGroundConcept = conceptList[UnityEngine.Random.Range(0, conceptList.Count)];
			}
			blockCreatedCount = 0;
			curConcept = backGroundConcept;
			curBlockIndex = -1;
			changeConceptFromNextBlock = false;
		}
		if (blockCreatedCount >= curConcept.conceptBlockCount)
		{
			GenerateNextBlock(curConcept.gateBlock);
			changeConceptFromNextBlock = true;
		}
		else if (blockCreatedCount == 0)
		{
			GenerateNextBlock(curConcept.easyBlockList);
		}
		else
		{
			GenerateNextBlock(curConcept.blockList);
		}
		blockCreatedCount++;
	}

	private float GetBlockLength(GameObject blockIn)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		return blockIn.transform.Find("anchor").position.z - blockIn.transform.position.z;
	}

	public void GenerateNextBlock(GameObject nextBlock)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = PoolManager.Spawn(nextBlock, lastBlock.transform.Find("anchor").position);
		lastBlock = val;
		lastBlockLength = GetBlockLength(lastBlock);
	}

	public void GenerateNextBlock(List<GameObject> blockList)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		if (spawnBlockByOrder)
		{
			curBlockIndex++;
			GameObject val = PoolManager.Spawn(blockList[curBlockIndex], lastBlock.transform.Find("anchor").position);
			lastBlock = val;
			lastBlockLength = GetBlockLength(lastBlock);
			return;
		}
		int num = UnityEngine.Random.Range(0, blockList.Count);
		while (curBlockIndex == num)
		{
			num = UnityEngine.Random.Range(0, blockList.Count);
		}
		GameObject val2 = PoolManager.Spawn(blockList[num], lastBlock.transform.Find("anchor").position);
		lastBlock = val2;
		lastBlockLength = GetBlockLength(lastBlock);
		curBlockIndex = num;
	}

	private void PoolAllExistingBlocks()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Block");
		GameObject[] array2 = array;
		foreach (GameObject go in array2)
		{
			PoolManager.Despawn(go);
		}
	}

	private void OnRestart()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (curConcept.type == ConceptType.tutorial)
		{
			Vector3 position = curTutorialBlock.transform.position;
			PoolManager.Despawn(curTutorialBlock);
			PoolManager.Spawn(curTutorialBlock, position);
		}
		else
		{
			ResetMap();
		}
	}

	private void OnShieldUsed()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		PoolAllExistingBlocks();
		lastBlock = PoolManager.Spawn(curConcept.startBlock, startBlockPos);
		lastBlockLength = GetBlockLength(lastBlock);
		changeConceptFromNextBlock = false;
	}

	private void OnHome()
	{
		ResetMap();
	}

	public void ResetMap()
	{
		PoolAllExistingBlocks();
		Initialize();
	}

	private void OnEnterTutorialBlock(GameObject blockObject)
	{
		curTutorialBlock = blockObject;
	}

	private void OnFinishTutorial()
	{
		MonoSingleton<UserData>.instance.IsTutorialDone = true;
		spawnBlockByOrder = false;
		curBlockIndex = -1;
		curConcept = conceptList[0];
	}

	private void OnEnterGateDoor()
	{
		SetSkyboxAndFogColorAndSakura();
	}

	private void SetSkyboxAndFogColorAndSakura()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		SkyboxFogColor skyboxFogColor = curConcept.skyboxFogColor[UnityEngine.Random.Range(0, curConcept.skyboxFogColor.Count)];
		RenderSettings.skybox = skyboxFogColor.skybox;
		RenderSettings.fogColor = skyboxFogColor.fogColor;
		RenderSettings.ambientLight = skyboxFogColor.ambientColor;
		Material[] array = zombientMaterials;
		foreach (Material val in array)
		{
			val.SetColor("_ZombientColor", skyboxFogColor.zombientColor);
		}
		if (Utility.IsGoodPerformance() && curConcept.type == ConceptType.village)
		{
			((Component)Camera.main).SendMessage("SetSakura", (object)true);
		}
		else
		{
			((Component)Camera.main).SendMessage("SetSakura", (object)false);
		}
	}
}
