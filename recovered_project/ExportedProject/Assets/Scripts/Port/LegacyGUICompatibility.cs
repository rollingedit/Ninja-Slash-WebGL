using UnityEngine;

public sealed class GUITexture : MonoBehaviour
{
	public Texture texture;
	public Color color = Color.white;
	public Rect pixelInset;
}

public sealed class GUIText : MonoBehaviour
{
	public Material material;
}
