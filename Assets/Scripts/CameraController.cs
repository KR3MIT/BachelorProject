using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform endPos;
    public Transform startPos;

    public float travelDuration = 1f; // Duration of the camera movement in seconds

    private Camera camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = Camera.main;
        startPos = camera.transform;
    }

    public IEnumerator MoveCamera()
    {
        float elapsedTime = 0f;
        float duration = 1f; // Duration of the camera movement in seconds
        while (elapsedTime < duration)
        {
            camera.transform.position = Vector3.Lerp(startPos.position, endPos.position, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        // Ensure the camera reaches the exact end position at the end of the movement
        camera.transform.position = endPos.position;
    }
}
