using UnityEngine;

namespace Character
{
    public class PlayerHolder : MonoBehaviour
    {
        public GameObject Player { get; private set; }
        // Start is called before the first frame update
        void Start()
        {
            FindPlayer();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void FindPlayer()
        {
            Player = GameObject.FindWithTag("Player");
        }
    }
}
