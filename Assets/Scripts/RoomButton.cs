using UnityEngine;

public class RoomButton : MonoBehaviour
{
    public int roomID;
    public GameManager gameManager;
    public GameObject border;

    public void SelectRoom()
    {
        GameManager.Instance.SelectRoom(roomID);

    }

    public void SetSelected(bool selected)
    {
        border.SetActive(selected);
    }
}