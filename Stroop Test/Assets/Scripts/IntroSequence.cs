using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    [RequireComponent(typeof(Animator))]
    public class IntroSequence : MonoBehaviour
    {
        // Contains a greeting and "{name}".
        // {name} is replaced with the actual name by this script.
        [SerializeField] private Text _greetingText;

        private Animator _animator;
        private int _skipHash;

        private bool _allowSkip;

        private void Start()
        {
            if (SessionController.Instance != null)
            {
                _greetingText.text = _greetingText.text.Replace("{name}", SessionController.Instance.CurrentUser);
            }

            _animator = GetComponent<Animator>();
            _skipHash = Animator.StringToHash("Next");
        }

        private void Update()
        {
            if (!_allowSkip) return;

            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                _animator.SetTrigger(_skipHash);
                _allowSkip = false;
                Debug.Log("Skipped to next screen by tapping/clicking the screen.");
            }
        }

        // Called by animation event
        internal void AllowSkip()
        {
            _allowSkip = true;
        }
    }
}
