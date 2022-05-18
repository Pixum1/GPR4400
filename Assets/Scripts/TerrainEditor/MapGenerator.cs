using UnityEngine;

public static class MapGenerator
{
    public static Chunk GenerateChunk(Vector3 _rootPos, float _scale, int _boidAmount, int _minCrateAmount, int _maxCrateAmount, TerrainData _data, GameObject[] _boidPrefabs, GameObject[] _lootCratePrefabs, Transform _parent)
    {
        GameObject newChunkObj = new GameObject($"Chunk {_rootPos.x} - {_rootPos.y} - {_rootPos.z}");
        newChunkObj.transform.SetParent(_parent);

        Chunk newChunk = newChunkObj.AddComponent<Chunk>();

        Mesh terrain = GenerateTerrain(_rootPos, _scale, _data);
        Mesh water = GenerateWater(_rootPos, _scale, _data);

        newChunk.InitGround(_rootPos, terrain, _data.TerrainMaterial);
        newChunk.InitWater(_rootPos, water, _data.WaterMaterial);

        if(Random.Range(0,1) <= 0.6f)
            newChunk.InitBoids(_rootPos, _scale, _boidAmount, _boidPrefabs);

        newChunk.InitLootCrates(_rootPos, _scale, _minCrateAmount, _maxCrateAmount, _lootCratePrefabs);

        return newChunk;
    }

    private static Mesh GenerateTerrain(Vector3 _rootPos, float _chunkScale, TerrainData _data)
    {
        float noiseScale = _data.NoiseScale;
        float noiseHeight = _data.NoiseHeight;
        int chunkRes = _data.TerrainResolution;
        int octaves = _data.Octaves;
        float persistence = _data.Persistence;
        float lacunarity = _data.Lacunarity;

        int seed = _data.Seed.GetHashCode();
        seed = Mathf.RoundToInt(seed / 100000);

        //Scale offset with resolution and world position
        float offsetX = (_rootPos.x / noiseScale) * ((chunkRes - 1) / 100f);
        float offsetZ = (_rootPos.z / noiseScale) * ((chunkRes - 1) / 100f);

        float[,] noiseMap = NoiseScript.GenerateNoiseMap(chunkRes, noiseScale, octaves, persistence, lacunarity, new Vector2(offsetX, offsetZ), seed);

        Vector3 meshStartPos = (new Vector3(_chunkScale, 0, _chunkScale) / 2) * -1;
        Vector3[] vertices = new Vector3[chunkRes * chunkRes];
        int[] triangles = new int[(chunkRes - 1) * (chunkRes - 1) * 2 * 3];

        int triIdx = 0;
        for (int y = 0, vertIdx = 0; y < chunkRes; y++)
        {
            for (int x = 0; x < chunkRes; x++, vertIdx++)
            {
                Vector2 percentualOffset = new Vector2(x, y) / (chunkRes - 1);

                Vector3 vertPosition = meshStartPos + new Vector3(percentualOffset.x, 0, percentualOffset.y) * _chunkScale;

                vertices[vertIdx] = vertPosition;

                if (x < chunkRes - 1 && y < chunkRes - 1)
                {
                    triangles[triIdx + 0] = triangles[triIdx + 3] = vertIdx;
                    triangles[triIdx + 1] = triangles[triIdx + 5] = vertIdx + chunkRes + 1;
                    triangles[triIdx + 2] = vertIdx + 1;
                    triangles[triIdx + 4] = vertIdx + chunkRes;

                    triIdx += 6;
                }

                vertices[vertIdx] = new Vector3(vertices[vertIdx].x, noiseMap[x, y] * noiseHeight, vertices[vertIdx].z);
                vertices[vertIdx] += _rootPos;
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = $"Ground: {_rootPos.x} | {_rootPos.y} | {_rootPos.z}";
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }

    private static Mesh GenerateWater(Vector3 _rootPos, float _chunkScale, TerrainData _data)
    {
        int chunkRes = _data.WaterResolution;

        Vector3 meshStartPos = (new Vector3(_chunkScale, 0, _chunkScale) / 2) * -1;
        Vector3[] vertices = new Vector3[chunkRes * chunkRes];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[(chunkRes - 1) * (chunkRes - 1) * 2 * 3];

        int triIdx = 0;
        for (int y = 0, vertIdx = 0; y < chunkRes; y++)
        {
            for (int x = 0; x < chunkRes; x++, vertIdx++)
            {
                Vector2 percentualOffset = new Vector2(x, y) / (chunkRes - 1);

                Vector3 vertPosition = meshStartPos + new Vector3(percentualOffset.x, 0, percentualOffset.y) * _chunkScale;

                vertices[vertIdx] = vertPosition;

                uvs[vertIdx] = new Vector2(vertices[vertIdx].x, vertices[vertIdx].z);

                if (x < chunkRes - 1 && y < chunkRes - 1)
                {
                    triangles[triIdx + 0] = triangles[triIdx + 3] = vertIdx;
                    triangles[triIdx + 1] = triangles[triIdx + 5] = vertIdx + chunkRes + 1;
                    triangles[triIdx + 2] = vertIdx + 1;
                    triangles[triIdx + 4] = vertIdx + chunkRes;

                    triIdx += 6;
                }

                vertices[vertIdx] += _rootPos;
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = $"Water: {_rootPos.x} | {_rootPos.y} | {_rootPos.z}";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        return mesh;
    }
}