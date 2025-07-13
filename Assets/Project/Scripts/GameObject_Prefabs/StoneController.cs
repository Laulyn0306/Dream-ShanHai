using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour
{

    public GameObject glowLayer;
    // Start is called before the first frame update
    void Start()
    {
        glowLayer.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            glowLayer.SetActive(true);
        }
    }

}
