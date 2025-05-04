using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public GameObject player_list;

    public GameObject player_prefab;

    public SkinManager skinmanager;

    public GameRoomManager gameRoomManager;

    GameObject player;
    private void Awake()
    {
        player_list = GameObject.Find("PlayerList");
        if (player_list.transform.Find("Player") != null)
        {
            player = player_list.transform.Find("Player").gameObject;
        }



    }


    public void Start()
    {

        if (PhotonNetwork.InRoom == true)
        {
            spawn_player();
        }

        else
        {
            Invoke("spawn_player", 0.5f);
        }
        


    }

    public void spawn_player()
    {
        if (player != null)
        {
            foreach (Transform child in player_list.transform)
            {
                child.gameObject.GetComponent<PlayerController>().isGame = false;
                child.gameObject.SetActive(true);
            }
        }

        //플레이어 체크
        if (player == null)
        {
            //없으면 스폰
            player = PhotonNetwork.Instantiate("Main/Prefabs/Player", transform.position, Quaternion.identity);
            player.name = "Player";
            player.transform.SetParent(player_list.transform);

        }
        //있든 없든 처음 씬에 도착했으니 초기화
        player.GetComponent<PlayerController>().Init_player();
    }

    void test()
    {

        skinmanager.photon_view.RPC("RPC_Refresh_skin", RpcTarget.All);

    }


    // Update is called once per frame
    void Update()
    {   

        
    }
}
