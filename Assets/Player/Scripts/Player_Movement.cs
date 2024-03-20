using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D body;

    [Header("Basic movement |settings")]
    private Vector2 move_input;
    public float speed;

    [Header("Dash |settings")]
    public float dash_speed;
    public float dash_time;
    public float dash_cooldown;
    bool can_dash = true;
    bool is_dashing = false;

    [Header("Atk |settings")]
    public Transform weapon;
    public float offset;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (is_dashing) { return; }
        // BASIC MOVEMENT
        move_input.y = Input.GetAxisRaw("Vertical");
        move_input.x = Input.GetAxisRaw("Horizontal");
        move_input.Normalize();
        body.velocity = move_input * speed;

        // DASH
        if (Input.GetKeyDown(KeyCode.Space) && can_dash)
        {
            StartCoroutine(Dash());
        }

        //ATK
        Vector3 displacement = weapon.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = - Mathf.Atan2(displacement.x, displacement.y) * Mathf.Rad2Deg;
        weapon.rotation = Quaternion.Euler(0, 0, angle + offset);

    }

    private IEnumerator Dash()
    {
        can_dash = false;
        is_dashing = true;
        body.velocity = move_input * dash_speed;
        yield return new WaitForSeconds(dash_time);
        is_dashing = false;
        yield return new WaitForSeconds(dash_cooldown);
        can_dash = true;
    }
}

