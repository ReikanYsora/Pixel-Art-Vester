using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BulletLineRenderer : MonoBehaviour
{
    #region ATTRIBUTES
    private LineRenderer _lineRenderer;                                                                 //Line renderer
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
    #endregion

    #region METHODS
    /// <summary>
    /// METHOD : Initialize bullet line renderer
    /// </summary>
    /// <param name="origin">Origin/param>
    /// <param name="destination">Destination</param>
    /// <param name="lifeTime">Life time</param>
    public void Initialize(Vector3 origin, Vector3 destination)
    {
        Vector3[] positions = new Vector3[]
        {
            origin,
            destination
        };

        _lineRenderer.SetPositions(positions);
    }
    #endregion
}
