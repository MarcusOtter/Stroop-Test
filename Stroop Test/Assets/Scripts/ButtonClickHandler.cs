using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ButtonClickHandler : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] private Text _debugText;

        [Space(5)]
        [Header("Registration UI")]
        [SerializeField] private InputField _firstNameInput;
        [SerializeField] private GameObject _firstNameAlert;
        [SerializeField] private InputField _lastNameInput;
        [SerializeField] private GameObject _lastNameAlert;

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
    }
}
