using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button _soundEffectsButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _soundEffectsText;
    [SerializeField] private TextMeshProUGUI _musicText;

    [SerializeField] private Button _moveUpButton;
    [SerializeField] private Button _moveDownButton;
    [SerializeField] private Button _moveLeftButton;
    [SerializeField] private Button _moveRightButton;
    [SerializeField] private Button _interactButton;
    [SerializeField] private Button _interactAltButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private TextMeshProUGUI _moveUpText;
    [SerializeField] private TextMeshProUGUI _moveDownText;
    [SerializeField] private TextMeshProUGUI _moveLeftText;
    [SerializeField] private TextMeshProUGUI _moveRightText;
    [SerializeField] private TextMeshProUGUI _interactText;
    [SerializeField] private TextMeshProUGUI _interactAltText;
    [SerializeField] private TextMeshProUGUI _pauseText;
    [SerializeField] private Transform _pressToRebindKeyTransform;

    private void Awake()
    {
        _soundEffectsButton.onClick.AddListener(OnSoundEffectsButtonClicked);
        _musicButton.onClick.AddListener(OnMusicButtonClicked);
        _closeButton.onClick.AddListener(OnCloseButtonClicked);

        _moveUpButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Up));
        _moveDownButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Down));
        _moveLeftButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Left));
        _moveRightButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Right));
        _interactButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Interact));
        _interactAltButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.InteractAlternate));
        _pauseButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Pause));
    }

    private void OnSoundEffectsButtonClicked()
    {
        SoundManager.Instance.ChangeVolume();
    }

    private void OnMusicButtonClicked()
    {
        MusicManager.Instance.ChangeVolume();
    }

    private void OnCloseButtonClicked()
    {
        Hide();
    }

    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

        UpdateVisual();

        HidePressToRebindKey();
        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        _soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        _musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        _moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        _moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        _moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        _moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        _interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        _interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        _pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        _pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        _pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
