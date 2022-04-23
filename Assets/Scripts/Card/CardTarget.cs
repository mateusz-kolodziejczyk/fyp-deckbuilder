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

        private CharacterDataMono dataMono;
        
        private bool isHighlightingSquares = false;

        private List<Vector3Int> highlightedSquares = new();

        // Start is called before the first frame update
        void Start()
        {
            dataMono = GetComponent<CharacterDataMono>();
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
            drawSquares.ResetHighlights(highlightedSquares, HighlightType.PlayerAttack);
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
            highlightedSquares = GridHighlightHelper.CalculateHightlightedSquares(dataMono.Position, card.range);
            drawSquares.DrawHighlights(highlightedSquares, HighlightType.PlayerAttack);
            
            isHighlightingSquares = true;
        }

        public bool ContainsPos(Vector3Int pos)
        {
            return highlightedSquares.Contains(pos);
        }
    }
}
