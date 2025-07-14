using UnityEngine;

public class RottenWheatCollectible : MonoBehaviour, ICollectible
{ 
    [SerializeField] private WheatDesingSO _wheatDesingSO;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _movementDecreaseSpeed;
    [SerializeField] private float _resetBoostDuration;

    public void Collect()
    {
        _playerController.SetMovementSpeed(_wheatDesingSO.IncreaseDecreaseMultiplier, _wheatDesingSO.ResetBoostDuration);
        Destroy(gameObject);
    }
}
