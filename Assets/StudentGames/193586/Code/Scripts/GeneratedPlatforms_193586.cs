using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] public GameObject platformPrefab;
    private const int PLATFORMS_NUM = 10;
    public GameObject[] platforms;
    public Vector3[] positions;
    private const float radius = 8;
    private const float offset_x = -40;
    private const float offset_y = 2.5f;
    private const float speed = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            // Calculate the index of the next position in a circular manner
            int nextIndex = (i + 1) % PLATFORMS_NUM;

            // Use PingPong to create continuous back-and-forth motion between current and next positions
            float t = Mathf.PingPong(Time.time * speed, 1f);
            Vector3 targetPosition = Vector3.Lerp(positions[i], positions[nextIndex], t);

            // Update the platform's position
            platforms[i].transform.position = targetPosition;

            //Debug.Log("Platform " + i + " moved to position: " + targetPosition + " from position " + positions[i]);
        }
    }

    private void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[PLATFORMS_NUM];
        
        for (int i = 0; i < platforms.Length; i++)
        {
            float angle = i / (float)platforms.Length * (2 * Mathf.PI);
            positions[i] = new Vector3(offset_x + Mathf.Cos(angle) * radius, offset_y + Mathf.Sin(angle) * radius, 0);
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
            //Debug.Log("Platform " + i + " created at position: " + positions[i]);
        }
    }
}
