using Animation;
using Pause;
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

        private PauseManagement pauseManager;
        private bool pauseMenuActive = false;
        private void Start()
        {
            animationScaler = GetComponent<AnimationScaler>();
            pauseManager = GameObject.FindWithTag("PauseManager").GetComponent<PauseManagement>();
        }

        private void Update()
        {
            if (!isFaded && !Active)
            {
                FadeOut();
            }
            // If pause menu was active the frame before, and inactive now, start a coroutine to return to original size
            if (pauseMenuActive && !pauseManager.IsActive)
            {
                animationCoroutine = StartCoroutine(animationScaler.ResetBoxSize());
            }
            pauseMenuActive = pauseManager.IsActive;
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
            // If pause menu is active, return
            if (pauseManager.IsActive)
            {
                return;
            }
            
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

        public void OnMouseOver()
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }
            // If pause menu is active, return
            if (pauseManager.IsActive)
            {
                return;
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
            // If pause menu is active, return
            if (pauseManager.IsActive)
            {
                return;
            }
            if (!Active)
            {
                return;
            }
            StartCoroutine(animationScaler.ResetBoxSize());
        }
    }
}
