using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    internal static SceneController Instance;

    private void Awake()
    {
        // Force landscape mode
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        Input.multiTouchEnabled = false;

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

    // TODO: Scene transition animation
    internal void LoadNextScene()
    {
        // Start animation, which should call a private method once finished.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    internal void LoadScene(int sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
