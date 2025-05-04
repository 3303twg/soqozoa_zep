using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    
    public InputField inputField;

    public GameObject chat_ballon;
    public Text chat_text;
    public GameObject player;


    void Start()
    {
        
    }

    public void Init_chat_manager()
    {
        chat_text = player.transform.Find("Canvas/chat_ballon/text").GetComponent<Text>();
        chat_ballon = player.transform.Find("Canvas/chat_ballon").gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        inputField.onEndEdit.AddListener(RPC_Write_text);
    }


    void RPC_Write_text(string text)
    {
        player.GetComponent<PlayerController>().photon_view.RPC("Write_text", Photon.Pun.RpcTarget.All, text);
        StartCoroutine(Destroy_chat_ballon());
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
