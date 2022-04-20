using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScaler : MonoBehaviour
{
    [SerializeField] [Range(1f, 2f)] private float maxScale;
    [SerializeField] [Range(0.01f, 0.1f)] private float progressPerFrame;
    [SerializeField] [Range(0.5f, 5f)] private float returnFactor;
    private Vector3 initialScale;
    private Vector3 finalScale;
    private Vector3 increasePerFrame;
    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;
        finalScale = initialScale * maxScale;
        
        // Increase per frame is the actual increase in explicit scale per frame
        increasePerFrame = (finalScale - initialScale) * progressPerFrame;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator IncreaseBoxSize()
    {
        while (transform.localScale.magnitude < finalScale.magnitude)
        {
            transform.localScale += increasePerFrame;
            yield return null;
        }
        
    }

    public IEnumerator ResetBoxSize()
    {
        while (transform.localScale.magnitude > initialScale.magnitude)
        {
            transform.localScale -= increasePerFrame * returnFactor;
            yield return null;
        }
    }
}
