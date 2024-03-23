using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text logText;
    void Log(string message)
    {
        // Spostamento del testo alla riga successiva
        logText.text += "\n";
        // Aggiunta di un messaggio
        logText.text += message;
    }
    // Start is called before the first frame update
    void Start()
    {
        // Assegnare a un giocatore un soprannome con un numero casuale
        PhotonNetwork.NickName = "Player" + Random.Range(1, 9999);
        // Visualizzazione del nickname nel campo Log
        Log("Player Name: " + PhotonNetwork.NickName);
        // Configurazione del gioco
        PhotonNetwork.AutomaticallySyncScene = true; // Passaggio automatico da una finestra all'altra
        PhotonNetwork.GameVersion = "1"; // Impostazione della versione del gioco
        PhotonNetwork.ConnectUsingSettings(); // Connessione al server Photon
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 15 });
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        Log("Connesso al server");
    }

    public override void OnJoinedRoom()
    {
        Log("Sono entrati nella lobby");
        PhotonNetwork.LoadLevel("Lobby");
    }
}
