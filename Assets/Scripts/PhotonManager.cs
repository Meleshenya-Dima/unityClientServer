using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"Подключение прошло успешно к региону: {PhotonNetwork.CloudRegion}");
        JoinOrCreateRoom();
    }

    public void JoinOrCreateRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinOrCreateRoom("InformationRoom", new RoomOptions { MaxPlayers = 2, CleanupCacheOnLeave = true, DeleteNullProperties = true, PlayerTtl = -1, EmptyRoomTtl = -1}, TypedLobby.Default);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Вы успешно подключились к комнате!");

        PhotonNetwork.LoadLevel("MainScene");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Комната успешно создана");
    }
}
