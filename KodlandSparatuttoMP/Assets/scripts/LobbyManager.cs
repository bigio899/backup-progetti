using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text ChatText;
    [SerializeField] TMP_InputField InputText;
    [SerializeField] TMP_Text PlayersText;
    [SerializeField] GameObject startButton;
    [SerializeField] TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        RefreshPlayers();
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Log(string message)
    {
        ChatText.text += "\n";
        ChatText.text += message;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        // Emissione di un messaggio che informa che un giocatore con un nickname specifico ha lasciato la stanza.
        Log(otherPlayer.NickName + " left the room");
        RefreshPlayers();
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // Emissione di un messaggio che informa che un giocatore con un nickname specifico è entrato nella stanza.
        Log(newPlayer.NickName + " entered the room");
        RefreshPlayers();
    }

    [PunRPC]
    public void ShowMessage(string message)
    {
        ChatText.text += "\n";
        ChatText.text += message;
    }

    public void Send()
    {
        //If the field does not have any text in it, we do not do anything
        if (string.IsNullOrWhiteSpace(InputText.text)) { return; }
        // If a player presses the enter button Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // We call the ShowMessage method for all the players on the server
            // We need to output the nickname of the player and all the text they wrote in their InputField
            photonView.RPC("ShowMessage", RpcTarget.All, PhotonNetwork.NickName + ": " + InputText.text);
            // Clearing the text sting in the InputField
            InputText.text = string.Empty;
        }
    }

    [PunRPC]
    public void ShowPlayers()
    {
        // Annullamento dell'elenco dei giocatori, lasciando solo la riga 'Giocatori: '.
        PlayersText.text = "Players: ";
        // Avvio di un ciclo che attraversa tutti i giocatori sul server
        foreach (Photon.Realtime.Player otherPlayer in PhotonNetwork.PlayerList)
        {
            // Passaggio alla nuova linea
            PlayersText.text += "\n";

            // Emissione del soprannome del giocatore
            PlayersText.text += otherPlayer.NickName;
        }
    }

    void RefreshPlayers()
    {
        // La chiamata può essere effettuata solo dal Master Client (il giocatore che ha creato il server).
        if (PhotonNetwork.IsMasterClient)
        {
        //Richiamo del metodo ShowPlayers per tutti i giocatori della Lobby
            photonView.RPC("ShowPlayers", RpcTarget.All);
        }
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }

}
