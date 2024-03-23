using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEnemy : Enemy
{
    // La velocità dello scarabeo
    [SerializeField] float speed;
    // L'area di rilevamento dello scarabeo
    [SerializeField] float detectionDistance;
    [SerializeField]float patrolTimer;

    public override void Move()
    {
        // Se la distanza tra il nemico e il giocatore è inferiore al raggio di rilevamento dello scarabeo
        // E la distanza tra il nemico e il giocatore è superiore al raggio di attacco:
        if (distance < detectionDistance && distance > attackDistance)
        {
            // Rivolgere il nemico verso il giocatore
            transform.LookAt(player.transform);
            // Abilitazione dell'animazione di corsa
            anim.SetBool("Run", true);
            // Il progresso del coleottero
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        }
        // Altrimenti:
        else if (distance > detectionDistance)
        {
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
            patrolTimer += Time.deltaTime;
            anim.SetBool("run", true);
            if (patrolTimer > 10)
            {
                transform.Rotate(new Vector3(0, 90, 0));
                patrolTimer = 0;
            }
        }
        else
        {
            // Disabilitare l'animazione di esecuzione
            anim.SetBool("Run", false);
        }
    }
    public override void Attack()
    {
        // Attivazione del timer
        timer += Time.deltaTime;
        // Se la distanza tra il nemico e il giocatore è inferiore alla distanza d'attacco e il valore del timer è superiore al cooldown dell'attacco, il giocatore può scegliere di attaccare.
        if (distance < attackDistance && timer > cooldown)
        {
            // Ripristino del timer
            timer = 0;
            // Ottenere lo script del giocatore e richiamare la funzione di sottrazione della salute
            player.GetComponent<PlayerController>().ChangeHealth(damage);
            // Abilitazione dell'animazione di attacco
            anim.SetBool("Attack", true);
        }
        // Altrimenti...
        else
        {
            // Disabilitazione dell'animazione di attacco
            anim.SetBool("Attack", false);
        }
    }
}
