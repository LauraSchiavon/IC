using UnityEngine;

namespace JigsawPuzzlesCollection.Scripts
{
    [CreateAssetMenu(fileName = "New Collection", menuName = "JigsawPuzzlesCollection/Collection")]
    public class Collection : ScriptableObject
    {
        public Sprite Image;
        public bool Free;
        public Level[] Levels;

        public bool IsAvailable
        {
            get
            {
                return PlayerPrefs.GetInt("Collection:" + name + ":Available") == 1;
            }
            set
            {
                PlayerPrefs.SetInt("Collection:" + name + ":Available", value ? 1 : 0);
            }
        }
    }
}