using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("==== PLAYER INPUT ====")]
    [SerializeField] PlayerInput playerInput;
    [Header("==== CANVAS ====")]
    [SerializeField] GameObject mainMenuCanvas;

    [SerializeField] GameObject optionMenuCanvas;

    [Header("==== BUTTON ====")]
    [SerializeField] Button buttonStart;
    [SerializeField] Button buttonOptions;
    [SerializeField] Button buttonQuit;

    [Header("==== OPTION MENU ====")]
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider soundVolume;
    AudioSource musicSource;
    AudioSource soundSource;

    private void Awake()
    {
        Transform audioManager = GameObject.FindObjectOfType<AudioManager>().transform;
        soundSource = audioManager.GetChild(0).GetComponent<AudioSource>();
        musicSource = audioManager.GetChild(1).GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        playerInput.onMainMenuOptions += MainMenuOptions;
        mainMenuCanvas.SetActive(true);
        optionMenuCanvas.SetActive(false);
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonStart.gameObject.name, OnButtonStartClicked);
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonOptions.gameObject.name, OnButtonOptionsClicked);
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonQuit.gameObject.name, OnButtonQuitClicked);
        musicVolume.onValueChanged.AddListener(OnMusicVolumeSliderChanged);
        soundVolume.onValueChanged.AddListener(OnSoundVolumeSliderChanged);
    }

    private void OnDisable()
    {
        playerInput.onMainMenuOptions -= MainMenuOptions;
        ButtonPressedBehaviour.buttonFunctionTable.Clear();
        musicVolume.onValueChanged.RemoveAllListeners();
        soundVolume.onValueChanged.RemoveAllListeners();
    }


    private void MainMenuOptions()
    {
        if (optionMenuCanvas.activeSelf)
        {
            optionMenuCanvas.SetActive(false);
            mainMenuCanvas.SetActive(true);
            buttonStart.Select();
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStart);
    }

    void OnButtonStartClicked()
    {
        mainMenuCanvas.SetActive(false);
        SceneLoader.Instance.LoadGameplayScene();
    }

    void OnButtonOptionsClicked()
    {
        UIInput.Instance.SelectUI(buttonOptions);
        playerInput.EnableMainMenuInput();
        mainMenuCanvas.SetActive(false);
        optionMenuCanvas.SetActive(true);
        musicVolume.value = musicSource.volume;
        soundVolume.value = soundSource.volume;
        UIInput.Instance.SelectUI(musicVolume);
    }

    void OnButtonQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnMusicVolumeSliderChanged(float value)
    {
        musicSource.volume = value;
    }

    void OnSoundVolumeSliderChanged(float value)
    {
        soundSource.volume = value;
    }
}
