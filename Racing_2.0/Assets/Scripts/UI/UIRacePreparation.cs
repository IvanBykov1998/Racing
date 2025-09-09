using UnityEngine;
using UnityEngine.UI;

public class UIRacePreparation : MonoBehaviour, IDependency<RaceStateTracker>
{
    [SerializeField] private GameObject panelText;
    [SerializeField] private Text startingText;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private void Start()
    {
        raceStateTracker.PreparationStarted += OnPreparationStarted;

        panelText.SetActive(true);
        startingText.enabled = true;
        enabled = true;
    }

    private void OnDestroy()
    {
        raceStateTracker.PreparationStarted -= OnPreparationStarted;
    }

    private void OnPreparationStarted()
    {
        panelText.SetActive(false);
        startingText.enabled = false;
        enabled = false;
    }

}