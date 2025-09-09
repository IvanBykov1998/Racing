using UnityEngine;

public class UIRaceCompletion : MonoBehaviour
{
    [SerializeField] private UISelectableButtonContainer buttonsContainer;

    [SerializeField] private RaceInfo[] levels;

    private UIRaceButton[] buttons;

    private int CurrentLevel = 0;

    private void Start()
    {
        buttons = buttonsContainer.GetComponentsInChildren<UIRaceButton>();
        
        SetCurrentLevel();
    }

    private void Update()
    {
        if (active == false)
        {
            EnableLevel();
        }
    }

    private bool active = false;

    private void EnableLevel() // ќтображение и разблокировка уровней
    {
        active = true;

        if (buttons == null)
        {
#if UNITY_EDITOR
            Debug.LogError("Button list is empty!");
#endif
        }

        for (int i = 1; i < buttons.Length; i++)
        {
            buttons[i].Blocked.enabled = true;
            buttons[i].Disable();
        }
        
        if (CurrentLevel > buttons.Length)
        {
            CurrentLevel = buttons.Length;
        }

        for (int i = 0; i < CurrentLevel; i++)
        {
            buttons[i].Blocked.enabled = false;
            buttons[i].Enable();
        }
    }

    private void SetCurrentLevel() // ѕодсчет пройденных уровней через SaveManager
    {
        CurrentLevel = 1;

        for (int i = 0; i < levels.Length; i++)
        {
            if (SaveManager.Instance.IsLevelCompleted(levels[i].SceneName) == true)
            {
                CurrentLevel++;
            }
        }
    }
}