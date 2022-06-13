using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [Header("==== PLAYER INPUT ====")]
    [SerializeField] PlayerInput playerInput;

    [Header("==== AUDIO DATA ====")]
    [SerializeField] AudioData pauseSFX;
    [SerializeField] AudioData unpauseSFX;

    [Header("==== CANVAS ====")]
    [SerializeField] Canvas hUDCanvas;
    [SerializeField] Canvas menusCanvas;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionMenu;

    [Header("==== PAUSE MENU ====")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button mainMenuButton;

    [Header("==== OPTION MENU ====")]
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider soundVolume;
    AudioSource musicSource;
    AudioSource soundSource;
    int buttonPressedParameterID = Animator.StringToHash("Pressed");

    private void Awake()
    {
        Transform audioManager = GameObject.FindObjectOfType<AudioManager>().transform;
        soundSource = audioManager.GetChild(0).GetComponent<AudioSource>();
        musicSource = audioManager.GetChild(1).GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;
        pauseMenu.SetActive(true);
        optionMenu.SetActive(false);
        ButtonPressedBehaviour.buttonFunctionTable.Add(resumeButton.gameObject.name, OnResumeButtonClick);
        ButtonPressedBehaviour.buttonFunctionTable.Add(optionsButton.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehaviour.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClick);
        musicVolume.onValueChanged.AddListener(OnMusicVolumeSliderChanged);
        soundVolume.onValueChanged.AddListener(OnSoundVolumeSliderChanged);
    }

    private void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;
        ButtonPressedBehaviour.buttonFunctionTable.Clear();
        musicVolume.onValueChanged.RemoveAllListeners();
        soundVolume.onValueChanged.RemoveAllListeners();
    }

    void Pause()
    {
        hUDCanvas.enabled = false;
        menusCanvas.enabled = true;
        GameManager.GameState = GameState.Paused;
        TimeController.Instance.Pause();

        playerInput.EnablePauseMenuInput();
        playerInput.SwitchToDynamicUpdateMode();

        UIInput.Instance.SelectUI(resumeButton);
        AudioManager.Instance.PlaySFX(pauseSFX);
    }

    void Unpause()
    {
        if (pauseMenu.activeSelf)
        {
            resumeButton.Select();
            resumeButton.animator.SetTrigger(buttonPressedParameterID);
            AudioManager.Instance.PlaySFX(unpauseSFX);
        }
        else if (optionMenu.activeSelf)
        {
            pauseMenu.SetActive(true);
            optionMenu.SetActive(false);
            resumeButton.Select();
        }
    }

    void OnResumeButtonClick()
    {
        if (pauseMenu.activeSelf)
        {
            hUDCanvas.enabled = true;
            menusCanvas.enabled = false;
            GameManager.GameState = GameState.Playing;
            TimeController.Instance.Unpause();
            playerInput.EnableGameplayInput();
            playerInput.SwitchToFixedUpdateMode();
        }
        else if (optionMenu.activeSelf)
        {
            pauseMenu.SetActive(true);
            optionMenu.SetActive(false);
            resumeButton.Select();
        }
    }

    void OnOptionsButtonClick()
    {
        UIInput.Instance.SelectUI(optionsButton);
        playerInput.EnablePauseMenuInput();
        pauseMenu.SetActive(false);
        optionMenu.SetActive(true);
        musicVolume.value = musicSource.volume;
        soundVolume.value = soundSource.volume;
        UIInput.Instance.SelectUI(musicVolume);
    }

    void OnMainMenuButtonClick()
    {
        menusCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
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
