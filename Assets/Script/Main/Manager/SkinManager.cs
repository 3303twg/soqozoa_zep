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




    //�� ����?
    [PunRPC]
    public void RPC_Refresh_skin()
    {
        player.GetComponent<PlayerController>().photon_view.RPC("Refresh_skin",RpcTarget.All, player_info.skin_index);
    }


    // �÷��̾� => ��Ų�Ŵ�����
    //��� Ŭ���̾�Ʈ�� �����ؾ��� ����
    public void Change_skin(GameObject player_object, int index)
    {
        player_object.GetComponent<Animator>().runtimeAnimatorController = animator_controller_list[index];
    }

    //����
    public void RPC_Change_num()
    {
        //�� Ŭ���̾�Ʈ������ index ����
        //�����͹ڽ� ���� ������ ���� ���ʿ� ������? ���� �ָ� �Ǵ°Ŷ�
        player_info.skin_index++;
        if (player_info.skin_index >= animator_controller_list.Count)
        {
            player_info.skin_index = 0;
        }

        RPC_Refresh_skin();
    }
}
