using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class TextFormatter : MonoBehaviour
{
    // Can contain "{name}", "{time}", "{total}".
    // They are replaced with their respective values by this script.
    [SerializeField] private Text[] _textToFormat;

    private SessionController _session;

    private void Start()
    {
        _session = SessionController.Instance;

        if (_session == null || _textToFormat == null) return;

        string name = _session.CurrentUser;
        string previousTime = _session.ClearTimes[_session.ActiveSection].ToString("0.00");
        string totalTime = _session.GetTotalTime().ToString("0.00");

        foreach (var text in _textToFormat)
        {
            text.text = text.text.Replace("{name}", name);
            text.text = text.text.Replace("{time}", previousTime);
            text.text = text.text.Replace("{total}", totalTime);
        }
    }
}
