using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static string SaveMark = "_level_completed";

    private int levelCompleted = 0; // 1 - �������, 0 - �� �������

    //����������� ������ �� ������������ ���������
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // ������ ��������� ������� ��� ����� ����

        Load();
    }

    private int Load() // ��������� ���������� � ���������� �������
    {
        levelCompleted = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + SaveMark, 0);
        return levelCompleted;
    }

    public bool IsLevelCompleted(string sceneName) // �������� ������� �� �������, ���������� � UIRaceCompletion
    {
        return PlayerPrefs.GetInt(sceneName + SaveMark, 0) > 0;
    }

    public void Save(float time) // ��������� ������� �� ������� ��� ��� ���������� � RaceResultTime
    {
        if (time <= 0)
        {
            levelCompleted = 0;
        }
        else
        {
            levelCompleted = 1;
        }

        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + SaveMark, levelCompleted);
    }
}