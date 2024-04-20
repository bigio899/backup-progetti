using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    void Start()
    {
        cooldown = 0.1f;
        //Questa non è un'arma automatica, ovvero dobbiamo cliccare il mouse ogni volta che vogliamo sparare
        auto = false;
        ammoCurrent = 10;
        ammoMax = 10;
        ammoBackPack = 30;
    }

    protected override void OnShoot()
    {
        Vector3 rayStartPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(rayStartPosition);
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
}
