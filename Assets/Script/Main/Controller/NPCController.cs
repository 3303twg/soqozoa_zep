using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;




public interface MiniGame
{
    //만들어만 놓음 ㅋㅋ..
    public abstract void Play_game();

}


public class NPCController : MonoBehaviour
{

    public Material normal_material;
    public Material outlineMaterial;
    private Renderer renderer;

    public GameObject chat_ballon;
    public Text ballon_text;

    public bool isDialog = false;
    public bool isSpeak = false;

    // 외곽선의 원래 크기
    public float outlineSizeOn = 1.0f;

    public string[] speak_text =
    {
        "저는 해골입니다.",
        "저에게 F키로 말을 거실수 있습니다.",
        "크아악"
    };
    public string[] dialog_text =
    {
        "하이여",
        "반갑여",
        "해골여"
    };

    public int dialog_index = 0;



    


    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        //renderer.material = normal_material;
    }


    public void Outline_off()
    {
        renderer.material = normal_material;
    }

    public void Outline_on()
    {
        renderer.material = outlineMaterial;
    }
    public void ToggleOutline()
    {

        if (renderer.material == outlineMaterial)
        {
            renderer.material = normal_material;
        }
        else
        {
            
            renderer.material = outlineMaterial;
        }
    }


    private void Update()
    {

        if (isDialog == false)
        {
            //일정 주기마다 시키고싶은데

            if (isSpeak == false)
            {
                isSpeak = true;
                Invoke("Auto_chat", 3f);
            }
        }
    }

    public void Auto_chat()
    {
        if (isDialog == false)
        {
            ballon_text.text = speak_text[Random.Range(0, speak_text.Length)];
            chat_ballon.SetActive(true);
            Invoke("Destroy_chat_ballon", Random.RandomRange(2f,5f));
        }
    }

    public void Destroy_chat_ballon()
    {
        if (isDialog == false)
        {
            chat_ballon.SetActive(false);
        }
        isSpeak = false;
    }

    public void Show_dialog()
    {
        isDialog = true;
        ballon_text.text = dialog_text[0];
        chat_ballon.SetActive(true);
        dialog_index++;

    }

    public void Next_dialog()
    {
        
        if (dialog_text.Length > dialog_index)
        {
            ballon_text.text = dialog_text[dialog_index];
            dialog_index++;
        }

        else
        {
            Close_dialog();
        }
        
    }

    public void Close_dialog()
    {
        dialog_index = 0;
        isDialog = false;
        chat_ballon.SetActive(false);
        isSpeak = false;
    }
}
