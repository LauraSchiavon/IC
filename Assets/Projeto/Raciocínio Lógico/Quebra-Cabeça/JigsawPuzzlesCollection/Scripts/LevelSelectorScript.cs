using UnityEngine;

namespace JigsawPuzzlesCollection.Scripts
{
    public class LevelSelectorScript : MonoBehaviour
    {
        public Collection SelectedCollection { get; set; }
        public Level SelectedLevel { get; set; }

        public static LevelSelectorScript Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
    }
}