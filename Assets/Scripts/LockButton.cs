using UnityEngine;

public class LockButton : MonoBehaviour
{
    public int roomID;

    public void LockRoom()
    {
        GameManager.Instance.TryLockRoom(roomID);
    }
}