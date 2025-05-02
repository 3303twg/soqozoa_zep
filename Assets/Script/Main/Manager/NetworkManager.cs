using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{

    GameObject player_prefab = Resources.Load<GameObject>("Main/Prefabs/Player");


    PhotonView photonView;



    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
