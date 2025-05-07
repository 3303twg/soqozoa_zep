# 젭타버스

## 📖 목차
1. [프로젝트 소개](#프로젝트-소개)
2. [주요기능](#주요기능)
3. [개발기간](#개발기간)
4. [기술스택](#기술스택)
5. [Trouble Shooting](#trouble-shooting)
    
## 👨‍🏫 프로젝트 소개
간단한 메타버스를 구현한 프로젝트입니다.
메타버스답게 멀티플레이가 가능하며
간단한 미니게임을 확인해볼 수 있습니다.

## 💜 주요기능

- 기능 1
- 유니티 포톤을 사용하여 실시간으로 유저들과 함께 플레이할 수 있습니다.

- 기능 2
 NPC와 상호작용하여 대화를 할 수 있습니다.
  
- 기능 3
 채팅을 통해 실시간으로 유저들과 소통할 수 있습니다.
  
- 기능 4
 탈것과 스킨을 변경할 수 있습니다.

## ⏲️ 개발기간
 2025.04.30 ~ 2025.05.07

## 📚️ 기술스택

### ✔️ Language

### ✔️ Version Control

### ✔️ IDE

### ✔️ Framework

### ✔️ Deploy



## Trouble Shooting

유니티 포톤의 메서드를 사용중

룸 입장 후 씬전환을 하여 awake나 start에서 PhotonNetwork.Instantiate를

사용시 룸의 입장상태가 false로 적용되는 문제가 있었다.

때문에 Invoke를 사용해서 일정 시간 뒤에 호출하게하여 룸의 입장상태가 씬전환 후에도

true가 되었을때 호출되도록 수정했다.

 

 

하지만

원래 오브젝트가 생성되고 awake나 start타이밍때 예를들어 게임매니저같은 스크립트들이

플레이어 오브젝트를 찾는 작업을 수행해야하는데

 

위의 이유때문에 해당 타이밍에 찾아야할 플레이어오브젝트를 찾을수 없게 되었다.

 

따라서 Init_player() 라는 메서드를 새로 만들어 임의의 타이밍에 초기화 작업을 해주도록했다.

public void Init_player()
{

    animator = GetComponent<Animator>();


    photon_view.RPC("Set_parent", RpcTarget.All);
    //카메라 설정

    if (photon_view.IsMine)
    {
        camera = Camera.main;
        GameObject.Find("Main Camera").GetComponent<CameraController>().target = gameObject;

        networkmanager = GameObject.Find("NetWorkManager");
        networkmanager.GetComponent<NetworkManager>().player = gameObject;
        networkmanager.GetComponent<NetworkManager>().photon_view.RPC("RPC_sync_player", RpcTarget.All);
        networkmanager.GetComponent<NetworkManager>().RPC_sync_player();

        chatmanager = GameObject.Find("ChatManager");
        chatmanager.GetComponent<ChatManager>().player = gameObject;
        chatmanager.GetComponent<ChatManager>().Init_chat_manager();


        nick_name = player_info.GetComponent<PlayerInfo>().nick_name;
        name.text = nick_name;
        networkmanager.GetComponent<NetworkManager>().RPC_sync_nick_name();

        
    }
    else if(photon_view.IsMine == false)
    {
        Debug.Log("왜안대??");
        GetComponent<BoxCollider2D>().isTrigger = true;
    }


    
    //모든 클라플레이어가 사용해야함
    skinmanager.photon_view.RPC("RPC_Refresh_skin", RpcTarget.All);
    //스킨매니저 참고해서 동작시킬듯
    vehicleManager.photon_view.RPC("RPC_Refresh_vehicle", RpcTarget.All);
}


해당 코드를 사용해서 초기화 작업을 수행해줬더니 문제없이 잘 동작했다.





