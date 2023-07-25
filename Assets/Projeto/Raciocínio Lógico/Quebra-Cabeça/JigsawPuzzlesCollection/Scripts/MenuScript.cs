using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

namespace JigsawPuzzlesCollection.Scripts
{
    public class MenuScript : Singleton<MenuScript>
    {
        public Image board;

        [Header("Collections")]
        public Collection[] Collections;
        public GameObject collectionsPanel;
        public GameObject collectionPrefab;

        [Header("Levels")]
        public GameObject levelsPanel;
        public GameObject levelPrefab;

        [Header("Settings")]
        public Button settingsButton;
        public GameObject settingsPanel;

        [Header("Settings/GamePlay")]
        public Toggle backgroundToggle;
        public Toggle[] boardsToggles;

        [Header("Settings/Sound")]
        public Toggle musicToggle;
        public Toggle soundsToggle;
        public Slider volumeSlider;

        public void Awake()
        {
            var gridParent = collectionsPanel.GetComponentInChildren<GridLayoutGroup>();
            foreach (var collection in Collections)
            {
                var collectionButton = Instantiate(collectionPrefab, gridParent.transform);
                collectionButton.name = collection.name;

                var collectionScript = collectionButton.GetComponent<CollectionScript>();
                collectionScript.SetDetails(collection);
            }

            var showBackground = GameplayManager.Instance.ShowBackground();
            backgroundToggle.isOn = showBackground;

            var musicMute = SoundManager.Instance.IsMusicMuted();
            var soundsMute = SoundManager.Instance.IsMuted();
            var volume = SoundManager.Instance.GetVolume();
            musicToggle.isOn = !musicMute;
            soundsToggle.isOn = !soundsMute;
            volumeSlider.value = volume;

            var selectedBoardIndex = GameplayManager.Instance.SelectedBoard();
            SelectBoard(selectedBoardIndex);
        }

        public void ToggleSettings()
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
            settingsButton.interactable = !settingsButton.interactable;
        }

        public void DisplayCollection(Collection collection)
        {
            var gridParent = levelsPanel.GetComponentInChildren<GridLayoutGroup>();
            var gridParentTransform = gridParent.transform;

            while (gridParentTransform.childCount > 0)
            {
                DestroyImmediate(gridParentTransform.GetChild(0).gameObject);
            }

            foreach (var level in collection.Levels)
            {
                var levelButton = Instantiate(levelPrefab, gridParentTransform);
                levelButton.GetComponent<LevelScript>().SetDetails(level);
            }

            collectionsPanel.SetActive(false);
            levelsPanel.SetActive(true);
        }

        public void HideCollection()
        {
            collectionsPanel.SetActive(true);
            levelsPanel.SetActive(false);
        }

        public void RefreshSelectedCollection()
        {
            var collection = LevelSelectorScript.Instance.SelectedCollection;

            var collectionButton = FindObjectsOfType<CollectionScript>().First(x => x.name == collection.name);
            var collectionScript = collectionButton.GetComponent<CollectionScript>();
            collectionScript.SetDetails(collection);

            DisplayCollection(collection);
        }

        public void RewardAdReady()
        {
            var collections = collectionsPanel.GetComponentsInChildren<CollectionScript>();
            foreach (var collection in collections)
            {
                collection.GetComponent<Button>().interactable = true;
            }
        }

        public void SelectBoard(int index)
        {
            GameplayManager.Instance.SetSelectedBoard(index);
            board.sprite = GameplayManager.Instance.Boards[index];

            for (int i = 0; i < boardsToggles.Length; i++)
            {
                boardsToggles[i].SetIsOnWithoutNotify(i == index);
            }
        }
    }
}