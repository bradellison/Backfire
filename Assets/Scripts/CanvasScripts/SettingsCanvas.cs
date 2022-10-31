using ManagerScripts;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CanvasScripts
{
    public class SettingsCanvas : MonoBehaviour
    {
    
        public Button settingsMenuButton;

        [SerializeField]
        private MusicVolumeManagerScriptableObject musicVolumeManagerScriptableObject;
        public Slider musicVolumeSlider;
        public TMP_Text musicVolumeText;

        [SerializeField] 
        private SfxVolumeManagerScriptableObject sfxVolumeManagerScriptableObject;
        public Slider sfxVolumeSlider;
        public TMP_Text sfxVolumeText;

        [SerializeField]
        private BackgroundResolutionManagerScriptableObject backgroundResolutionManagerScriptableObject;
        public Slider backgroundResolutionSlider;
        public TMP_Text backgroundResolutionText;

        public Button unlockLevelsButton;

        public Button enableDebugLoggingButton;

        [SerializeField] private OnPreferencesResetScriptableObject onPreferencesResetScriptableObject;
        public Button resetPreferencesButton;
        
        private GameManager _gameManager;

        

        private void Start()
        {
            _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

            settingsMenuButton.onClick.AddListener(ToggleSettingsMenu);
    
            musicVolumeSlider.onValueChanged.AddListener(delegate {MusicVolumeSliderChanged(); });

            sfxVolumeSlider.onValueChanged.AddListener(delegate {SfxVolumeSliderChanged(); });

            backgroundResolutionSlider.onValueChanged.AddListener(delegate {BackgroundResolutionSliderChanged(); });

            unlockLevelsButton.onClick.AddListener(UnlockLevels);

            enableDebugLoggingButton.onClick.AddListener(ToggleDebug);
            
            resetPreferencesButton.onClick.AddListener(ResetPrefs);
            
            SetAllTextValues();
        }
        
        private void ToggleSettingsMenu() {
            _gameManager.ToggleSettingsMenu();
        }
        
        private void ToggleDebug()
        {
            _gameManager.canvasManager.ToggleDebugCanvas();
        }
        
        private void SetAllTextValues()
        {
            musicVolumeSlider.value = _gameManager.musicManager.volume;
            sfxVolumeSlider.value = _gameManager.sfxManager.volume;
            backgroundResolutionSlider.value = _gameManager.outerSpace.resolution;
            
            musicVolumeText.text = ($"{Mathf.RoundToInt(musicVolumeSlider.value).ToString()}");
            sfxVolumeText.text = ($"{Mathf.RoundToInt(sfxVolumeSlider.value).ToString()}");
            backgroundResolutionText.text = ($"{Mathf.RoundToInt(backgroundResolutionSlider.value).ToString()}");
        }
        private void MusicVolumeSliderChanged() {
            musicVolumeText.text = ($"{Mathf.RoundToInt(musicVolumeSlider.value).ToString()}");
            musicVolumeManagerScriptableObject.SetMusicVolume(musicVolumeSlider.value);
        }

        private void SfxVolumeSliderChanged() {
            sfxVolumeText.text = ($"{Mathf.RoundToInt(sfxVolumeSlider.value).ToString()}");
            sfxVolumeManagerScriptableObject.SetSfxVolume(sfxVolumeSlider.value);
        }

        private void BackgroundResolutionSliderChanged() {
            backgroundResolutionText.text = ($"{Mathf.RoundToInt(backgroundResolutionSlider.value).ToString()}");
            backgroundResolutionManagerScriptableObject.SetBackgroundResolution(Mathf.RoundToInt(backgroundResolutionSlider.value));
        }

        private void UnlockLevels() {
            _gameManager.levelManager.UnlockAllLevels();
        }
        
        private void ResetPrefs()
        {
            onPreferencesResetScriptableObject.OnPreferencesReset();
            Invoke(nameof(SetAllTextValues), 0.1f);
        }
    
    }
}
