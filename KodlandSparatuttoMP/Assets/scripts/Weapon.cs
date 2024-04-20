using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Weapon : MonoBehaviourPunCallbacks
{
    //Il sistema a particelle che lascerà i buchi dei proiettili
    [SerializeField] protected GameObject particle;
    //La visuale (ci aiuterà a trovare il centro dello schermo)
    [SerializeField] protected GameObject cam;
    //La modalità di sparo dell'arma
    protected bool auto = false;
    //L'intervallo tra gli spari e il timer che conta il tempo
    protected float cooldown = 0;
    protected float timer = 0;
    //numero di proiettili nel caricatore
    protected int ammoCurrent;
    //massima capienza del caricatore
    protected int ammoMax;
    //munizioni di riserva 
    protected int ammoBackPack;
    //variabile che viene utilizzata per rappresentare il testo nella UI
    [SerializeField] protected TMP_Text ammoText;
    [SerializeField] AudioSource shoot;
    [SerializeField] AudioClip bulletSound, noBulletSound, reload;

    //All'inizio del gioco, impostiamo il timer in modo che sia uguale al valore del cooldown dell'arma.
    //Questo ci assicura che i primi colpi vengano sparati senza ritardo
    private void Start()
    {
        timer = cooldown;
    }
    private void Update()
    {
        if(photonView.IsMine)
        {
            //Avviare il timer
            timer += Time.deltaTime;
            //Se il giocatore sta premendo il tasto sinistro, chiamiamo la funzione Shoot per sparare
            if (Input.GetMouseButton(0))
            {
                Shoot();
            }

            //se il giocatore preme il tasto R
            if (Input.GetKeyDown(KeyCode.R))
            {
                //se il caricatore non è pieno, OPPURE se abbiamo almeno una munizione nelle riserve, allora
                if (ammoCurrent != ammoMax || ammoBackPack != 0)
                {
                    //attiva la funzione di ricarica con un leggero ritardo
                    //puoi impostare il ritardo a qualunque valore preferisci
                    Invoke("Reload", 1);
                }
            }
        }
    }
    //Controllare se su può sparare
    public void Shoot()
    {
        if (Input.GetMouseButtonDown(0) || auto)
        {
            if (ammoCurrent > 0)
            {
                OnShoot();
                timer = 0;
                ammoCurrent = ammoCurrent - 1;
                shoot.PlayOneShot(bulletSound);
                shoot.pitch = Random.Range(1f, 1.5f);
                AmmoTextUpdate();
            }
            else
            {
                shoot.PlayOneShot(noBulletSound);
            }
        }
    }
    //Questa funzione definirà ciò che accade ogni volta che l'arma spara. Dal momento che ha i modificatori protetti e virtuali, le classi che derivano da questa potranno definire la propria logica di tiro
    protected virtual void OnShoot()
    {

    }

    private void AmmoTextUpdate()
    {
        ammoText.text = ammoCurrent + " / " + ammoBackPack;
    }

    private void Reload()
    {
        //dichiarare una variabile che calcoli il numero di munizioni necessarie per effettuare una ricarica completa del caricatore
        int ammoNeed = ammoMax - ammoCurrent;
        shoot.PlayOneShot(reload);
        //se la quantità di munizioni di riserva è almeno pari a quelli che servono effettua la ricarica
        if (ammoBackPack >= ammoNeed)
        {
            //sottrarre il numero di munizioni ricaricate da quelle di riserva
            ammoBackPack -= ammoNeed;
            //aggiungere le munizioni  ricaricate al caricatore
            ammoCurrent += ammoNeed;
            AmmoTextUpdate();
        }
        //altrimenti(se le riserve non sono sufficienti per una ricarica completa)
        else
        {
            //aggiungere tutte le munizioni rimaste al caricatore
            ammoCurrent += ammoBackPack;
            //impostare le munizioni di riserva a 0
            ammoBackPack = 0;
            AmmoTextUpdate();
        }
    }


}
