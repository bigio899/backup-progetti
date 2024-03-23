using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected int damage;
    [SerializeField] protected float cooldown;
    protected GameObject player;
    protected Animator anim;
    protected Rigidbody rb;
    protected float distance;
    protected float timer;
    bool dead = false;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (!dead)
        {
            Attack();
        }
    }

    public virtual void Move()
    {
    }

    public virtual void Attack()
    {
    }

    private void FixedUpdate()
    {
        if (!dead)
        {
            Move();
        }
    }

    public void ChangeHealth(int count)
    {
        // sottrazione della salute
        health -= count;
        // se la salute è pari o inferiore a zero, allora...
        if (health <= 0)
        {
            // cambiare il valore della variabile di morte, il che significa che le chiamate alle funzioni Attack e Move non funzioneranno più.
            dead = true;
            // disabilitare il collisore del nemico
            GetComponent<Collider>().enabled = false;
            // abilitazione dell'animazione della morte
            anim.SetBool("Die", true);
        }
    }
}
