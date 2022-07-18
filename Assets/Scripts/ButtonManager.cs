using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    void Start()
    {
#if UNITY_STANDALONE_WIN
        gameObject.SetActive(false);
#endif
    }

    public void NavigatorClick()
    {
        InformationManager.imageSource = "Image/Navigator";
        CarController.autopilotWork = false;
    }

    public void AutopilotClick()
    {
        InformationManager.imageSource = "Image/Autopilot";
        CarController.autopilotWork = true;
    }

    public void SignalStart() => CarController.signalColor = "blue";

    public void SignalStop() => CarController.signalColor = "white";
}
