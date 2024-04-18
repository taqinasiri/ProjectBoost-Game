using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float mainThrust = 1f;
    [SerializeField] private float rotationThrust = 1f;
    [SerializeField] private AudioClip mainEngine;

    [SerializeField] private ParticleSystem mainEngineParticles;
    [SerializeField] private ParticleSystem rightThrusterParticles;
    [SerializeField] private ParticleSystem leftThrusterParticles;

    private Rigidbody rigidbody;
    private AudioSource audioSource;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    #region Thrust

    private void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void StartThrusting()
    {
        rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if(!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if(!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }

    #endregion Thrust

    #region Rotate

    private void ProcessRotation()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            RotateLeft();
        }
        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    private void StopRotating()
    {
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }

    private void RotateRight()
    {
        if(!leftThrusterParticles.isPlaying)
        {
            leftThrusterParticles.Play();
        }
        ApplyRotation(-rotationThrust);
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationThrust);

        if(!rightThrusterParticles.isPlaying)
        {
            rightThrusterParticles.Play();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rigidbody.freezeRotation = false;
    }

    #endregion Rotate
}