using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public List<RuntimeAnimatorController> animator_controller_list = new List<RuntimeAnimatorController>();

    public GameObject player;
    public PlayerInfo player_info;

    public PhotonView photon_view;


    private void Awake()
    {
        photon_view = GetComponent<PhotonView>();
        RuntimeAnimatorController[] loadedAnimators = Resources.LoadAll<RuntimeAnimatorController>("Animator");
        animator_controller_list.AddRange(loadedAnimators);

    }
    void Start()
    {
        
    }




    //넌 뭐냐?
    [PunRPC]
    public void RPC_Refresh_skin()
    {
        player.GetComponent<PlayerController>().photon_view.RPC("Refresh_skin",RpcTarget.All, player_info.skin_index);
    }


    // 플레이어 => 스킨매니저로
    //모든 클라이언트가 수행해야할 동작
    public void Change_skin(GameObject player_object, int index)
    {
        player_object.GetComponent<Animator>().runtimeAnimatorController = animator_controller_list[index];
    }

    //시작
    public void RPC_Change_num()
    {
        //내 클라이언트에서만 index 변경
        //데이터박스 내용 변경임 남이 알필요 없을듯? 내가 주면 되는거라
        player_info.skin_index++;
        if (player_info.skin_index >= animator_controller_list.Count)
        {
            player_info.skin_index = 0;
        }

        RPC_Refresh_skin();
    }
}
