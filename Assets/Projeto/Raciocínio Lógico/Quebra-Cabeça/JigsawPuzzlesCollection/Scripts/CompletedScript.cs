using UnityEngine;

namespace JigsawPuzzlesCollection.Scripts
{
    public class CompletedScript : MonoBehaviour
    {
        public int CollectionIndex;
        public int LevelIndex;

        public void Start()
        {
            var key = $"Collection{CollectionIndex}:Level{LevelIndex}:Completed";
            if (PlayerPrefs.GetInt(key) != 1)
            {
                gameObject.SetActive(false);
            }
        }
    }
}