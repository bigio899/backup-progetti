using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Pistol
{
    void Start()
    {
        //Il ritardo tra i colpi (puoi mettere il valore che vuoi)
        cooldown = 0.2f;
        //Quest'arma sar� totalmente automatica; continuer� a sparare finch� teniamo premuto il mouse (non ti preoccupare: il ritardo che hai implementato prima verr� tenuto in considerazione!)
        auto = true;
        ammoCurrent = 300;
        ammoMax = 30;
        ammoBackPack = 120;
    }
}
