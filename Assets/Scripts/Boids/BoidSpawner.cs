using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [SerializeField]
    private float m_BoidAmount;
    [SerializeField]
    private Boid m_BoidPrefab;
    [SerializeField]
    private float m_SpawnRadius;

    private void Start()
    {
        for (int i = 0; i < m_BoidAmount; i++)
        {
            Boid g = Instantiate(m_BoidPrefab, transform);
            g.transform.position += RandomPointInBox(Vector3.one * m_SpawnRadius);
        }
    }
    private Vector3 RandomPointInBox(Vector3 size)
    {

        return new Vector3(
           (Random.value - 0.5f) * size.x,
           (Random.value - 0.5f) * size.y,
           (Random.value - 0.5f) * size.z
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, Vector3.one * m_SpawnRadius);
    }
}
