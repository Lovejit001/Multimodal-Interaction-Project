using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;

public class VoiceInput : MonoBehaviour
{
    public GameManager gameManager;

    private KeywordRecognizer recognizer;

    void Start()
    {
        Dictionary<string, System.Action> commands =
            new Dictionary<string, System.Action>();

        commands.Add("lock 1",
            () => gameManager.TryLockRoom(0));

        commands.Add("lock 2",
            () => gameManager.TryLockRoom(1));

        commands.Add("lock 3",
            () => gameManager.TryLockRoom(2));

        commands.Add("lock 4",
            () => gameManager.TryLockRoom(3));

        recognizer =
            new KeywordRecognizer(new List<string>(commands.Keys).ToArray());

        recognizer.OnPhraseRecognized +=
            (args) =>
            {
                commands[args.text].Invoke();
            };
    }

    public void StartListening()
    {
        if (!recognizer.IsRunning)
        {
            recognizer.Start();
            
            Invoke(nameof(StopListening), 2f);

            Debug.Log("Listening...");
        }
    }

    public void StopListening()
    {
        if (recognizer.IsRunning)
        {
            recognizer.Stop();

            Debug.Log("Stopped");
        }
    }

    void OnDestroy()
    {
        recognizer?.Dispose();
    }
}