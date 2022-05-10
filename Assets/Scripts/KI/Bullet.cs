using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float attackDamage;
    public LayerMask mask;
    private float timer = 2;

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        StateMachine enemySM = other.gameObject.GetComponent<StateMachine>();
        if (enemySM != null && enemySM != transform.GetComponentInParent<StateMachine>())
        {
            enemySM.health -= attackDamage;
            Destroy(gameObject);
        }
    }
}
