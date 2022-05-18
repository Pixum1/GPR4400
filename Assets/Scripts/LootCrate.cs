using System.Collections;
using UnityEngine;

public class LootCrate : MonoBehaviour
{
    private bool up = true;

    private void Update()
    {
        if (transform.position.y >= Random.Range(.3f, .5f))
            up = false;
        if (transform.position.y <= Random.Range(-.3f, -.5f))
            up = true;

        if (up)
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, 1, transform.position.z), 1 * Time.deltaTime);
        else if(!up)
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, -1, transform.position.z), 1 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
            Destroy(gameObject);
    }
}