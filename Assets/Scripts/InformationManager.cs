using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class InformationManager : MonoBehaviour
{
    public GameObject detourObstaclesObject;
    private PhotonView _photonView;
    public Image canvasImage;
    public static string imageSource = "Image/Navigator";

    void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
#if UNITY_ANDROID
        _photonView.RPC("SendCanvasInformation", RpcTarget.All, imageSource);
#endif
    }

    [PunRPC]
    public void SendCanvasInformation(string image)
    {
        imageSource = image;
        canvasImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(image);
        if (image == "Image/Autopilot")
            detourObstaclesObject.SetActive(false);
        else
            detourObstaclesObject.SetActive(true);
    }
}
