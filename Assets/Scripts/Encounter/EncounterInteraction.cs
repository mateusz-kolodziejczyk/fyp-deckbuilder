using Animation;
using ProceduralGeneration;
using SceneManagement;
using ScriptableObjects;
using Statics;
using UnityEngine;

namespace Encounter
{
    [RequireComponent(typeof(AnimationScaler))]
    public class EncounterInteraction : MonoBehaviour
    {
        private AnimationScaler animationScaler;

        private Coroutine animationCoroutine;

        public bool Active { get; set; } = true;
        private bool isFaded = false;
    
        private void Start()
        {
            animationScaler = GetComponent<AnimationScaler>();
        }

        private void Update()
        {
            if (!isFaded && !Active)
            {
                FadeOut();
            }
        }

        private void FadeOut()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();

            var color = spriteRenderer.color;
            color = new Color(color.r, color.g, color.b, 0.5f);
            spriteRenderer.color = color;
        }
        public void OnMouseDown()
        {
            if (!Active)
            {
                return;
            }
            if (!TryGetComponent(out EncounterData data)) return;
            // Check to see if the square is connected to player
            // Get the map manager to update the map data store
            var mapManager = GameObject.FindWithTag("MapManager").GetComponent<MapGeneration>();
            mapManager.UpdateDataStore();
        
            if (!mapManager.IsConnected(CampaignMapDataStore.CurrentSquare, data.Position))
            {
                return;
            }
        
            CampaignMapDataStore.CurrentSquare = GetComponent<EncounterData>().Position;
        


            switch (data.EncounterScriptableObject)
            {
                case BattleScriptableObject:
                    SceneMovement.LoadCombat();
                    break;
                case ShopScriptableObject:
                    SceneMovement.LoadShop();
                    break;
            }
        }

        public void OnMouseEnter()
        {
        
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);

            }
            if (!Active)
            {
                return;
            }
            animationCoroutine = StartCoroutine(animationScaler.IncreaseBoxSize());
        }

        public void OnMouseExit()
        { 
        
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);

            }
            if (!Active)
            {
                return;
            }
            StartCoroutine(animationScaler.ResetBoxSize());
        }
    }
}
