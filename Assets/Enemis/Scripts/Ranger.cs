using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ranger : MonoBehaviour
{
    [Header("Health + Death |settings")]
    public float health;

    [Header("Follow |settings")]
    public GameObject target;
    public float range;
    public float speed;
    private float distance;

    [Header("Aim |settings")]
    public Transform weapon;
    public float offset;

    [Header("Atk |settings")]
    public Transform shot_point;
    public GameObject bullet;
    public float atk_range;
    public float atk_cooldown;
    public float atk_time;
    bool can_atk = true;
    bool is_atk = false;

    void Update()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);
        Vector2 direction = target.transform.position - transform.position;
        if (distance < range && distance > atk_range && !is_atk)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }

        if (!is_atk)
        {
            Vector3 displacement = weapon.position - target.transform.position;
            float angle = -Mathf.Atan2(displacement.x, displacement.y) * Mathf.Rad2Deg;
            weapon.rotation = Quaternion.Euler(0, 0, angle + offset);
        }

        if (distance < atk_range && can_atk)
        {
            StartCoroutine(Atk());
        }

        //DEATH
        if (health <= 0)
        {
            Death();
        }
    }

    private IEnumerator Atk()
    {
        is_atk = true;
        can_atk = false;
        yield return new WaitForSeconds(atk_time);
        Instantiate(bullet, shot_point.position, shot_point.rotation);
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
        }
        else if (other.CompareTag("Explosion"))
        {
            health -= 100;
        }
    }
}
