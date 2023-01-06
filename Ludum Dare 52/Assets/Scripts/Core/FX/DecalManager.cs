using System.Collections.Generic;
using UnityEngine;

public class DecalManager : MonoBehaviour
{
    #region ATTRIBUTES
    [Header("Visual FX")]
    [SerializeField] private Sprite[] _decalSprites;                                             //Decal sprites
    [SerializeField] private GameObject _decalPrefab;                                            //Decal prefab
    private Queue<GameObject> _decals;                                                           //In game decals
    [SerializeField] private int _maxDecals;                                                     //Max decal in game

    [Header("Sprays")]
    [SerializeField] private Sprite[] _spraySprites;                                             //Spray sprites
    private Queue<GameObject> _sprays;                                                           //In game sprays
    [SerializeField] private int _maxSprays;                                                     //Max decal in game
    private int _selectedSpray;                                                                  //Selected spray
    #endregion 

    #region PROPERTIES
    /// <summary>
    /// PROPERTY : Manager instance for direct access
    /// </summary>
    public static DecalManager Instance { get; private set; }

    /// <summary>
    /// PROPERTY : Get selected spray
    /// </summary>
    public Sprite SelectedSpray
    {
        get
        {
            return _spraySprites[_selectedSpray];
        }
    }

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
        _decals = new Queue<GameObject>();
        _sprays = new Queue<GameObject>();
    }
    #endregion

    #region METHODS
    /// <summary>
    /// METHOD : Create decal at position
    /// </summary>
    /// <param name="position">Position</param>
    public void CreateDecalAtPosition(Vector3 position)
    {
        Vector3 tempPosition = position;
        tempPosition.y = 0f;

        if ((_decals.Count == _maxDecals) && (_maxDecals > 0))
        {
            GameObject oldDecal = _decals.Dequeue();
            StartCoroutine(oldDecal.GetComponent<DecalElement>().Fade());
        }

        GameObject _instanceDecal = Instantiate(_decalPrefab);
        DecalElement tempDecal = _instanceDecal.GetComponent<DecalElement>();
        tempDecal.SetSprite(_decalSprites[0]);
        tempDecal.transform.position = tempPosition;
        _decals.Enqueue(_instanceDecal);
    }

    /// <summary>
    /// METHOD : Create spray at position
    /// </summary>
    /// <param name="position">Position</param>
    public void CreateSprayAtPosition(Vector3 position)
    {
        Vector3 tempPosition = position;
        tempPosition.y = 0f;

        if ((_sprays.Count == _maxSprays) && (_maxSprays > 0))
        {
            GameObject oldSpray = _sprays.Dequeue();
            StartCoroutine(oldSpray.GetComponent<DecalElement>().Fade());
        }

        GameObject _instanceSpray = Instantiate(_decalPrefab);
        DecalElement tempDecal = _instanceSpray.GetComponent<DecalElement>();
        tempDecal.SetSprite(_spraySprites[_selectedSpray]);
        tempDecal.transform.position = tempPosition;
        _sprays.Enqueue(_instanceSpray);

        FXManager.Instance.CreateBulletHitImpactEffect(_instanceSpray.transform.position);
    }

    /// <summary>
    /// METHOD : Set next spray sprite
    /// </summary>
    public void SetNextSpray()
    {
        if (_selectedSpray < _spraySprites.Length - 1)
        {
            _selectedSpray++;
        }
        else
        {
            _selectedSpray = 0;
        }
    }

    /// <summary>
    /// METHOD : Set previous spray sprite
    /// </summary>
    public void SetPreviousSpray()
    {
        if (_selectedSpray > 0)
        {
            _selectedSpray--;
        }
        else
        {
            _selectedSpray = _spraySprites.Length - 1;
        }
    }
    #endregion
}
