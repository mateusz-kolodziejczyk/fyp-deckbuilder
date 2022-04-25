using UnityEngine;
using UnityEngine.UI;

public class ScrollBarListener : MonoBehaviour
{
    private Scrollbar scrollbar;

    [SerializeField] private Vector3 minPosition, maxPosition;
    // Start is called before the first frame update
    void Start()
    {
        var position = transform.position;
        minPosition.z = position.z;
        maxPosition.z = position.z;
        scrollbar = GameObject.FindWithTag("MapScroll").GetComponent<Scrollbar>();
        if (scrollbar != null)
        {
            scrollbar.onValueChanged.AddListener(onScroll);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void onScroll(float value)
    {
        transform.position = Vector3.Lerp(minPosition, maxPosition, value);
    }
}
