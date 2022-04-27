using System.Collections.Generic;
using Board;
using Enemy;
using Enums;
using Helper;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Character
{
    [RequireComponent(typeof(EnemyAttack))]
    public class Intent : MonoBehaviour
    {
        private Tilemap tilemap;
        private EnemyDataMono dataMono;
        private List<Vector3Int> paintedTiles;
        private EnemyAttack enemyAttack;
        private DrawSquares drawSquares;

        [SerializeField] private Image intentIcon;
        [SerializeField] private TextMeshProUGUI intentMagnitude;
        [SerializeField] private List<SerializableKeyValuePair<AbilityType, Sprite>> abilityTypeIconsList;

        private readonly Dictionary<AbilityType, Sprite> abilityTypeIcons = new();
        private void Start()
        {
            tilemap = GameObject.FindWithTag("TileMap").GetComponent<Tilemap>();
            dataMono = GetComponent<EnemyDataMono>();
            paintedTiles = new();
            enemyAttack = GetComponent<EnemyAttack>();
        
            var gridDrawerController = GameObject.FindWithTag("GridDrawerController");
            if (gridDrawerController == null) return;
            
            if (gridDrawerController.TryGetComponent(out DrawSquares drawSquares))
            {
                this.drawSquares = drawSquares;
            }
            
            // Hide intent graphics until intent is set for the first time
            intentIcon.gameObject.SetActive(false);
            intentMagnitude.gameObject.SetActive(false);

            foreach (var abilityTypeIconPair in abilityTypeIconsList)
            {
                abilityTypeIcons[abilityTypeIconPair.key] = abilityTypeIconPair.value;
            }
        }

        public void DrawIntent()
        {
            var currentAbility = enemyAttack.CurrentAbility;
            HighlightTargetedSquares();
            UpdateIntentGraphic(currentAbility.abilityType, currentAbility.magnitude);
        }

        private void HighlightTargetedSquares()
        {
            var squaresToPaint = new List<Vector3Int>(enemyAttack.SquaresToAttack);
            if (drawSquares != null)
            {
                drawSquares.DrawHighlights(squaresToPaint, HighlightType.EnemyAttack);
            }
        }

        private void UpdateIntentGraphic(AbilityType type, int magnitude)
        {
            if (!intentIcon.gameObject.activeSelf || !intentMagnitude.gameObject.activeSelf)
            {
                intentIcon.gameObject.SetActive(true);
                intentMagnitude.gameObject.SetActive(true);
            }

            intentIcon.sprite = abilityTypeIcons[type];
            intentMagnitude.text = magnitude.ToString();
        }
    }
}
