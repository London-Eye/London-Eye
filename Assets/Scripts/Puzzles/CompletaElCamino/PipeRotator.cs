using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PipeRotator : MonoBehaviour
{
    public Transform Pipe;

    public const float RotationAngle = -90f;

    public float RotationSpeed = 100f;

    public float IncreaseSpeedFactorPerRequest = 1.5f;

    public UnityEvent OnRotationEnded;

    private float currentRotationSpeed;

    private int pendantRotations;

    private bool ready;

    private PipeSetter controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("ComponentContainer").GetComponent<PipeSetter>();
        currentRotationSpeed = RotationSpeed;
        pendantRotations = 0;
        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (ready)
        {
            if (pendantRotations > 0)
            {
                pendantRotations--;
                StartRotating();         
            }
            else
            {
                currentRotationSpeed = RotationSpeed;
            }
        }
    }

    private void OnMouseDown()
    {
        if (!controller.pause.activeSelf)
        {
            pendantRotations++;
            if (!ready) currentRotationSpeed *= IncreaseSpeedFactorPerRequest;
        }
    }

    private void StartRotating()
    {
        ready = false;
        StartCoroutine(RotateSmoothly());
    }

    private IEnumerator RotateSmoothly()
    {
        Quaternion targetRotation = Quaternion.AngleAxis(RotationAngle, Vector3.forward) * Pipe.transform.rotation;
        while (Pipe.transform.rotation != targetRotation)
        {
            Pipe.transform.rotation = Quaternion.RotateTowards(Pipe.transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Pipe.transform.rotation = targetRotation;
        EndRotation();
        yield return null;
    }

    private void EndRotation()
    {
        ready = true;
        OnRotationEnded?.Invoke();
    }
}
