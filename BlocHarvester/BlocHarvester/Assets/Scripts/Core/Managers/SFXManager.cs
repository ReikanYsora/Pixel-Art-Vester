using UnityEngine;

public class SFXManager : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private AudioClip BlocDestructionSFX;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private GameObject ExplosionPrefab;

    #endregion

    #region UNITY METHODS
    private void Start()
    {
        MatrixManager.Instance.OnBlocDestroyed += CBOnBlocDestroyed;
    }
    #endregion

    #region CALLBACKS
    private void CBOnBlocDestroyed(Vector3 position)
    {
        GameObject.Instantiate(ExplosionPrefab, position, Quaternion.identity);
    }

    #endregion
}
