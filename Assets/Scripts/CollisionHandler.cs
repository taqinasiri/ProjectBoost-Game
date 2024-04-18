using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 1f;
    [SerializeField] private AudioClip successClip;
    [SerializeField] private AudioClip crashClip;
    [SerializeField] private ParticleSystem successPatricles;
    [SerializeField] private ParticleSystem crashPatricles;

    private AudioSource audioSource;

    private bool isTransitioning = false;
    private bool collisionDisable = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collisionDisable || isTransitioning)
            return;

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
                StartSuccessSequnce();

                break;

            default:
                StartCrashSequnce();
                break;
        }
    }

    private void StartCrashSequnce()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crashClip);
        crashPatricles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke(nameof(ReloadLevel),levelLoadDelay);
    }

    private void StartSuccessSequnce()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(successClip);
        successPatricles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke(nameof(LoadNextLevel),levelLoadDelay);
    }

    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}