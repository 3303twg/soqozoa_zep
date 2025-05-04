using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text people_num;

    public Text point;

    public GameObject gamemanager;
    public GameManager gamemanager_script;
    void Start()
    {
        gamemanager_script = gamemanager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //people_num.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "Έν";
        //point.text = gamemanager_script.point.ToString();
    }
}
