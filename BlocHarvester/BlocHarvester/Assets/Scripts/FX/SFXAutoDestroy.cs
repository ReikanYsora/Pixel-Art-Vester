using UnityEngine;

public class SFXAutoDestroy : MonoBehaviour
{
    #region UNITY METHODS
    private void Awake()
    {
        AudioSource audio = GetComponent<AudioSource>();

        if (audio == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, audio.clip.length);
        }
    }
    #endregion
}
