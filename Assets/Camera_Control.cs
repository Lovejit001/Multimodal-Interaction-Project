using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    public Camera Entrance_Office;
    public Camera Treatment_Room_1;
    public Camera Waiting_Room_1;
    public Camera Hall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Entrance_Office.gameObject.SetActive(true);
        Treatment_Room_1.gameObject.SetActive(false);
        Waiting_Room_1.gameObject.SetActive(false);
        Hall.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Entrance_Office.gameObject.SetActive(true);
            Treatment_Room_1.gameObject.SetActive(false);
            Waiting_Room_1.gameObject.SetActive(false);
            Hall.gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Entrance_Office.gameObject.SetActive(false);
            Treatment_Room_1.gameObject.SetActive(true);
            Waiting_Room_1.gameObject.SetActive(false);
            Hall.gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Entrance_Office.gameObject.SetActive(false);
            Treatment_Room_1.gameObject.SetActive(false);
            Waiting_Room_1.gameObject.SetActive(true);
            Hall.gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Entrance_Office.gameObject.SetActive(false);
            Treatment_Room_1.gameObject.SetActive(false);
            Waiting_Room_1.gameObject.SetActive(false);
            Hall.gameObject.SetActive(true);
        }
    }
}


// used scource:
// https://youtu.be/LyEHlc7vmrE?si=ocYzuT76r-Rp852