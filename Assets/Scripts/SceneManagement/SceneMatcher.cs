using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace SceneManagement
{
    public static class SceneMatcher
    {
        public static Dictionary<SceneType, int> SceneTypeToIndex { get; } = new()
        {
            {SceneType.MainMenu, 0},
            {SceneType.Battle, 1},
            {SceneType.CampaignMap, 2},
            {SceneType.Shop, 3},
            {SceneType.Win, 4},
            {SceneType.GameOver, 5},
        };
        public static Dictionary<int, SceneType> IndexToSceneType { get; } = new()
        {
            {0, SceneType.MainMenu},
            {1, SceneType.Battle},
            {2, SceneType.CampaignMap},
            {3, SceneType.Shop},
            {4, SceneType.Win},
            {5, SceneType.GameOver},
        };

        
    }
}
