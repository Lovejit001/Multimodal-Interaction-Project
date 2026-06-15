using UnityEngine;

public class RoomButton : MonoBehaviour
{
    public int roomID;

    public void SelectRoom()
    {
        GameManager.Instance.SelectRoom(roomID);
    }
}