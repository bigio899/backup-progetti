using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeSkin : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] GameObject[] body;
    [SerializeField] GameObject[] head;
    [SerializeField] bool isMale;
    // Start is called before the first frame update
    private void Start()
    {
        Replace(isMale);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isMale = !isMale;
                Replace(isMale);
            }
        }
    }

    public void Replace(bool value)
    {
        body[0].SetActive(value);
        head[0].SetActive(value);
        body[1].SetActive(!value);
        head[1].SetActive(!value);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Invio del valore della variabile isMale agli altri giocatori se � stata modificata
            stream.SendNext(isMale);
        }
        else
        {
            // Ricevere i dati dagli altri giocatori e memorizzarli nella variabile isMale
            isMale = (bool)stream.ReceiveNext();
            // Richiamo del metodo Replace con il nuovo valore della variabile isMale
            Replace(isMale);
        }
    }
}
