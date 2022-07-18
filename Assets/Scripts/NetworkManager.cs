using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private PhotonView _photonView;
    private Vector3 _saveCarGameObject;
    private Text _text;
    private bool connect;
    public GameObject carGameObject;
    private void Start()
    {
        _text = GetComponent<Text>();
        _photonView = GetComponent<PhotonView>();
    }

    void Update() => _text.text = $"ping {PhotonNetwork.GetPing()}";


    public override void OnDisconnected(DisconnectCause cause)
    {
        _saveCarGameObject = carGameObject.transform.position;
        StartCoroutine(MainReconnect());
    }
    private IEnumerator MainReconnect()
    {
        while (PhotonNetwork.NetworkingClient.LoadBalancingPeer.PeerState != ExitGames.Client.Photon.PeerStateValue.Disconnected)
            yield return new WaitForSeconds(0.2f);
        connect = true;
        if (!PhotonNetwork.ReconnectAndRejoin())
            if (PhotonNetwork.Reconnect())
                Debug.Log("Successful reconnected!", this);
            else
                Debug.Log("Successful reconnected and joined!", this);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        connect = true;
        _photonView.RPC("SaveAfterReconnect", RpcTarget.All, carGameObject.transform.position);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) => connect = false;

    public override void OnJoinedRoom() => connect = false;

    [PunRPC]
    public void SaveAfterReconnect(Vector3 gameObject)
    {
        _saveCarGameObject = gameObject;
    }
    private void OnGUI()
    {
        if (connect)
        {
            GUI.Box(Rect.MinMaxRect(0, 0, Screen.width / 2, Screen.height / 2), "Связь потеряна!");
            carGameObject.transform.position = _saveCarGameObject;
        }
    }
}
