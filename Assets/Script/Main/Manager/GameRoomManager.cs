using JetBrains.Annotations;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameRoomManager : MonoBehaviourPun
{
    public GameObject flappy;
    public Text slot_1;
    public Text slot_2;

    PhotonView photon_view;

    GameObject player_1;
    GameObject player_2;


    private void Awake()
    {
        photon_view = GetComponent<PhotonView>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open_flappy()
    {
        if (slot_1 == null || slot_2.text == null)
        {

            flappy.SetActive(true);
            slot_1 = flappy.transform.Find("Slot_1/name").GetComponent<Text>();
            slot_2 = flappy.transform.Find("Slot_2/name").GetComponent<Text>();

            if (slot_1.text == "")
            {
                slot_1.text = PhotonNetwork.NickName;

            }
            
            else if (slot_2.text == "")
            {
                slot_2.text = PhotonNetwork.NickName;

            }
            
        }

    }

    public void Close_flappy()
    {
        flappy.SetActive(false);
    }

    public void Start_flappy()
    {

        photon_view.RPC("RPC_Start_flappy", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_Start_flappy()
    {

        
        if (slot_1.text == "" && slot_2.text == "")
        {
            return;
        }

        else
        {
            if (slot_1.text == PhotonNetwork.NickName || slot_2.text == PhotonNetwork.NickName)
            {
                slot_1.text = null;
                slot_2.text = null;

                if (slot_1.text == PhotonNetwork.NickName)
                {
                    player_1.SetActive(false);

                }
                else
                {
                    player_2.SetActive(false);
                }
                SceneManager.LoadScene("Flappy");
            }
        }
        

    }
}
