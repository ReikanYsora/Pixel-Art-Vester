using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EntityPhysics : MonoBehaviour
{
    #region ATTRIBUTES
    protected Rigidbody _body;
    [Header("Movement settings")]
    [SerializeField] private float maxVelocity = 20.0f;

    [Header("Gravity sensors")]
    [SerializeField] private float levitationMultiplier;
    [SerializeField] private Transform[] levitationAnchors = new Transform[4];
    private RaycastHit[] levitationHits = new RaycastHit[4];

    [Header("Wall avoidance sensors")]
    [SerializeField] private LayerMask _raycastLayer;
    [SerializeField] private float avoidanceLength;
    [SerializeField] private float avoidanceForce = 5.0f;
    [SerializeField] private Transform avoidanceLeftAnchor;
    private RaycastHit avoidanceLeftHit = new RaycastHit();
    [SerializeField] private Transform avoidanceRightAnchor;
    private RaycastHit avoidanceRightHit = new RaycastHit();
    [SerializeField] private Transform avoidanceFrontRightAnchor;
    private RaycastHit avoidanceFrontRightHit = new RaycastHit();
    [SerializeField] private Transform avoidanceFrontLeftAnchor;
    private RaycastHit avoidanceFrontLeftHit = new RaycastHit();
    #endregion

    #region PROPERTIES
    public bool IsActive { set; get; }

    public float MaxVelocity
    {
        set
        {
            maxVelocity = value;
        }
        get
        {
            return maxVelocity;
        }
    }
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        IsActive = true;
        _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ExecuteOnUpdate();
    }

    private void FixedUpdate()
    {
        ExecuteOnFixedUpdate();
    }
    #endregion

    #region METHODS
    protected void ExecuteOnUpdate()
    {
        ManageFall();
    }

    public Rigidbody GetRigidbody()
    {
        return _body;
    }
    protected void ExecuteOnFixedUpdate()
    {
        ManageGravitySensors();
        ManageAvoidanceSensors();
        ManageSpeedLimitation();
    }

    private void ManageGravitySensors()
    {
        for (int i = 0; i < levitationAnchors.Length; i++)
        {
            ApplyHorizontalForce(levitationAnchors[i], levitationHits[i]);
        }
    }
    private void ManageAvoidanceSensors()
    {
        if (avoidanceLeftAnchor != null)
        {
            ApplyAvoidanceLeftForce();
        }

        if (avoidanceRightAnchor != null)
        {
            ApplyAvoidanceRightForce();
        }

        if (avoidanceFrontLeftAnchor != null)
        {
            ApplyAvoidanceFrontLeftForce();
        }

        if (avoidanceFrontRightAnchor != null)
        {
            ApplyAvoidanceFrontRightForce();
        }
    }

    private void ApplyAvoidanceLeftForce()
    {
        if (Physics.Raycast(avoidanceLeftAnchor.position, -avoidanceLeftAnchor.right, out avoidanceLeftHit, avoidanceLength, _raycastLayer))
        {
            _body.AddForceAtPosition(avoidanceLeftAnchor.right * avoidanceForce, avoidanceLeftAnchor.position, ForceMode.VelocityChange);
        }
    }

    private void ApplyAvoidanceRightForce()
    {
        if (Physics.Raycast(avoidanceRightAnchor.position, avoidanceRightAnchor.right, out avoidanceRightHit, avoidanceLength, _raycastLayer))
        {
            _body.AddForceAtPosition(-avoidanceRightAnchor.right * avoidanceForce, avoidanceRightAnchor.position, ForceMode.VelocityChange);
        }
    }

    private void ApplyAvoidanceFrontLeftForce()
    {
        if (Physics.Raycast(avoidanceFrontLeftAnchor.position, avoidanceFrontLeftAnchor.forward, out avoidanceFrontLeftHit, avoidanceLength, _raycastLayer))
        {
            _body.AddForceAtPosition(-avoidanceFrontLeftAnchor.forward * avoidanceForce, avoidanceFrontLeftAnchor.position, ForceMode.VelocityChange);
            _body.AddForceAtPosition(avoidanceLeftAnchor.right * avoidanceForce, avoidanceLeftAnchor.position, ForceMode.VelocityChange);
        }
    }
    public void ApplyForceAtCenter(float force)
    {
        _body.AddExplosionForce(force, transform.position, 500f);
    }

    private void ApplyAvoidanceFrontRightForce()
    {
        if (Physics.Raycast(avoidanceFrontRightAnchor.position, avoidanceFrontRightAnchor.forward, out avoidanceFrontRightHit, avoidanceLength, _raycastLayer))
        {
            _body.AddForceAtPosition(-avoidanceFrontRightAnchor.forward * avoidanceForce, avoidanceFrontRightAnchor.position, ForceMode.VelocityChange);
            _body.AddForceAtPosition(-avoidanceLeftAnchor.right * avoidanceForce, avoidanceRightAnchor.position, ForceMode.VelocityChange);
        }
    }

    private void ApplyHorizontalForce(Transform anchor, RaycastHit hit)
    {
        if (Physics.Raycast(anchor.position, -anchor.up, out hit))
        {
            float force = Mathf.Abs(1 / (hit.point.y - anchor.position.y));
            _body.AddForceAtPosition(transform.up * force * levitationMultiplier, anchor.position, ForceMode.Acceleration);
        }
    }

    private void ManageFall()
    {
        if (transform.position.y <= 0)
        {
            Vector3 position = transform.position;
            position.y = 1.5f;
            transform.position = position;
        }
    }

    private void ManageSpeedLimitation()
    {
        //Regulate max speed
        if (_body.velocity.magnitude > maxVelocity)
        {
            _body.velocity = _body.velocity.normalized * maxVelocity;
        }
    }
    #endregion
}
