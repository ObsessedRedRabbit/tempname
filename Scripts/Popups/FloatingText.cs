using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Text text;
    public float scale;
    public float DestroyTime = 1f;
    public Vector3 ScaleChange;
    public Vector3 Offset = new Vector3(0, 2, 0);
    public Vector3 RandomizeIntensity = new Vector3(1f, 1f, 0);

    void Start()
    {
        Destroy(gameObject, DestroyTime);

        if (text != null)
        {
            scale = float.Parse(text.text);
        }

        transform.localPosition += Offset;
        transform.localScale += ScaleChange;
        ScaleChange = new Vector3(scale, scale, scale);
        transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
        Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y), 
        Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));
    }
}
