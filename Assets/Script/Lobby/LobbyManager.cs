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

    public GameObject player_info;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
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

    public override void OnConnectedToMaster()
    {
        //�κ� ����
        PhotonNetwork.JoinLobby();
    }


    //�κ� ���� �Ϸ��
    public override void OnJoinedLobby()
    {

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
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

    }


    //�� ����Ϸ�
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Main");
    }



    //��ư���� ȣ���� �Լ�
    public void JoinRoom()
    {
        
        string txt = text;
        player_info.GetComponent<PlayerInfo>().nick_name = txt;
        
        //���� �ϳ��ۿ� ���������̶� ���������ϱ�����
        //���� �����Ϸ��� �̸� �Է��ؾ���
        PhotonNetwork.JoinRandomRoom();

    }
}
