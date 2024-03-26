using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    [Header("Health + Death |settings")]
    public float health;

    [Header("Follow |settings")]
    public GameObject target;
    public float range;
    public float speed;
    private float distance;

    [Header("Atk |settings")]
    public CircleCollider2D atk_area;
    public float atk_range;
    public float atk_cooldown;
    public float atk_time;
    bool can_atk = true;
    bool is_atk = false;

    void Start()
    {
        atk_area.enabled = false;
    }
    
    void Update()
    {
        //FOLLOW
        distance = Vector2.Distance(transform.position, target.transform.position);
        Vector2 direction = target.transform.position - transform.position;
        if (distance < range && !is_atk)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }

        //ATK
        if(distance < atk_range && !is_atk && can_atk)
        {
            StartCoroutine(Atk());
        }
    }

    private IEnumerator Atk()
    {
        is_atk = true;
        can_atk = false;
        yield return new WaitForSeconds(atk_cooldown);
        atk_area.enabled = true;
        yield return new WaitForSeconds(atk_time);
        atk_area.enabled = false;
        is_atk = false;
        yield return new WaitForSeconds(atk_cooldown);
        can_atk = true;
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            health -= 1;
            if (health <= 0)
            {
                Death();
            }
        }
    }
}
