using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _timerRotatableTransform;
    [SerializeField] private TMP_Text _timerText;

    [Header("Settings")]
    [SerializeField] private float _rotationDuration;
    [SerializeField] private Ease _roationEase;

    private float _elapsedTime;

    private void Start()
    {
        PlayerRotationAnimation();
        StartTimer();
    }
    private void PlayerRotationAnimation()
    {
        _timerRotatableTransform.DORotate(new Vector3(0f, 0f, -360f), _rotationDuration, RotateMode.FastBeyond360)
        .SetLoops(-1, LoopType.Restart)
        .SetEase(_roationEase);
    }
    private void StartTimer()
    {
        _elapsedTime = 0f;
        InvokeRepeating(nameof(UptadeTimerIU), 0f, 1f);
    }
    private void UptadeTimerIU()
    {
        _elapsedTime += 1f;

        int minutes = Mathf.FloorToInt(_elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60f);

        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
