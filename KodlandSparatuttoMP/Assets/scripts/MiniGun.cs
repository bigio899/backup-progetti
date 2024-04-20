using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGun : Rifle
{

    void Start()
    {
        //Il ritardo tra i colpi (puoi mettere il valore che vuoi)
        cooldown = 0.1f;
        //Quest'arma sparerà in automatico; continuerà a sparare finché teniamo premuto il pulsante del mouse (non ti preoccupare: il ritardo che hai definito prima verrà tenuto in considerazione!)
        auto = true;
        ammoCurrent = 100;
        ammoMax = 100;
        ammoBackPack = 300;
    }

    protected override void OnShoot()
    {
        Vector3 rayStartPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 drift = new Vector3(Random.Range(-15, -15), Random.Range(-15, -15), Random.Range(-15, -15));

        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(rayStartPosition + drift);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject gameBullet = Instantiate(particle, hit.point, hit.transform.rotation);
            if (hit.collider.CompareTag("enemy"))
            {
                // È possibile modificare il numero 10 in qualsiasi altro numero. È la quantità di danni causati da 1 proiettile.
            }
            Destroy(gameBullet, 1);
        }
    }

    public void Shoot()
    {
        if (Input.GetMouseButtonDown(0) || auto)
        {
            if (timer > cooldown)
            {
                if (ammoCurrent > 0)
                {
                    OnShoot();
                    timer = 0;
                    ammoCurrent = ammoCurrent - 1;
                }
            }
        }
    }

    private void AmmoTextUpdate()
    {
        ammoText.text = ammoCurrent + " / " + ammoBackPack;
    }

    private void Update()
    {       
        timer += Time.deltaTime;
        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        AmmoTextUpdate();
    }
}
