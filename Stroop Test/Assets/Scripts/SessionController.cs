using UnityEngine;

namespace Scripts
{
    public class SessionController : MonoBehaviour
    {
        internal static SessionController Instance;

        internal string CurrentUser { get; private set; }

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

        internal void RegisterUser(string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(CurrentUser))
            {
                Debug.LogError("Cannot register while another user is in a session. Please end the previous session before registering.");
                return;
            }

            CurrentUser = string.Format("{0} {1}", firstName, lastName);
            Debug.Log(string.Format("New user '{0}' registered.", CurrentUser));

            // Perhaps register the user to json file (or wait until user is done?)
            SceneController.Instance.LoadNextScene();
        }
    }
}
