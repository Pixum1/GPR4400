using UnityEngine;

public static class NoiseScript
{
    public static float[,] GenerateNoiseMap(int _mapWidth, int _mapHeight, float _scale, int _octaves, float _persistance, float _lacunarity)
    {
        float[,] noiseMap = new float[_mapWidth, _mapHeight];

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int y = 0; y < _mapHeight; y++)
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;

                float noiseHeight = 0;

                //octaves
                for (int i = 0; i < _octaves; i++)
                {
                    float sampleX = x / _scale * frequency;
                    float sampleY = y / _scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    noiseHeight += perlinValue * amplitude;

                    amplitude *= _persistance;
                    frequency *= _lacunarity;
                }

                if(noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                if(noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noiseMap[y, x] = noiseHeight;
            }
        }
        for (int y = 0; y < _mapHeight; y++)
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                //normalize noiseMap
                noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x,y]);
            }
        }


        return noiseMap;
    }
}
