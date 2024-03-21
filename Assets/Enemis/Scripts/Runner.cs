using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Runner : MonoBehaviour
{
    [Header("Health + Death |settings")]
    public float health;

    [Header("Aim |settings")]
    public Transform weapon;
    public Transform target;
    public float offset;

    

    void Start()
    {
        
    }

    
    void Update()
    {
        Vector3 displacement = weapon.position - target.position;
        float angle = -Mathf.Atan2(displacement.x, displacement.y) * Mathf.Rad2Deg;
        weapon.rotation = Quaternion.Euler(0, 0, angle + offset);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            health -= 1;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
