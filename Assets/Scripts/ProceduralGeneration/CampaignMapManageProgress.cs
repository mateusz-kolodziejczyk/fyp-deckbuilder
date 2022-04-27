using Statics;
using TMPro;
using UnityEngine;


// Manage map progress by greying out unusable squares, and moving the player
namespace ProceduralGeneration
{
    public class CampaignMapManageProgress : MonoBehaviour
    {
        private GameObject player;

        private MapGeneration mapGeneration;

        [SerializeField] private TextMeshProUGUI healthText;

        [SerializeField] private TextMeshProUGUI currencyText;
        // Start is called before the first frame update
        private void Start()
        {
        
            player = GameObject.FindWithTag("Player");
            mapGeneration = GetComponent<MapGeneration>();
            if (CampaignMapDataStore.CurrentSquare == new Vector2Int(0, -1))
            {
                mapGeneration.GenerateCampaignMap();
            }
            else
            {
                mapGeneration.LoadMap(CampaignMapDataStore.Connections, CampaignMapDataStore.EncounterScriptableObjects, CampaignMapDataStore.BossEncounterScriptableObject);
            }

            PositionPlayer();
            mapGeneration.SetActiveSquares(CampaignMapDataStore.CurrentSquare);
            // Update player hp text
            healthText.text = $"{PlayerDataStore.CharacterData.HitPoints}/{PlayerDataStore.CharacterData.MAXHitPoints}";
            // Update player currency text
            currencyText.text = $"{PlayerDataStore.CharacterData.Currency}";
        }

        private void PositionPlayer()
        {
            // Set player transform parent to the map
            player.transform.SetParent(mapGeneration.StartEncounter.o.transform.parent, false);
            // Try to position the player
            var newWorldPos = Vector3.zero;
            if (mapGeneration.Encounters.TryGetValue(CampaignMapDataStore.CurrentSquare, out var encounter))
            {
                newWorldPos = encounter.transform.position;
            }
        
            else if (CampaignMapDataStore.CurrentSquare == mapGeneration.StartEncounter.pos)
            {
                newWorldPos = mapGeneration.StartEncounter.o.transform.position;
            }

            player.transform.position = newWorldPos;
        }
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
