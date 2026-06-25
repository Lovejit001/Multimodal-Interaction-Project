using UnityEngine;

public class RoomButton : MonoBehaviour
{
    public int roomID;
    public GameManager gameManager;
    public GameObject border;
    public VoiceInput voiceInput;

    public void SelectRoom()
    {
        GameManager.Instance.SelectRoom(roomID);
        voiceInput.StartListening();

    }

    private System.Collections.IEnumerator HideBorder()
{
    yield return new WaitForSeconds(2f);

    border.SetActive(false);
}

    public void SetSelected(bool selected)
    {
        border.SetActive(selected);
        StartCoroutine(HideBorder());
    }
}