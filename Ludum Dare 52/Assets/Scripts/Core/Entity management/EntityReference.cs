using UnityEngine;

public class EntityReference : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private EntityBase _entityBase;
    [SerializeField] private EntityPhysics _entityPhysics;
    [SerializeField] private Transform _root;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// PROPERTY : Return EntityPhysics scrpit link
    /// </summary>
    public EntityPhysics EntityPhysics { get { return _entityPhysics; } }


    /// <summary>
    /// PROPERTY : Return EntityBase scrpit link
    /// </summary>
    public EntityBase EntityBase { get { return _entityBase; } }

    /// <summary>
    /// PROPERTY : Return EntityBase scrpit link
    /// </summary>
    public Transform Root { get { return _root; } }
    #endregion
}
