using Character;
using UnityEngine;

namespace Player
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

        public CharacterDataMono GetPlayerData()
        {
            if (Player == null)
            {
                FindPlayer();
            }
            return Player.GetComponent<CharacterDataMono>();
        }
    }
}
