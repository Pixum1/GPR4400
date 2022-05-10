using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    [SerializeField]
    private Renderer m_textureRenderer;

    public void DrawNoiseMap(float[,] _map)
    {
        int width = _map.GetLength(0);
        int height = _map.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colorMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[x + y * width] = Color.Lerp(Color.black, Color.white, _map[x, y]);
            }
        }

        texture.SetPixels(colorMap);
        texture.Apply();
        m_textureRenderer.sharedMaterial.mainTexture = texture;
        m_textureRenderer.transform.localScale = new Vector3(width, 1, height);
    }
}