using UnityEngine;

public class HolyWheatCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private WheatDesingSO _wheatDesingSO;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _ForceIncrease;
    [SerializeField] private float _resetBoostDuration;

    public void Collect()
    {
        _playerController.SetJumpForce(_wheatDesingSO.IncreaseDecreaseMultiplier, _wheatDesingSO.ResetBoostDuration);
        Destroy(gameObject);
    }
}
