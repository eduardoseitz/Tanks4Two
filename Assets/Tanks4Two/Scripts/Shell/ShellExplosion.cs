using UnityEngine;

/* Controls projectile behaviour and player damage when hit */
/* Should be attached to projectile */
public class ShellExplosion : MonoBehaviour
{
    #region Declarations
    [SerializeField] float explosionForce = 1000;
    [SerializeField] float exlosionMaxDamage = 100;
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] float explosionLifeTime = 2f;

    [SerializeField] ParticleSystem particleExplosion;
    [SerializeField] AudioSource sourceExplosion;
    [SerializeField] Light lightExplosion;
    [SerializeField] LayerMask layerPlayer;

    #endregion

    #region Main Methods
    private void Start()
    {
        Destroy(this.gameObject, explosionLifeTime);
    }

    //
    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them
        Collider[] _colliders = Physics.OverlapSphere(transform.position, explosionRadius, layerPlayer);

        // Loop through each gameObject collided with player tag
        foreach (Collider player in _colliders)
        {
            // Safe check if player has a rigidbody
            if (!player.attachedRigidbody)
                continue;

            // Apply explosion force to player rigidbody
            player.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius);

            // If there is a health component than deal damage
            TankHealth _tankHealth = player.GetComponent<TankHealth>();
            if (_tankHealth)
            {
                _tankHealth.TakeDamage(CalculateDamage(player.transform.position));
            }
        }
        
        sourceExplosion.transform.parent = null;
        sourceExplosion.Play();
        lightExplosion.enabled = true;
        particleExplosion.Play();
        Destroy(sourceExplosion, explosionLifeTime);

        Destroy(this.gameObject, explosionLifeTime);
    }
    #endregion

    #region Main Methods
    // Calculate the amount of damage a target should take based on it's position
    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate target distance
        Vector3 _targetDistance = targetPosition - transform.position;
        float _explosionDistance = _targetDistance.magnitude;
        float _relativeDistance = (explosionRadius - _explosionDistance) / explosionRadius;

        // Calculate damage based on distance and max damage
        float _calculatedDamage = Mathf.Max(0, _relativeDistance * exlosionMaxDamage);
        
        return _calculatedDamage;
    }
    #endregion
}
