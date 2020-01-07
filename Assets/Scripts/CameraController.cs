using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Vector3 offset = Vector3.zero;
    private Vector3 startPos;
    private Vector3 startRot;

    public Transform depthCamera;
    public Transform blankCamera;

    //public RenderTexture Depth;
    //public RenderTexture Normal;
    private bool active = true;
    // Use this for initialization
    void Start () {
        startPos = transform.position;
        startRot = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        if (active)
        {
            transform.position = startPos + offset;
            transform.rotation = Quaternion.Euler(new Vector3(startRot.x, Input.GetAxis("Horizontal") / 2.0f, startRot.z));
        }
    }

    public IEnumerator LerpPosition(Transform target)
    {
        active = false;
        float t = 0.0f;
        while (t < .95f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + startPos, t);
            t += Time.deltaTime;
            yield return null;
        }
        t = 5.0f;
        while (t > 0.0f)
        {
            transform.position = target.position + startPos;
            t -= Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator screenShake(float intensity, float duration)
    {
        while (duration > 0.0f)
        {
            offset = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) / 5.0f * intensity;
            duration -= Time.deltaTime;
            yield return null;
        }
        offset = Vector2.zero;
    }
}
