using UnityEngine;

/* Prevents an object from following the parent rotation */
/* Should be attached to a transform */
public class UIDirectionControl : MonoBehaviour
{
    [SerializeField] bool useRelativeRotation = true;

    Quaternion _defaultRotation;

    private void Start()
    {
        // Get the parent rotation
        _defaultRotation = transform.parent.localRotation;
    }


    private void Update()
    {
        // Set the default rotation every frame
        if (useRelativeRotation)
            transform.rotation = _defaultRotation;
    }
}
