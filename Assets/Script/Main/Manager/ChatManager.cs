using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class ChatManager : MonoBehaviour
{
    
    public InputField inputField;

    public GameObject chat_ballon;
    public Text chat_text;
    public GameObject player;

    public bool chat_flag = false;

    private Coroutine destroyCoroutine;

    void Start()
    {
        inputField.interactable = false;
        inputField.onEndEdit.AddListener(RPC_Write_text);
    }

    public void Init_chat_manager()
    {
        chat_text = player.transform.Find("Canvas/chat_ballon/text").GetComponent<Text>();
        chat_ballon = player.transform.Find("Canvas/chat_ballon").gameObject;
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            chat_flag = !chat_flag; // 엔터로 입력 모드 토글
            inputField.interactable = chat_flag;

            if (chat_flag)
            {
                player.GetComponent<PlayerController>().isChat = true;
                inputField.ActivateInputField(); // 포커스 부여
            }
            else
            {
                if (inputField.text != "")
                {
                    RPC_Write_text(inputField.text);
                    inputField.text = "";
                }
                inputField.DeactivateInputField(); // 포커스 해제
                player.GetComponent<PlayerController>().isChat = false;
            }
        }
        
    }


    void RPC_Write_text(string text)
    {
        player.GetComponent<PlayerController>().photon_view.RPC("Write_text", Photon.Pun.RpcTarget.All, text);

        // 기존 코루틴이 있다면 멈춤
        if (destroyCoroutine != null)
        {
            StopCoroutine(destroyCoroutine);
        }

        // 새로 시작
        destroyCoroutine = StartCoroutine(Destroy_chat_ballon());
    }

    IEnumerator Destroy_chat_ballon()
    {
        yield return new WaitForSeconds(3f);  // 3초 대기
        RPC_Destroy_chat_ballon();
    }


    void RPC_Destroy_chat_ballon()
    {
        player.GetComponent<PlayerController>().photon_view.RPC("Destroy_chat_ballon", Photon.Pun.RpcTarget.All);
    }

    /*
    public void Write_text(string text)
    {
        chat_ballon.SetActive(true);
        chat_text.text = text;


        //이거 코루틴으로 하면 안될듯 매우 꼬이네
        StartCoroutine(Destroy_chat_ballon());
    }

    */

}
