using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character
{
    public class EnemyHolder : MonoBehaviour
    {
        public List<GameObject> Enemies { get; private set; }
        // Start is called before the first frame update
        void Start()
        {
            FindEnemies();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void FindEnemies()
        {
            Enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();

        }
    }
}
