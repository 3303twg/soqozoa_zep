using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flappy : MonoBehaviour, MiniGame
{
    GameObject player_list;
    GameObject player_info;

    private void Awake()
    {
        player_list = GameObject.Find("PlayerList");
        player_info = GameObject.Find("PlayerInfo");
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Play_game()
    {
        foreach (Transform child in player_list.transform)
        {
            player_info.GetComponent<PlayerInfo>().isGame = true;
            child.gameObject.GetComponent<PlayerController>().isGame = true;
            child.gameObject.SetActive(false);
        }

        SceneManager.LoadScene("Flappy");

    }
}
