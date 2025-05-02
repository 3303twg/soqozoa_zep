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
        if (stream.IsWriting) // 내가 주인이라면
        {
            stream.SendNext(point); // 내 데이터를 보냄
        }
        else // 상대방이 보내는 값을 받음
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
        //마스터클라이언트일때도 해당 호출을 당하긴하는데 뭐 일단 문제없지않나?
        photon_view.RPC("send_point", RpcTarget.MasterClient, point);

    }
}
