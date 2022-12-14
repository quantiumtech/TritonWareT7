using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScenePlayerMovement : MonoBehaviour
{
    // Movement
    [Header("Movement")]
    [SerializeField] private float speed = 3f;
    public Rigidbody rb;
    private Vector3 velocity;


    // Attacking
    [Header("Attacking")]
    public Transform attackPosition;
    public GameObject attackSprite;
    public float attackRadius;
    //public float attackDuration; - Might use later if we need attacking to be a longer thing
    //private bool isAttacking = false;
    

    // Sprites and Animation
    /*[Header("Sprite/Animation")]*/
    public SpriteRenderer sr;
    public Animator anim;


    void Start()
    {
        /*rb = GetComponent<Rigidbody>();
        sr= GetComponent<SpriteRenderer>();
        anim= GetComponent<Animator>();*/
    }


    void Update()
    {
        // Gets WASD Input
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.y = 0;
        velocity.z = Input.GetAxisRaw("Vertical");

        // Attacking
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        if (velocity.magnitude > 0.01)
        {
            anim.Play("Base Layer.Run");
        }
        else
        {
            anim.Play("Base Layer.Idle");

        }

        // Flips Sprite
        if (velocity.x<0)
        {
            sr.flipX = false;
            if(attackPosition)
                attackPosition.eulerAngles = new Vector3(attackPosition.eulerAngles.x, 180, attackPosition.eulerAngles.z);
        }
        else if (velocity.x>0)
        {
            sr.flipX = true;
            if(attackPosition)
                attackPosition.eulerAngles = new Vector3(attackPosition.eulerAngles.x, 0, attackPosition.eulerAngles.z);
        }
    }

    private void FixedUpdate()
    {
        // Moves player
        rb.velocity = velocity.normalized * speed;
    }

    private void Attack()
    {
        if (!attackSprite || !attackPosition)
            return;
        //isAttacking = true;

        // Sets the sword image on
        attackSprite.SetActive(true);

        // Gets all enemies within a certain circle at the attack pos
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius);

        // Gets their enemy scripts and does damage
        foreach (Collider2D col in colliders)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                col.gameObject.GetComponent<Enemy>();
            }
        }

        // Sets sword image off
        attackSprite.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        // Draws attack radius
        if(attackPosition)
            Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
