using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private GameObject ExplosionPrefab;
    private Queue<AudioSource> EnqueuedSFX;

    #endregion

    #region UNITY METHODS
    private void Start()
    {
        EnqueuedSFX = new Queue<AudioSource>();
        MatrixManager.Instance.OnBlocDestroyed += CBOnBlocDestroyed;
    }
    #endregion

    #region CALLBACKS
    private void CBOnBlocDestroyed(Vector3 position)
    {
        if (EnqueuedSFX.Count > 10)
        {
            Destroy(EnqueuedSFX.Dequeue());
        }

        GameObject tempObject = GameObject.Instantiate(ExplosionPrefab, position, Quaternion.identity);
        AudioSource tempAudio = tempObject.GetComponent<AudioSource>();

        if (tempAudio != null)
        {
            EnqueuedSFX.Enqueue(tempAudio);
            Destroy(tempObject, tempAudio.clip.length);
        }
    }

    #endregion
}
