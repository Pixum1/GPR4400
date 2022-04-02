using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [SerializeField]
    private float m_BoidAmount;
    [SerializeField]
    private GameObject m_BoidPrefab;
    [SerializeField]
    private float m_SpawnRadius;

    private void Start()
    {
        for (int i = 0; i < m_BoidAmount; i++)
        {
            GameObject g = Instantiate(m_BoidPrefab, transform);
            g.transform.position += Random.insideUnitSphere * m_SpawnRadius;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, m_SpawnRadius);
    }
}
