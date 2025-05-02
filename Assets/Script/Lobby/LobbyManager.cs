using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;


public class LobbyManager : MonoBehaviourPunCallbacks
{

    public InputField inputField;
    public string text;

    public Text ConnectionStatus;
    public Text IDtext;
    public Button connetBtn;


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        
    }


    void Update()
    {
        //���� ���� ǥ��
        ConnectionStatus.text = PhotonNetwork.NetworkClientState.ToString();
        inputField.onValueChanged.AddListener(OnTextChanged);
    }

    void OnTextChanged(string text1)
    {
        text = text1;
    }

    // Update is called once per frame
    public override void OnConnectedToMaster()
    {

        
        print("���� ���� �Ϸ�");
        
        //�κ� ����
        PhotonNetwork.JoinLobby();
    }


    //�κ� ���� �Ϸ��
    public override void OnJoinedLobby()
    {
        print("�κ� ���� �Ϸ�");

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("���� �� ����. �� �� ���� ��...");

        // ���� ���� ���ٸ� ���� ����
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20; // �ִ� �ο� ����

        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;


        
        //�� �����ϴ°ǵ� �߰� �����̳� ���� ������ �ٵ� ������ ���ص��ɵ�?
        PhotonNetwork.CreateRoom("test", roomOptions); // �̸����� �����ϸ� ������ �ڵ����� �̸� ����
    }


    //�� ����
    public override void OnCreatedRoom()
    {
        Debug.Log("���ο� �� ������");
    }


    //�� ����Ϸ�
    public override void OnJoinedRoom()
    {
        Debug.Log("�뿡 ���� �Ϸ�");

        //���ڿ� ���߿� ������ �־��ֱ� int ������ �صεǱ���
        //SceneManager.LoadScene("Main");
        if (PhotonNetwork.IsMasterClient)
        {
            string txt = text;
            PhotonNetwork.LocalPlayer.NickName = txt;

            PhotonNetwork.LoadLevel("Main");
        }
        //
    }



    //��ư���� ȣ���� �Լ�
    public void JoinRoom()
    {
        //���� �ϳ��ۿ� ���������̶� ���������ϱ�����
        //���� �����Ϸ��� �̸� �Է��ؾ���
        PhotonNetwork.JoinRandomRoom();
    }
}
