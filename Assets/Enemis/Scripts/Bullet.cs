using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header(". |settings")]
    public float speed;
    public float atk_time;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        StartCoroutine(Bullet_time());
    }

    private IEnumerator Bullet_time()
    {
        yield return new WaitForSeconds(atk_time);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Weapon"))
        {
            Destroy(gameObject);
        }
    }
}
