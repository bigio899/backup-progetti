using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] List<Transform> spawns = new List<Transform>();
    int randSpawn;

    // Start is called before the first frame update
    void Start()
    {
        // Ottenere un numero casuale
        randSpawn = Random.Range(0, spawns.Count);
        PhotonNetwork.Instantiate("Player", spawns[randSpawn].position, spawns[randSpawn].rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
