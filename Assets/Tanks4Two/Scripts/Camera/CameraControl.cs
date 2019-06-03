using UnityEngine;

/* Controls the ortographic camera behaviour of a child camera */
/* Should be attached to camera rig */
public class CameraControl : MonoBehaviour
{
    #region Declarations
    public float _DampTime = 0.2f;                 
    public float _ScreenEdgeBuffer = 4f;           
    public float _MinSize = 6.5f;                  
    [HideInInspector] public Transform[] Targets; 
    
    Camera _Camera;                        
    float _ZoomSpeed;                      
    Vector3 _MoveVelocity;                 
    Vector3 _DesiredPosition;
    #endregion

    #region Main Methods
    private void Awake()
    {
        _Camera = GetComponentInChildren<Camera>();
    }

    private void FixedUpdate()
    {
        Move();
        Zoom();
    }
    #endregion

    #region Helper Methods
    // Gets called by game manager every round to reset the camera position and size
    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = _DesiredPosition;

        _Camera.orthographicSize = FindRequiredSize();
    }

    // Change the camera position
    private void Move()
    {
        FindAveragePosition();
        transform.position = Vector3.SmoothDamp(transform.position, _DesiredPosition, ref _MoveVelocity, _DampTime);
    }

    // Finds the average position between n players
    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < Targets.Length; i++)
        {
            // If the player is not active than skip the the next loop iteration
            if (!Targets[i].gameObject.activeSelf)
                continue;

            // If the player is active than add its position to the average position
            averagePos += Targets[i].position;
            numTargets++;
        }

        // Divide the position by the number of players
        if (numTargets > 0)
        {
            averagePos /= numTargets;
        }

        // Make sure that the camera y does not get changed
        averagePos.y = transform.position.y;

        // Set the average position to the camera
        _DesiredPosition = averagePos;
    }

    //
    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        _Camera.orthographicSize = Mathf.SmoothDamp(_Camera.orthographicSize, requiredSize, ref _ZoomSpeed, _DampTime);
    }

    // Finds the desired camera size / zoom
    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(_DesiredPosition);

        float size = 0f;

        // Check each of the player is further away
        for (int i = 0; i < Targets.Length; i++)
        {
            if (!Targets[i].gameObject.activeSelf)
                continue;

            // Caculate the size so that players are in the camera screen
            Vector3 targetLocalPos = transform.InverseTransformPoint(Targets[i].position);
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            // Size is set to the current value or to the new desired value
            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));

            // Decides if x or y have a greater size
            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / _Camera.aspect);
        }
        
        // Adds a margin to the screen size
        size += _ScreenEdgeBuffer;

        // If the new size is small than the minimum size than set its minumum size
        size = Mathf.Max(size, _MinSize);
        
        return size;
    }
    #endregion
}
