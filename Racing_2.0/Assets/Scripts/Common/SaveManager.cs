using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static string SaveMark = "_level_completed";

    private int levelCompleted = 0; // 1 - пройдет, 0 - не пройден

    //Статическая ссылка на единственный экземпляр
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Объект останется активен при смене сцен

        Load();
    }

    private int Load() // Загружаем информацию о пройденных уровнях
    {
        levelCompleted = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + SaveMark, 0);
        return levelCompleted;
    }

    public bool IsLevelCompleted(string sceneName) // Проверка пройден ли уровень, вызывается в UIRaceCompletion
    {
        return PlayerPrefs.GetInt(sceneName + SaveMark, 0) > 0;
    }

    public void Save(float time) // Сохраняем пройден ли уровень или нет вызывается в RaceResultTime
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