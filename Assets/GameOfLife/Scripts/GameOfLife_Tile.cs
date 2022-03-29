using System;
using System.Collections;
using UnityEngine;

public class GameOfLife_Tile : MonoBehaviour
{
    private Renderer tileRenderer;

    private Material aliveMat;
    private Material deathMat;

    private bool isAlive;
    private Vector2Int idx;

    public event Action<Vector2Int, bool> e_OnTileClick;

    void Awake()
    {
        tileRenderer = GetComponent<Renderer>();
    }

    internal void Init(Material _aliveMat, Material _deathMat, Vector2Int _idx, bool _isAlive)
    {
        aliveMat = _aliveMat;
        deathMat = _deathMat;

        idx = _idx;

        UpdateTile(_isAlive);
    }

    public void UpdateTile(bool _isAlive)
    {
        isAlive = _isAlive;
        tileRenderer.material = isAlive ? aliveMat : deathMat;
    }

    private void OnMouseDown()
    {
        UpdateTile(!isAlive);

        e_OnTileClick?.Invoke(idx, isAlive);
    }
}