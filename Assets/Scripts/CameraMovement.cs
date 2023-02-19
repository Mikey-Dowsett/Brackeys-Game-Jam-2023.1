using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform player;
    [SerializeField] Transform camBody;

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position - new Vector3(0, 0, 10);

        if (Time.timeScale == 0)
        {
            StopAllCoroutines();
        }
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = player.position - new Vector3(0, 0, 10);

        float elaspedTime = 0f;

        while (elaspedTime < duration)
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;

            camBody.position = player.position + new Vector3(xOffset, yOffset, originalPos.z);

            elaspedTime += Time.deltaTime;

            yield return null;
        }
        camBody.position = player.position - new Vector3(0, 0, 10);
    }
}
