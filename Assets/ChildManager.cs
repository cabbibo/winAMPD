using UnityEngine;

public class ChildManager : MonoBehaviour
{
    public TransparentWindow TransparentCamera;
    public GameObject BackgroundUI;

    public void LoadChild()
    {
        TransparentCamera.enabled = true;
        BackgroundUI.SetActive(false);
    }

    public void UnloadChild()
    {
        TransparentCamera.enabled = false;
        BackgroundUI.SetActive(true);
    }
}
