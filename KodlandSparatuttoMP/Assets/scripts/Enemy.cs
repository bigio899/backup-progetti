using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Enemy : MonoBehaviourPunCallbacks
{
    [SerializeField] protected int health;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected int damage;
    [SerializeField] protected float cooldown;
    protected Animator anim;
    protected Rigidbody rb;
    protected float distance;
    protected float timer;
    bool dead = false;
    protected GameObject[] players;
    public GameObject player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        CheckPlayers();
    }


    public virtual void Move()
    {
    }

    public virtual void Attack()
    {
    }

    private void Update()
    {
        //Dichiarare una variabile che memorizzerà la distanza minima
        //Mathf.Infinity - infinito positivo
        float closestDistance = Mathf.Infinity;
        //Scorrendo l'elenco dei giocatori
        foreach (GameObject closestPlayer in players)
        {
            //Calcolo della distanza tra il nemico e il giocatore
            float checkDistance = Vector3.Distance(closestPlayer.transform.position, transform.position);
            //Se la distanza da questo giocatore è inferiore alla distanza dal giocatore precedente, allora....
            if (checkDistance < closestDistance)
            {
                //Se il giocatore precedente è vivo
                if (closestPlayer.GetComponent<PlayerController>().dead == false)
                {
                    //Salvataggio del giocatore corrente come giocatore più vicino 
                    player = closestPlayer;
                    //Modifica del valore di closestDistance in base alla distanza da questo giocatore.
                    closestDistance = checkDistance;
                }
            }
        }
        //Verifica se la variabile player ha un giocatore al suo interno
        //Questo controllo ci aiuterà a prevenire gli errori
        if (!dead && player != null)
        {
            //Il resto della sceneggiatura non è cambiato rispetto alle lezioni precedenti.
            distance = Vector3.Distance(transform.position, player.transform.position);
            if (!dead)
            {
                Attack();
            }
        }
    }

    [PunRPC]
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
            anim.enabled = true;
            GetComponent<Collider>().enabled = false;
            // abilitazione dell'animazione della morte
            anim.SetBool("Die", true);
        }
    }

    void CheckPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        Invoke("CheckPlayers", 3f);
    }

    public void GetDamage(int count)
    {
        photonView.RPC("ChangeHealth", RpcTarget.All, count);
    }
}
