using System;
using System.Linq;
using UnityEngine;

public class SessionController : MonoBehaviour
{
    internal static SessionController Instance;

    internal string CurrentUser { get; private set; }

    internal int ActiveSection { get; private set; } // 0-based
    internal readonly float[] ClearTimes = new float[3];

    private readonly float[] _startTimes = new float[3];

    private void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    internal float GetTotalTime()
    {
        return ClearTimes.Sum();
    }

    internal void StartCountingTime(int activeSection)
    {
        ActiveSection = activeSection - 1;
        _startTimes[ActiveSection] = Time.time;
    }

    internal void StopCountingTime()
    {
        ClearTimes[ActiveSection] = (float) Math.Round(Time.time - _startTimes[ActiveSection], 2);
        print("Clear time: " + ClearTimes[ActiveSection]);
    }

    internal void RegisterUser(string firstName, string lastName)
    {
        if (!string.IsNullOrEmpty(CurrentUser))
        {
            Debug.LogError("Cannot register while another user is in a session. Please end the previous session before registering.");
            return;
        }

        // Remove spaces in first name & last name.
        firstName = firstName.Replace(" ", "");
        lastName = lastName.Replace(" ", "");

        CurrentUser = string.Format("{0} {1}", firstName, lastName);
        Debug.Log(string.Format("User '{0}' entered session.", CurrentUser));

        SceneController.Instance.LoadNextScene();
    }

    internal void EndSession()
    {
        DataController.Instance.SerializeNewUserData(CurrentUser, ClearTimes);
        CurrentUser = "";
    }
}
