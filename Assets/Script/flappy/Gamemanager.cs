using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;

public class Gamemanager : MonoBehaviour
{
    GameObject player_list;
    GameObject player_info;
    public GameObject gameover_UI;

    public int score = 0;
    public Text score_text;

    public int best_score = 0;
    public Text best_score_text;
    public Text best_name_text;


    public string name;

    private void Awake()
    {
        player_info = GameObject.Find("PlayerInfo");
    }
    void Start()
    {
        Hashtable flappy = PhotonNetwork.CurrentRoom.CustomProperties;

        if (flappy.TryGetValue("score", out object record_score))
        {
            best_score = (int)record_score;
            
        }
        
        if (flappy.TryGetValue("name", out object nick_name))
        {
            name = nick_name as string;
            
        }

        best_score_text.text = name + " : " + best_score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        score_text.text = score.ToString();

    }


    public void Exit_game()
    {
        //player_list.SetActive(true);
        player_info.GetComponent<PlayerInfo>().isGame = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }

    public void save_data()
    {

        Hashtable flappy = new Hashtable();

        if (best_score < score)
        {
            flappy["score"] = score;                  // Á¤¼ö
            flappy["name"] = GameObject.Find("PlayerList").transform.Find("Player/Canvas/name").GetComponent<Text>().text;

            PhotonNetwork.CurrentRoom.SetCustomProperties(flappy);
        }
    }
}
