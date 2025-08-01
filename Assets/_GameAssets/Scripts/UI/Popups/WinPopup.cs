using MaskTransitions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPopup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TimerUI _timerUI;
    [SerializeField] private Button _oneMoreButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _timerText;
    void OnEnable()
    {
        _timerText.text = _timerUI.GetFinalTime();

        _oneMoreButton.onClick.AddListener(OnOnMoreButtonClicked);

        _mainMenuButton.onClick.AddListener(() =>
        {
        TransitionManager.Instance.LoadLevel(Consts.SceneNames.MENU_SCENE);
        });
    }
    private void OnOnMoreButtonClicked()
    {
    TransitionManager.Instance.LoadLevel(Consts.SceneNames.GAME_SCENE);
    }
}
