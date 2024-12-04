using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    private int PLATFORMS_NUM = 8;
    private GameObject[] platforms;
    private Vector2[] positions;
    private Vector3[] DstPositions;
    [SerializeField] private float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] DstPositions = new Vector3[PLATFORMS_NUM];
        int j;
        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            j = i + 1;
            if (j == PLATFORMS_NUM)
            {
                j = 0;
            }
            DstPositions[i] = new Vector3(
                positions[j].x,
                positions[j].y,
                0
                );
        }


        for (int i = 0; i < PLATFORMS_NUM; i++) {
            Vector3.MoveTowards(platforms[i].transform.position, DstPositions[i], speed * Time.deltaTime);
        }
    }

    void Awake()
    {
        Vector2 center = new Vector2(21.0f, 21.0f);
        float radius = 5.0f;

        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector2[PLATFORMS_NUM];

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            float angle = 2 * Mathf.PI * i / PLATFORMS_NUM;
            positions[i] = new Vector2(
                center.x + radius * Mathf.Cos(angle), // X-coordinate
                center.y + radius * Mathf.Sin(angle)
                );
        }

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
        }

    }
}
