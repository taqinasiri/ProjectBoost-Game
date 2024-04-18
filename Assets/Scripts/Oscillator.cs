using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVectore;
    [SerializeField] private float period = 2f;
    private Vector3 startingPosition;

    private const float tau = Mathf.PI * 2;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        if(period <= Mathf.Epsilon)
        {
            return;
        }
        float cycles = Time.time / period; // continually growing over time
        float rawSinWave = Mathf.Sin(cycles * tau); // get a number between -1 ~ 1

        float movementFactor = (rawSinWave + 1f) / 2;  // recalculate to get a number between 0 ~ 1
        Vector3 offset = movementVectore * movementFactor;
        transform.position = startingPosition + offset;
    }
}