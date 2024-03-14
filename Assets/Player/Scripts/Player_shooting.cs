using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_shooting : MonoBehaviour
{
    public Transform weapon;
    public float offset;

    public Transform shot_point;
    public GameObject bullet;
    public float shot_cooldown;
    private float next_shot;

    void Start()
    {
        
    }

    
    void Update()
    {
        Vector3 displacement = weapon.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = -Mathf.Atan2(displacement.x, displacement.y) * Mathf.Rad2Deg;
        weapon.rotation = Quaternion.Euler(0f,0f,angle + offset);

        if(Input.GetMouseButton(0))
        {
            if(Time.time > next_shot)
            {
                next_shot = Time.time + shot_cooldown;
                Instantiate(bullet,shot_point.position,shot_point.rotation);
            }
        }
    }
}
