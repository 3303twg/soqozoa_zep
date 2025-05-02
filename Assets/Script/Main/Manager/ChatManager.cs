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



    void Start()
    {
        
    }

    public void Init_chat_manager()
    {
        chat_text = GameObject.Find("Player").transform.Find("Canvas/chat_ballon/text").GetComponent<Text>();
        chat_ballon = GameObject.Find("Player").transform.Find("Canvas/chat_ballon").gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        inputField.onEndEdit.AddListener(Write_text);
    }

    void Write_text(string text)
    {
        chat_ballon.SetActive(true);
        chat_text.text = text;

        StartCoroutine(Destroy_chat_ballon());
    }

    IEnumerator Destroy_chat_ballon()
    {
        yield return new WaitForSeconds(3f);  // 3√  ¥Î±‚
        chat_ballon.SetActive(false);
    }

}
