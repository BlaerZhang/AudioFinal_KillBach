using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int value;
    public TextMeshPro valueText;
    public float moveSpeed;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        valueText.text = value + " pt";
    }

    public int GetKilled()
    {
        gameObject.SetActive(false);
        return value;
    }
}
