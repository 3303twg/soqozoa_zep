using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour, IPunObservable
{
    public int max_point;
    public int point;

    PhotonView photon_view;


    private void Awake()
    {
        photon_view = GetComponent<PhotonView>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // ���� �����̶��
        {
            stream.SendNext(point); // �� �����͸� ����
        }
        else // ������ ������ ���� ����
        {
            point = (int)stream.ReceiveNext();
        }


    }


    [PunRPC]
    public void send_point(int my_point)
    {
        point = my_point;
    }

    public void RPC_send_point()
    {
        //������Ŭ���̾�Ʈ�϶��� �ش� ȣ���� ���ϱ��ϴµ� �� �ϴ� ���������ʳ�?
        photon_view.RPC("send_point", RpcTarget.MasterClient, point);

    }
}
