using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] plants;

    BoxCollider2D boxCollider;

    public int plantCount;
    public float spacingX;
    public float spacingY;
    public bool sizeVar;
    public float maxSize;
    public float minSize;

    Vector3 plantPos;
    float height;
    float width;

    List<Vector3> plantLocations = new List<Vector3>();

    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = !enabled;
        boxCollider = GetComponent<BoxCollider2D>();

        for (int i = 0; i < plantCount; i++)
        {
            width = boxCollider.bounds.extents.x;
            height = boxCollider.bounds.extents.y;
            plantPos = new Vector3(
                Mathf.Round(Random.Range(-width, width) / spacingX) * spacingX
                + transform.position.x + boxCollider.bounds.extents.x,
                Mathf.Round(Random.Range(-height, height) / spacingY) * spacingY + transform.position.y, 0f);

            if (!plantLocations.Contains(plantPos))
            {
                if (sizeVar == true)
                {
                    int whichplant = Random.Range(0, plants.Length);
                    float randomScale = Random.Range(minSize, maxSize);
                    Instantiate(plants[whichplant], plantPos, Quaternion.Euler(0f, 0f, 0f));
                    Vector3 scale = plants[whichplant].transform.localScale;
                    scale.x = randomScale;
                    scale.y = randomScale;
                    plants[whichplant].transform.localScale = scale;

                }else if (sizeVar == false)
                {
                    Instantiate(plants[Random.Range(0, plants.Length)], plantPos, Quaternion.Euler(0f, 0f, 0f));
                }
                plantLocations.Add(plantPos);
            }
        }
    }
}
        