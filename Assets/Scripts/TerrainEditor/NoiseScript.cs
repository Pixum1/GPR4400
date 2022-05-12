using UnityEngine;

public static class NoiseScript
{
    public static float[,] GenerateNoiseMap(int _mapSize, float _scale, int _octaves, float _persistance, float _lacunarity, Vector2 _offset, int _seed)
    {
        float[,] noiseMap = new float[_mapSize, _mapSize];

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int y = 0; y < _mapSize; y++)
        {
            for (int x = 0; x < _mapSize; x++)
            {
                float amplitude = 1;
                float frequency = 1;

                float noiseHeight = 0;

                //octaves
                for (int i = 0; i < _octaves; i++)
                {
                    float sampleX = (x / _scale + (_offset.y  + _seed)) * frequency;
                    float sampleY = (y / _scale + (_offset.x  + _seed)) * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    noiseHeight += perlinValue * amplitude;

                    amplitude *= _persistance;
                    frequency *= _lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noiseMap[y, x] = noiseHeight;
            }
        }


        return noiseMap;
    }
}
