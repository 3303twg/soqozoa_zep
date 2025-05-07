using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    //�̰� �ȹٲ�µ�?
    public GameObject player;
    GameObject[] vehicles;
    PlayerInfo player_info;


    public PhotonView photon_view;


    private void Awake()
    {
        photon_view = GetComponent<PhotonView>();
        player_info = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
    }


    public void Get_player_vehicle()
    {

        Transform parentTransform = player.transform.Find("VehiclePivot");
        Transform[] children = new Transform[parentTransform.childCount];

        for (int i = 0; i < parentTransform.childCount; i++)
        {
            children[i] = parentTransform.GetChild(i);
        }


        vehicles = new GameObject[parentTransform.childCount];

        for (int i = 0; i < parentTransform.childCount; i++)
        {
            vehicles[i] = parentTransform.GetChild(i).gameObject;
        }
    }

    public void RPC_Change_num()
    {
        //��Ÿ�������� ž�¸�
        if (player.GetComponent<PlayerController>().isRiding == false)
        {
            player.GetComponent<PlayerController>().photon_view.RPC("Change_isRiding",RpcTarget.All,true);
            Get_on(player,player_info.vehicle_index);
        }

        //Ÿ�������� �ε��� ������ �ʰ��� ����
        else
        {
            player_info.vehicle_index++;
            if (player_info.vehicle_index >= vehicles.Length)
            {
                player_info.vehicle_index = 0;
                player.GetComponent<PlayerController>().photon_view.RPC("Change_isRiding", RpcTarget.All, false);
            }

        }


        RPC_Refresh_vehicle();
    }

    public void Get_on(GameObject player_object, int index)
    {

        Transform parentTransform = player_object.transform.Find("VehiclePivot");
        vehicles = new GameObject[parentTransform.childCount];

        for (int i = 0; i < parentTransform.childCount; i++)
        {
            vehicles[i] = parentTransform.GetChild(i).gameObject;
        }


        //�ε����� ������ �����ϱ⿣ �ƿ������ε����� �ذ�������ؼ� ���� ����
        foreach (GameObject obj in vehicles)
        {
            obj.SetActive(false);
        }


        player_object.GetComponent<PlayerController>().speed = vehicles[index].GetComponent<VehicleStat>().Speed;
        vehicles[index].SetActive(true);


        if (player_object.GetComponent<PlayerController>().isRiding == false)
        {
            Get_off(player_object);
        }
    }

    public void Get_off(GameObject player_object)
    {
        player_object.GetComponent<PlayerController>().speed = 5f;

        Transform parentTransform = player_object.transform.Find("VehiclePivot");
        vehicles = new GameObject[parentTransform.childCount];

        for (int i = 0; i < parentTransform.childCount; i++)
        {
            vehicles[i] = parentTransform.GetChild(i).gameObject;
        }

        foreach (GameObject obj in vehicles)
        {
            obj.SetActive(false);
        }
        player_object.GetComponent<PlayerController>().photon_view.RPC("Change_isRiding", RpcTarget.All, false);
    }

    public void Change_vehicle(GameObject player_object, int index)
    {
        
        Transform parentTransform = player_object.transform.Find("VehiclePivot");
        Transform[] children = new Transform[parentTransform.childCount];

        for (int i = 0; i < parentTransform.childCount; i++)
        {
            children[i] = parentTransform.GetChild(i);
        }


        vehicles = new GameObject[parentTransform.childCount];

        for (int i = 0; i < parentTransform.childCount; i++)
        {
            vehicles[i] = parentTransform.GetChild(i).gameObject;
        }
        
        if (player_object.GetComponent<PlayerController>().isRiding == true)
        {
            Get_on(player_object, index);
        }
        else
        {
            Get_off(player_object);
        }
    }


    [PunRPC]
    public void RPC_Refresh_vehicle()
    {
        player.GetComponent<PlayerController>().photon_view.RPC("Refresh_vehicle", RpcTarget.All, player_info.vehicle_index);
    }
}
