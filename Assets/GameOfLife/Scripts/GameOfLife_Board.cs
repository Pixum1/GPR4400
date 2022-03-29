using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife_Board : MonoBehaviour
{
    [Header("Unity Interaction")]
    [SerializeField]
    private GameObject mTilePrefab;
    [SerializeField]
    private Material mAliveMat;
    [SerializeField]
    private Material mDeathMat;
    private GameOfLife_Tile[,] tiles;

    [Header("Field Logic")]
    private int[,] map;
    [SerializeField]
    private int mWidth;
    [SerializeField]
    private int mHeight;
    [SerializeField]
    private bool mGenerateRandom;
    [SerializeField, Range(0, 1)]
    private float mRandomFillPercent;
    [SerializeField]
    private string mSeed;

    private enum ETileState
    {
        invalid = -1,
        dead,
        alive
    }

    private void Awake()
    {
        GenerateMap();
    }

    private void Start()
    {
        SpawnMap();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StepForwardInGame();
            UpdateTiles();
        }
    }

    private void GenerateMap()
    {
        int seed = 0;

        if (string.IsNullOrEmpty(mSeed) || string.IsNullOrWhiteSpace(mSeed))
            seed = (int)System.DateTime.Now.Ticks;
        else
            seed = mSeed.GetHashCode();

        System.Random prng = new System.Random(seed);
        map = new int[mWidth, mHeight];

        for (int y = 0; y < mHeight; y++)
        {
            for (int x = 0; x < mWidth; x++)
            {
                if (mGenerateRandom)
                {
                    float rngValue = (float)prng.NextDouble();
                    map[x, y] = rngValue <= mRandomFillPercent ? (int)ETileState.alive : (int)ETileState.dead;
                }
                else
                    map[x, y] = (int)ETileState.dead;
            }
        }
    }

    private void SpawnMap()
    {
        tiles = new GameOfLife_Tile[mWidth, mHeight];
        Vector3 startSpawnPosition = new Vector3(mWidth, 0, mHeight) * .5f * -1;

        for (int z = 0; z < mHeight; z++)
        {
            for (int x = 0; x < mWidth; x++)
            {
                GameObject newTile = Instantiate(mTilePrefab, this.transform);

                newTile.transform.localPosition = startSpawnPosition + new Vector3(x, 0, z);

                tiles[x, z] = newTile.GetComponent<GameOfLife_Tile>();
                tiles[x, z].Init(mAliveMat, mDeathMat, new Vector2Int(x, z), map[x, z] == (int)ETileState.alive);
                tiles[x, z].e_OnTileClick += OnTileClicked;
            }
        }
    }

    private void UpdateTiles()
    {
        for (int y = 0; y < mHeight; y++)
        {
            for (int x = 0; x < mWidth; x++)
            {
                tiles[x, y].UpdateTile(map[x, y] == (int)ETileState.alive);
            }
        }
    }

    private void StepForwardInGame()
    {
        int[,] newMap = new int[mWidth, mHeight];

        for (int y = 0; y < mHeight; y++)
        {
            for (int x = 0; x < mWidth; x++)
            {
                int neighbourCount = GetNeighbourCount(x, y);

                //Any alive cell with two or three neighbours stays alive
                if (map[x, y] == (int)ETileState.alive && (neighbourCount == 2 || neighbourCount == 3))
                    newMap[x, y] = (int)ETileState.alive;

                //Any dead cell with tree neighbours becomes alive
                else if (map[x, y] == (int)ETileState.dead && neighbourCount == 3)
                    newMap[x, y] = (int)ETileState.alive;

                //Any other cell will die or stay dead
                else
                    newMap[x, y] = (int)ETileState.dead;
            }
        }

        map = newMap;
    }

    private int GetNeighbourCount(int _x, int _y)
    {
        int count = 0;

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                int currX = _x + x;
                int currY = _y + y;

                //if in bounds
                if (currX >= 0 && currX < mWidth &&
                    currY >= 0 && currY < mHeight)
                {
                    //if not self
                    if (!(currX == _x && currY == _y))
                    {
                        if (map[currX, currY] == (int)ETileState.alive)
                            count++;
                    }
                }
            }
        }

        return count;
    }

    private void OnTileClicked(Vector2Int _tileIdx, bool _alive)
    {
        map[_tileIdx.x, _tileIdx.y] = _alive ? (int)ETileState.alive : (int)ETileState.dead;
    }
}
