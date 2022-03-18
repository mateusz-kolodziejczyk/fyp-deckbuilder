using System.Collections.Generic;
using Character;
using Enums;
using Helper;
using UnityEngine;

namespace Card
{
    public class CardTarget : MonoBehaviour
    {
        private CardPlaying cardPlaying;

        private DrawSquares drawSquares;

        private CharacterData data;
        
        private bool isHighlightingSquares = false;

        private List<Vector3Int> highlightedSquares = new();

        // Start is called before the first frame update
        void Start()
        {
            data = GetComponent<CharacterData>();
            cardPlaying = GetComponent<CardPlaying>();
            drawSquares = GameObject.FindWithTag("GridDrawerController").GetComponent<DrawSquares>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void ClearTargetSquares()
        {
            if (!isHighlightingSquares)
            {
                return;
            }
            isHighlightingSquares = false;
            drawSquares.ResetHighlights(highlightedSquares, EntityType.Player);
        }
        public void HighlightTargetSquares()
        {
            if (isHighlightingSquares)
            {
                return;
            }
            var card = cardPlaying.GetCurrentCard();
            // If no card selected, null
            if (card == null)
            {
                return;
            }

            highlightedSquares = GridHighlightHelper.CalculateHightlightedSquares(data.Position, card.range);
            drawSquares.DrawHighlights(highlightedSquares, EntityType.Player);
            
            isHighlightingSquares = true;
        }
    }
}
