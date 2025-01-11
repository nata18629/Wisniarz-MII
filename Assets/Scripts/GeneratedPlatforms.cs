using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    private int PLATFORMS_NUM = 8;
    private GameObject[] platforms;
    private Vector2[] positions;
    private Vector2[] DstPositions;
    private Vector2[] TmpPositions;
    [SerializeField] private float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2[] DstPositions = new Vector2[PLATFORMS_NUM];
        int nextIndex;

        // Calculate the destination positions for circular movement
        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            nextIndex = (i + 1) % PLATFORMS_NUM; // Ensure circular indexing
            DstPositions[i] = new Vector2(
                positions[nextIndex].x,
                positions[nextIndex].y
            );
        }

        // Move platforms towards their calculated destinations
        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            platforms[i].transform.position = Vector2.MoveTowards(
                platforms[i].transform.position,
                DstPositions[i],
                speed * Time.deltaTime
            );
        }

        if (Vector2.Distance(platforms[0].transform.position, DstPositions[0]) < 0.1f)
        {

            for (int i = 0; i < PLATFORMS_NUM; i++)
            {
                nextIndex = (i + 1) % PLATFORMS_NUM;
                TmpPositions[i] = positions[nextIndex];
            }
            for(int i = 0;i < PLATFORMS_NUM; i++)
            {
                positions[i] = TmpPositions[i];
            }
        }
    }

    void Awake()
    {
        Vector2 center = new Vector2(17.0f, 21.0f);
        float radius = 8.0f;

        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector2[PLATFORMS_NUM];
        TmpPositions = new Vector2[PLATFORMS_NUM];

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            float angle = 2 * Mathf.PI * i / PLATFORMS_NUM;
            positions[i] = new Vector2(
                center.x + radius * Mathf.Cos(angle), // X-coordinate
                center.y + radius * Mathf.Sin(angle)
                );
            TmpPositions[i]= positions[i];
        }

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
        }

    }
}
