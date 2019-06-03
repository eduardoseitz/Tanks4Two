using UnityEngine;

/* Controls the player movement */
/* Should be attached to player */
public class TankMovement : MonoBehaviour
{
    #region Declarations
    [Range(1, 4)]public int playerId = 1;

    [SerializeField] string movementInputName = "Vertical";
    [SerializeField] string turnInputName = "Horizontal";
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float turnSpeed = 2f;

    [SerializeField] AudioSource engineSource;
    [SerializeField] AudioClip clipEngineIdling;
    [SerializeField] AudioClip clipEngineDriving;
    [SerializeField] [Range(0f, 1f)] float pitchVariation;

    float _movementInput;
    float _turnInput;
    float _originalPitch;
    Rigidbody _rbPlayer;
    #endregion

    #region Main Methods
    private void Awake()
    {
        // Links the player rigidbody
        _rbPlayer = GetComponent<Rigidbody>();

        // Get the original pitch value
        _originalPitch = engineSource.pitch;
    }

    private void Start()
    {
        EnableMovement();
    }

    private void FixedUpdate()
    {
        // Move and turn the tank
        MovePlayer();
        TurnPlayer();
    }

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing
        _movementInput = Input.GetAxisRaw(movementInputName + playerId);
        _turnInput = Input.GetAxisRaw(turnInputName + playerId);

        SwitchEngineAudio();
    }
    #endregion

    #region Helper Methods
    // The movement is enabled when the match starts
    public void EnableMovement()
    {
        _rbPlayer.isKinematic = false;
    }

    // The movement is disabled when the player loses or the match ends
    public void DisableMovement()
    {
        _rbPlayer.isKinematic = true;
    }

    // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing
    private void SwitchEngineAudio()
    {
        // If player is idling set the audio to idle
        if ((Mathf.Abs(_movementInput) < 0.1f) && (Mathf.Abs(_turnInput) < 0.1f))
        {
            if (engineSource.clip == clipEngineDriving)
            {
                engineSource.clip = clipEngineIdling;
                engineSource.pitch = Random.Range(_originalPitch - pitchVariation, _originalPitch + pitchVariation);
                engineSource.Play();
            }
        }
        // If player is moving set the audio to running
        else
        {
            if (engineSource.clip == clipEngineIdling)
            {
                engineSource.clip = clipEngineDriving;
                engineSource.pitch = Random.Range(_originalPitch - pitchVariation, _originalPitch + pitchVariation);
                engineSource.Play();
            }
        }
    }

    // Adjust the position of the tank based on the player's input
    private void MovePlayer()
    {
        // Calculate player speed
        float _speed = _movementInput * movementSpeed * Time.deltaTime;

        // Apply speed to the player rigidbody
        _rbPlayer.MovePosition(_rbPlayer.position + transform.forward * _speed);
    }


    // Adjust the rotation of the tank based on the player's input
    private void TurnPlayer()
    {
        float _angle = _turnInput * turnSpeed * Time.deltaTime * 10;
        _rbPlayer.MoveRotation(_rbPlayer.rotation * Quaternion.Euler(0, _angle, 0));
    }
    #endregion
}
