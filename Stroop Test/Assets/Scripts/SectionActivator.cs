using UnityEngine;

public class SectionActivator : MonoBehaviour
{
    [SerializeField] [Range(1, 3)] private int _currentSection;
    [SerializeField] private GameObject _sectionToActivate;

    // Called by countdown animation event
    public void ActivateSection()
    {
        _sectionToActivate.SetActive(true);
        gameObject.SetActive(false);
        SessionController.Instance.StartCountingTime(_currentSection);
    }
}
