using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumQuiz : MonoBehaviour, MiniGame
{
    public Text num_1;
    public Text num_2;
    public Text num_3;

    public GameObject game;
    public GameObject gamemanager;

    void Start()
    {
        Quiz_reset();
    }


    private void Update()
    {
        if (num_1 != null && num_2 != null && num_3 != null)
        {
            if (int.Parse(num_3.text) == int.Parse(num_1.text) + int.Parse(num_2.text))
            {
                Quiz_reset();
                game.SetActive(false);
                gamemanager.GetComponent<GameManager>().point += 1;
                gamemanager.GetComponent<GameManager>().RPC_send_point();
            }
        }
    }


    public void Play_game()
    {

        game.SetActive(true);

    }

    public void Up_btn()
    {
        num_3.text = (int.Parse(num_3.text) + 1).ToString();
        if (int.Parse(num_3.text) > 20)
        {
            num_3.text = 0.ToString();
        }
    }

    private void Quiz_reset()
    {
        if (num_1 != null && num_2 != null && num_3 != null)
        {
            num_1.text = Random.Range(0, 10).ToString();
            num_2.text = Random.Range(0, 10).ToString();


            //값이 무조건 다르게
            while (true)
            {
                num_3.text = Random.Range(0, 20).ToString();
                if (int.Parse(num_3.text) != int.Parse(num_1.text) + int.Parse(num_2.text))
                {
                    break;
                }

            }
        }
    }

}
