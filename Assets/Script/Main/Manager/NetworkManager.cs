using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{



    PhotonView photon_view;

    public GameObject player;

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

    [PunRPC]
    void sync_nick_name()
    {
        player.GetComponent<PlayerController>().RPC_change_nick_name();
    }

    public void RPC_sync_nick_name()
    {
        photon_view.RPC("sync_nick_name", RpcTarget.All);
    }
}
