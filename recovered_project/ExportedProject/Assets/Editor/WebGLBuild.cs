using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class WebGLBuild
{
	public static void Build()
	{
		string outputPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../../../build_webgl/NinjaSlash_AssetRipper"));
		Directory.CreateDirectory(outputPath);

		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
		PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
		PlayerSettings.WebGL.decompressionFallback = false;

		// Diagnostic mode: NINJA_DEV_BUILD=1 produces a development build with
		// full C# exception stack traces in the browser console.
		bool devBuild = System.Environment.GetEnvironmentVariable("NINJA_DEV_BUILD") == "1";
		PlayerSettings.WebGL.exceptionSupport = devBuild
			? WebGLExceptionSupport.FullWithStacktrace
			: WebGLExceptionSupport.ExplicitlyThrownExceptionsOnly;
		BuildOptions options = devBuild ? BuildOptions.Development : BuildOptions.None;

		ForceRecoveredTextureAlpha();

		string[] scenes =
		{
			"Assets/scene/loadingScene.unity",
			"Assets/scene/gameScene.unity"
		};

		BuildReport report = BuildPipeline.BuildPlayer(scenes, outputPath, BuildTarget.WebGL, options);
		if (report.summary.result != BuildResult.Succeeded)
		{
			throw new System.Exception("WebGL build failed: " + report.summary.result);
		}

		PatchWebGLShell(outputPath);
	}

	private static void ForceRecoveredTextureAlpha()
	{
		string[] textureGuids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/Texture2D" });
		foreach (string guid in textureGuids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
			if (importer == null)
			{
				continue;
			}

			importer.alphaSource = TextureImporterAlphaSource.FromInput;
			importer.alphaIsTransparency = true;
			importer.mipmapEnabled = false;
			importer.textureCompression = TextureImporterCompression.Uncompressed;
			importer.SaveAndReimport();
		}
	}

	private static void PatchWebGLShell(string outputPath)
	{
		string indexPath = Path.Combine(outputPath, "index.html");
		string cssPath = Path.Combine(outputPath, "TemplateData", "style.css");
		if (File.Exists(indexPath))
		{
			string html = File.ReadAllText(indexPath);
			html = html.Replace("<div id=\"unity-footer\">\r\n        <div id=\"unity-logo-title-footer\"></div>\r\n        <div id=\"unity-fullscreen-button\"></div>\r\n        <div id=\"unity-build-title\">ninjaslash</div>\r\n      </div>", string.Empty);
			html = html.Replace("<div id=\"unity-footer\">\n        <div id=\"unity-logo-title-footer\"></div>\n        <div id=\"unity-fullscreen-button\"></div>\n        <div id=\"unity-build-title\">ninjaslash</div>\n      </div>", string.Empty);
			html = html.Replace("canvas.style.width = \"420px\";\r\n        canvas.style.height = \"560px\";", "document.querySelector(\"#unity-container\").className = \"unity-mobile\";\r\n        canvas.className = \"unity-mobile\";");
			html = html.Replace("canvas.style.width = \"420px\";\n        canvas.style.height = \"560px\";", "document.querySelector(\"#unity-container\").className = \"unity-mobile\";\n        canvas.className = \"unity-mobile\";");
			html = html.Replace("document.querySelector(\"#unity-fullscreen-button\").onclick = () => {\r\n                  unityInstance.SetFullscreen(1);\r\n                };\r\n", string.Empty);
			html = html.Replace("document.querySelector(\"#unity-fullscreen-button\").onclick = () => {\n                  unityInstance.SetFullscreen(1);\n                };\n", string.Empty);
			// Expose the instance for local testing tools (scripts/probe_session.js).
			html = html.Replace("}).then((unityInstance) => {",
				"}).then((unityInstance) => {\n              window.unityInstance = unityInstance;");
			File.WriteAllText(indexPath, html);
		}
		if (File.Exists(cssPath))
		{
			File.WriteAllText(cssPath,
@"html, body {
  width: 100%;
  height: 100%;
  padding: 0;
  margin: 0;
  overflow: hidden;
  background: #000;
}
#unity-container {
  position: fixed;
  inset: 0;
  width: 100%;
  height: 100%;
  transform: none;
}
#unity-canvas {
  display: block;
  width: 100%;
  height: 100%;
  background: #000;
}
#unity-loading-bar {
  position: absolute;
  left: 50%;
  top: 50%;
  transform: translate(-50%, -50%);
  display: none;
}
#unity-logo {
  width: 154px;
  height: 130px;
  background: url('unity-logo-dark.png') no-repeat center;
}
#unity-progress-bar-empty {
  width: 141px;
  height: 18px;
  margin-top: 10px;
  margin-left: 6.5px;
  background: url('progress-bar-empty-dark.png') no-repeat center;
}
#unity-progress-bar-full {
  width: 0%;
  height: 18px;
  margin-top: 10px;
  background: url('progress-bar-full-dark.png') no-repeat center;
}
#unity-footer,
#unity-logo-title-footer,
#unity-build-title,
#unity-fullscreen-button {
  display: none !important;
}
#unity-warning {
  position: absolute;
  left: 50%;
  top: 5%;
  transform: translate(-50%);
  background: white;
  padding: 10px;
  display: none;
}
");
		}
	}
}
