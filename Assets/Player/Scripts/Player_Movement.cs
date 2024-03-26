using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Animator anim;
    public Animator atk_anim;

    [Header("Health + Death |settings")]
    public float health;
    public float invulnerable_cooldown;
    bool invulnerable = false;

    [Header("Basic movement |settings")]
    public float speed;
    private Vector2 move_input;
    

    [Header("Dash |settings")]
    public float dash_speed;
    public float dash_time;
    public float dash_cooldown;
    bool can_dash = true;
    bool is_dashing = false;

    [Header("Aim |settings")]
    public Transform weapon;
    public float offset;

    [Header("Atk |settings")]
    public CapsuleCollider2D atk_area;
    public float atk_time;
    public float atk_cooldown;
    bool can_atk = true;
    bool is_atk = false;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        atk_area.enabled = false;
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

        //AIM
        if (!is_atk)
        {
            Vector3 displacement = weapon.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = -Mathf.Atan2(displacement.x, displacement.y) * Mathf.Rad2Deg;
            weapon.rotation = Quaternion.Euler(0, 0, angle + offset);
            //AIM ANIM
            anim.SetFloat("angle", angle);
            //ATK ANGLE
            if (angle > 0)
            {
                atk_area.transform.localScale = new Vector3(2,1.5f,1);
            } else if (angle < 0)
            {
                atk_area.transform.localScale = new Vector3(2,-1.5f, 1);
            }
        }
        
        
        //ATK
        if(Input.GetKey(KeyCode.Mouse0) && can_atk)
        {
            StartCoroutine(Atk());
        }
    }

    private IEnumerator Dash()
    {
        can_dash = false;
        is_dashing = true;
        invulnerable = true;
        body.velocity = move_input * dash_speed;
        yield return new WaitForSeconds(dash_time);
        is_dashing = false;
        invulnerable = false;
        yield return new WaitForSeconds(dash_cooldown);
        can_dash = true;
    }

    private IEnumerator Atk()
    {
        can_atk = false;
        is_atk = true;
        atk_area.enabled = true;
        atk_anim.SetTrigger("atk");
        yield return new WaitForSeconds(atk_time);
        atk_area.enabled = false;
        is_atk = false;
        yield return new WaitForSeconds(atk_cooldown);
        can_atk = true;
    }

    private IEnumerator Dmg()
    {
        invulnerable = true;
        sprite.color = new Color(1,0,0,1);
        yield return new WaitForSeconds(invulnerable_cooldown);
        invulnerable = false;
        sprite.color = new Color(1, 1, 1, 1);
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon") && !invulnerable)
        {
            health -= 1;
            if (health > 0)
            {
                StartCoroutine(Dmg());
            } 
            else if (health <= 0)
            {
                Death();
            }
        }
    }
}

