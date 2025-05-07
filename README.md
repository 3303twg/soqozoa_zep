# 젭타버스

## 📖 목차
1. [프로젝트 소개](#프로젝트-소개)
2. [주요기능](#주요기능)
3. [개발기간](#개발기간)
4. [기술스택](#기술스택)
5. [Trouble Shooting](#trouble-shooting)
    
## 👨‍🏫 프로젝트 소개
간단한 메타버스를 구현한 프로젝트입니다.  
메타버스답게 멀티플레이가 가능하며 간단한 미니게임을 확인해볼 수 있습니다.

## 💜 주요기능

- **기능 1**  
  유니티 포톤을 사용하여 실시간으로 유저들과 함께 플레이할 수 있습니다.

- **기능 2**  
  NPC와 상호작용하여 대화를 할 수 있습니다.
  
- **기능 3**  
  채팅을 통해 실시간으로 유저들과 소통할 수 있습니다.
  
- **기능 4**  
  탈것과 스킨을 변경할 수 있습니다.

## ⏲️ 개발기간
2025.04.30 ~ 2025.05.07

## 📚️ 기술스택

### ✔️ Language
- C#

### ✔️ Version Control
- Git (GitHub Desktop 사용)

### ✔️ IDE
- Visual Studio

### ✔️ Framework
- Unity 2D
- Photon (PUN2)

### ✔️ Deploy
- Unity Build (PC, Mac)

## Trouble Shooting

유니티 포톤의 메서드를 사용 중  
룸 입장 후 씬 전환을 하여 `Awake`나 `Start`에서 `PhotonNetwork.Instantiate`를 사용 시, 룸의 입장 상태가 false로 적용되는 문제가 있었습니다.  
그래서 `Invoke`를 사용하여 일정 시간 뒤에 호출하게 해서 룸의 입장 상태가 씬 전환 후에도 true가 되었을 때 호출되도록 수정했습니다.

하지만 원래 오브젝트가 생성되고 `Awake`나 `Start` 타이밍에 게임 매니저 같은 스크립트들이 플레이어 오브젝트를 찾는 작업을 수행해야 하는데, 위의 이유 때문에 해당 타이밍에 찾아야 할 플레이어 오브젝트를 찾을 수 없게 되었습니다.

따라서 `Init_player()`라는 메서드를 새로 만들어 임의의 타이밍에 초기화 작업을 해주도록 했습니다.

```csharp
public void Init_player()
{
    animator = GetComponent<Animator>();
    photon_view.RPC("Set_parent", RpcTarget.All);

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
    else if (photon_view.IsMine == false)
    {
        Debug.Log("왜안대??");
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    // 모든 클라 플레이어가 사용해야 함
    skinmanager.photon_view.RPC("RPC_Refresh_skin", RpcTarget.All);
    vehicleManager.photon_view.RPC("RPC_Refresh_vehicle", RpcTarget.All);
}
