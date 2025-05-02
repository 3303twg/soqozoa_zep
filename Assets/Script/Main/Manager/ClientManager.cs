using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public GameObject player_prefab;

    private void Awake()
    {
        
    }


    public void Start()
    {

        Invoke("spawn_player",1);
        

    }

    public void spawn_player()
    {
        GameObject go = PhotonNetwork.Instantiate("Main/Prefabs/Player", transform.position, Quaternion.identity);
        go.name = "Player";
    }
    

    // Update is called once per frame
    void Update()
    {   

        
    }
}
