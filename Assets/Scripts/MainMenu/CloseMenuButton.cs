using UnityEngine;

public class CloseMenuButton : MonoBehaviour
{
    public GameObject menu;

    public void CloseMenu()
    {
        menu.SetActive(false);
    }
}
