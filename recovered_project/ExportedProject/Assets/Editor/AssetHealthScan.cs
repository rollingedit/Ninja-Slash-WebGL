// TEMP DIAGNOSTIC: scans all prefabs for silent import failures (BrokenPrefabAsset etc.)
using System.Text;
using UnityEditor;
using UnityEngine;

public static class AssetHealthScan
{
	public static void Run()
	{
		var sb = new StringBuilder();
		string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
		int broken = 0, ok = 0;
		foreach (string guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			var type = AssetDatabase.GetMainAssetTypeAtPath(path);
			var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
			if (type == null || go == null || type.Name != "GameObject")
			{
				broken++;
				sb.AppendLine("[SCAN-BROKEN] " + path + " mainType=" + (type != null ? type.Name : "NULL") + " load=" + (go ? "ok" : "NULL"));
			}
			else ok++;
		}
		sb.AppendLine("[SCAN] prefabs ok=" + ok + " broken=" + broken);
		Debug.Log(sb.ToString());
		System.IO.Directory.CreateDirectory("Temp");
		System.IO.File.WriteAllText("Temp/asset_health.txt", sb.ToString());
		EditorApplication.Exit(0);
	}
}
