using DG.Tweening;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private GameObject _directionIndicatorUp;
    [SerializeField] private GameObject _directionIndicatorDown;
    [SerializeField] private GameObject _directionIndicatorRight;
    [SerializeField] private GameObject _directionIndicatorLeft;
    [SerializeField] private float _colorDuration = 0.25f;
    #endregion

    #region UNITY METHODS
    private void Start()
    {
        PlayerManager.Instance.OnDirectionChanged += CBOnDirectionChanged;
    }

    private void ShowDirectionEffect(GameObject go)
    {
        go.GetComponent<Renderer>().material.DOColor(Color.red, _colorDuration).OnComplete(() =>
        {
            go.GetComponent<Renderer>().material.DOColor(Color.white, _colorDuration);
        });

        go.transform.DOScale(2f, _colorDuration).OnComplete(() =>
        {
            go.transform.DOScale(1f, _colorDuration);
        });
    }
    #endregion

    #region CALLBACKS
    private void CBOnDirectionChanged(PlayerManager.Direction direction)
    {
        DOTween.CompleteAll(false);

        switch (direction)
        {
            default:
                break;
            case PlayerManager.Direction.Up:
                ShowDirectionEffect(_directionIndicatorUp);
                break;
            case PlayerManager.Direction.Down:
                ShowDirectionEffect(_directionIndicatorDown);
                break;
            case PlayerManager.Direction.Right:
                ShowDirectionEffect(_directionIndicatorRight);
                break;
            case PlayerManager.Direction.Left:
                ShowDirectionEffect(_directionIndicatorLeft);
                break;
        }
    }
    #endregion
}
