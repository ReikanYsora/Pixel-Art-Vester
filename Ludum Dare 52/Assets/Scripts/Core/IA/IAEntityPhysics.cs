using System.Collections.Generic;
using UnityEngine;

public class IAEntityPhysics : EntityPhysics
{
    #region ATTRIBUTES
    [Header("Movement settings")]
    [SerializeField] private float moveForce;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float driftDistance = 2f;
    [SerializeField] private GameObject _activityLight;
    #endregion

    #region PROPERTIES

    public List<Vector3> Path { set; get; }

    public Vector3 NextPosition { set; get; }

    public float RotationSpeed
    {
        set
        {
            if ((rotationSpeed != value) && (rotationSpeed >= 0))
            {
                rotationSpeed = value;
            }
        }
        get
        {
            return rotationSpeed;
        }
    }
    #endregion

    #region UNITY METHODS
    private void Update()
    {
        ExecuteOnUpdate();

        if (_activityLight != null)
        {
            _activityLight.SetActive(IsActive);
        }

        if (IsActive)
        {
            ManageTargetDestination();
        }
    }

    private void FixedUpdate()
    {
        ExecuteOnFixedUpdate();

        if (IsActive)
        {
            ManageRotation();
            ManagePropulsion();
        }
    }
    #endregion

    #region METHODS
    private void ManagePropulsion()
    {
        if (NextPosition != Vector3.zero)
        {
            _body.AddForce(transform.TransformDirection(Vector3.forward) * moveForce);
        }
    }

    private void ManageRotation()
    {
        Vector3 lookTo = NextPosition - transform.position;

        if ((NextPosition != Vector3.zero) && (lookTo != Vector3.zero))
        {
            Quaternion lookOnLook = Quaternion.LookRotation(lookTo);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.fixedDeltaTime * rotationSpeed);
        }
    }

    private void ManageTargetDestination()
    {
        if (Vector3.Distance(NextPosition, transform.position) <= driftDistance)
        {
            if ((Path != null) && (Path.Count > 0))
            {
                NextPosition = Path[0];
                Path.Remove(NextPosition);
            }
        }
    }

    public void SearchPathTo(HexData targetTile)
    {
        if (targetTile != null)
        {
            Queue<System.Guid> result = HexPathManager.Instance.FindPath(new PathfindingOperation
            {
                TargetNode = HexGridManager.Instance.GetNearestTile(transform.position).GUID,
                StartNode = targetTile.GUID
            });

            Path = new List<Vector3>();

            while (result.Count > 0)
            {
                HexData tempData = HexGridManager.Instance.GetHexTile(result.Dequeue());
                Vector3 position = tempData.Position;
                position.y = transform.position.y;
                Path.Add(position);
            }
        }

        if ((Path != null) && (Path.Count > 0))
        {
            NextPosition = Path[0];
            Path.Remove(NextPosition);
        }
        else
        {
            NextPosition = Vector3.zero;
        }
    }

    public void ClearPath()
    {
        Path.Clear();
        NextPosition = Vector3.zero;
    }
    #endregion
}
