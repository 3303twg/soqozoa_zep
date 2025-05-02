using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text people_num;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        people_num.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "Έν";
    }
}
