using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Pistol
{
    void Start()
    {
        //Il ritardo tra i colpi (puoi mettere il valore che vuoi)
        cooldown = 0.2f;
        //Quest'arma sarà totalmente automatica; continuerà a sparare finché teniamo premuto il mouse (non ti preoccupare: il ritardo che hai implementato prima verrà tenuto in considerazione!)
        auto = true;
        ammoCurrent = 300;
        ammoMax = 30;
        ammoBackPack = 120;
    }
}
