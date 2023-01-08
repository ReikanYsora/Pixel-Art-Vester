using UnityEngine;

public class SFXManager : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private GameObject ExplosionPrefab;
    [SerializeField] private AudioClip _clicSFX;
    [SerializeField] private AudioClip _pauseSFX;
    [SerializeField] private AudioClip _resumeSFX;
    [SerializeField] private AudioSource _audioSource;
    #endregion

    #region PROPERTIES
    public static SFXManager Instance { get; private set; }
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameManager.Instance.OnResumed += CBOnResumed;
        GameManager.Instance.OnPaused += CBOnPaused;

        MatrixManager.Instance.OnBlocDestroyed += CBOnBlocDestroyed;
    }

    private void CBOnPaused()
    {
        _audioSource.PlayOneShot(_pauseSFX);
    }

    private void CBOnResumed()
    {
        _audioSource.PlayOneShot(_resumeSFX);
    }
    #endregion

    #region METHODS
    private void PlayUISound()
    {
        _audioSource.PlayOneShot(_clicSFX);
    }
    #endregion

    #region CALLBACKS
    private void CBOnBlocDestroyed(Vector3 position)
    {
        GameObject tempObject = GameObject.Instantiate(ExplosionPrefab, position, Quaternion.identity);
        AudioSource tempAudio = tempObject.GetComponent<AudioSource>();

        if (tempAudio != null)
        {
            Destroy(tempObject, tempAudio.clip.length);
        }
        else
        {
            Destroy(tempObject, 10.0f);
        }
    }

    #endregion
}
