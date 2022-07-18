using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public static bool autopilotWork = false;
    public Text defectText;
    private PhotonView _photonView;
    public static string signalColor = "white";
    private float _speed = 0.05f;
    private bool _defect = false;

    public enum MovementType
    {
        Moveing
    }

    public MovementType type = MovementType.Moveing;
    public MovementPath MyPath;
    public float maxDistance = .1f;
    private IEnumerator<Transform> pointInPath;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        if (MyPath == null)
        {
            return;
        }
        pointInPath = MyPath.GetNextPathPoints();
        pointInPath.MoveNext();
        if (pointInPath.Current == null)
        {
            return;
        }
        gameObject.transform.position = pointInPath.Current.position;
    }

    private void Update()
    {

#if UNITY_STANDALONE_WIN
        if (Input.GetKey(KeyCode.T)) _defect = !_defect;
        CarMove();
        _photonView.RPC("SendComputerCarInformation", RpcTarget.All, gameObject.transform.position, _defect);
#endif
#if UNITY_ANDROID
        _photonView.RPC("SendPhoneCarInformation", RpcTarget.All, signalColor, autopilotWork);
#endif
    }

    private void CarMove()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.localPosition += new Vector3(0, _speed, 0);
            _photonView.RPC("CarComputerAutopilotOff", RpcTarget.AllBuffered);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.localPosition += new Vector3(0, -_speed, 0);
            _photonView.RPC("CarComputerAutopilotOff", RpcTarget.AllBuffered);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localPosition += new Vector3(-_speed, 0, 0);
            _photonView.RPC("CarComputerAutopilotOff", RpcTarget.AllBuffered);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.localPosition += new Vector3(_speed, 0, 0);
            _photonView.RPC("CarComputerAutopilotOff", RpcTarget.AllBuffered);
        }

        if (autopilotWork)
        {
            if (pointInPath == null || pointInPath.Current == null)
            {
                return;
            }

            if (type == MovementType.Moveing)
            {
                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, pointInPath.Current.position, _speed);
            }

            var distanceSqure = (gameObject.transform.position - pointInPath.Current.position).sqrMagnitude;

            if (distanceSqure < maxDistance * maxDistance)
            {
                pointInPath.MoveNext();
            }
        }
    }

    [PunRPC]
    public void CarComputerAutopilotOff()
    {
        autopilotWork = false;
        InformationManager.imageSource = "Image/Navigator";
    }

    [PunRPC]
    public void SendComputerCarInformation(Vector3 carPosition, bool defect)
    {
        _defect = defect;
        if (defect)
            defectText.GetComponent<Text>().text = "Машина неисправна";
        else
            defectText.GetComponent<Text>().text = "Машина исправна";
        gameObject.transform.position = carPosition;
    }

    [PunRPC]
    public void SendPhoneCarInformation(string color, bool autopilot)
    {
        autopilotWork = autopilot;
        if (color == "blue")
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        else
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
