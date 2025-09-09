using UnityEngine;
using UnityEngine.UI;

public class UIRaceResultPanel : MonoBehaviour, IDependency<RaceResultTime>
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Text recordTimeText;
    [SerializeField] private Text currentTimeText;

    private RaceResultTime raceResultTime;
    public void Construct(RaceResultTime obj) => raceResultTime = obj;

    private void Start()
    {
        resultPanel.SetActive(false);

        raceResultTime.ResultUpdated += OnUpdateResults;
    }

    private void OnDestroy()
    {
        raceResultTime.ResultUpdated -= OnUpdateResults;
    }

    private void OnUpdateResults()
    {
        resultPanel.SetActive(true);

        recordTimeText.text += StringTime.SecondToTimeString(raceResultTime.GetAbsoluteRecord());
        currentTimeText.text += StringTime.SecondToTimeString(raceResultTime.CurrentTime);
    }
}