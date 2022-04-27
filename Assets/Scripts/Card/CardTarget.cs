using System.Collections.Generic;
using Board;
using Character;
using Enums;
using Helper;
using ScriptableObjects;
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
            drawSquares.ResetHighlights(HighlightType.PlayerAttack, highlightedSquares);
        }

        public void HighlightPlayerSquare()
        {
            if (isHighlightingSquares)
            {
                return;
            }

            highlightedSquares = new() {dataMono.Position};
            drawSquares.DrawHighlights(highlightedSquares, HighlightType.PlayerAttack);
            
            isHighlightingSquares = true;
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
            // If card is not a simple card(one with a target pattern, return)
            if (card is not SimpleCardScriptableObject targetCard) return;
            
            highlightedSquares = GridHighlightHelper.CalculateHighlightedSquares(dataMono.Position, targetCard.range, targetCard.targetingPattern);
            drawSquares.DrawHighlights(highlightedSquares, HighlightType.PlayerAttack);
            
            isHighlightingSquares = true;
        }

        public bool ContainsPos(Vector3Int pos)
        {
            return highlightedSquares.Contains(pos);
        }
    }
}
