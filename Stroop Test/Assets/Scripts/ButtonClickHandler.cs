using UnityEngine;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private Text _debugText;

    [Space(5)]
    [Header("UI alerts")]
    [SerializeField] private InputField _firstNameInput;
    [SerializeField] private GameObject _firstNameAlert;
    [SerializeField] private InputField _lastNameInput;
    [SerializeField] private GameObject _lastNameAlert;
    [SerializeField] private InputField _emailInput;
    [SerializeField] private GameObject _emailAlert;

    public void TryRegisterUser()
    {
        var firstName = _firstNameInput.text;
        var lastName = _lastNameInput.text;

        if (string.IsNullOrEmpty(firstName))
        {
            _firstNameAlert.SetActive(true);
            return;
        }

        _firstNameAlert.SetActive(false);

        if (string.IsNullOrEmpty(lastName))
        {
            _lastNameAlert.SetActive(true);
            return;
        }

        _lastNameAlert.SetActive(false);

        SessionController.Instance.RegisterUser(firstName, lastName);
    }

    public void TryEmail()
    {
        var mailAddress = _emailInput.text;

        if (string.IsNullOrEmpty(mailAddress)
            || !mailAddress.Contains("@")
            || !mailAddress.Contains("."))
        {
            _emailAlert.SetActive(true);
            return;
        }

        _emailAlert.SetActive(false);
        DataController.Instance.EmailTo(mailAddress);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void DebugLog(string action)
    {
        Debug.Log(action);

        if (_debugText != null)
        {
            _debugText.text = action;
        }
    }

    public void LoadNextScene()
    {
        SceneController.Instance.LoadNextScene();
    }

    public void FinishSession()
    {
        SessionController.Instance.EndSession();
        SceneController.Instance.LoadScene(0);
    }
}
