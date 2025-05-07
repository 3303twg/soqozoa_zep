using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{



    public PhotonView photon_view;
    public GameObject player;
    public GameObject player_list;
    public GameObject player_info;

    //��� �̱������� �� �ʿ䰡 ���������� 
    //�̴ϰ����� ���Կ� ���ؼ��� �����ؾ���
    private void Awake()
    {
        photon_view = GetComponent<PhotonView>();
        player_list = GameObject.Find("PlayerList");
        player_info = GameObject.Find("PlayerInfo");

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



    [PunRPC]
    public void RPC_sync_player()
    {
        

        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in objectsWithTag)
        {
            obj.transform.SetParent(player_list.transform);
        }

        if (player_info.GetComponent<PlayerInfo>().isGame == true)
        {
            player.SetActive(false);
        }

        else
        {
            player.GetComponent<PlayerController>().photon_view.RPC("Get_SkinManager", RpcTarget.All);
        }
    }
}
