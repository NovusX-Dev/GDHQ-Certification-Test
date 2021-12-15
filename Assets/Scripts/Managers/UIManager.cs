using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Player Stats")]
    [SerializeField] Image _healthFill;
    [SerializeField] TextMeshProUGUI _comboText;
    [SerializeField] TextMeshProUGUI _killscoreText;
    [SerializeField] GameObject[] _rankIcons;

    [Header("Panels")]
    [SerializeField] GameObject _playerDeathPanel;
    [SerializeField] GameObject _escapeOptionsPanel;
    [SerializeField] GameObject _levelEndPanel;
    [SerializeField] GameObject _dialogPanel;

    [Header("Player Death Panel")]
    [SerializeField] TextMeshProUGUI _deathTotalScoreText, _respawnLivesText;
    [SerializeField] GameObject _respawnButton, _deathRestartLevelButton;

    [Header("Next Level Panel")]
    [SerializeField] TextMeshProUGUI _highestComboText;
    [SerializeField] TextMeshProUGUI _levelKillScoreText, _totalScoreText;

    [Header("Dialog Panel")]
    [SerializeField] TextMeshProUGUI _dialogText;


    [Header("SFX")]
    [SerializeField] AudioClip[] _buttonSFXs;

    [Header("References")]
    [SerializeField] LevelCheckpointManager _levelCheckpointManager;

    private float _playerMaxHealth;
    private string _nextLevelToLoad;
    private LevelCheckpointManager[] _levelCheckpoints = null;
    private bool _canRespawn;

    Light _mainLight;
    Fader _fader;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDamagedInt += UpdateHealthUI;
        PlayerHealth.OnPlayerDeathRespawnCheck += ActivateDeathPanel;
        PlayerLevelManager.OnPlayerLevelUp += UpdateRankIcons;
        LevelEndTrigger.OnLevelEnded += ActivateLevelEndPanel;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDamagedInt -= UpdateHealthUI;
        PlayerHealth.OnPlayerDeathRespawnCheck -= ActivateDeathPanel;
        PlayerLevelManager.OnPlayerLevelUp -= UpdateRankIcons;
        LevelEndTrigger.OnLevelEnded -= ActivateLevelEndPanel;
    }

    protected override void Awake()
    {
        base.Awake();
        _fader = GetComponentInChildren<Fader>();
    }

    void Start()
    {
        _comboText.gameObject.SetActive(false);
        var player = FindObjectOfType<PlayerHealth>();
        if (player != null) _playerMaxHealth = player.GetDifficultyMaxHealth();

        UpdateRankIcons(1);

        _mainLight = GameObject.FindGameObjectWithTag("Sun").GetComponentInChildren<Light>();
        _mainLight.intensity = LoadingData._gameBrightness;
        _levelCheckpoints = FindObjectsOfType<LevelCheckpointManager>();

        StartCoroutine(_fader.FadeIn(1f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivateOptionsPanel();
        }
    }

    public void DialogBoxActivation(string dialog)
    {
        _dialogText.text = dialog;
    }

    public void UIFadeIN(float time)
    {
        StartCoroutine(_fader.FadeIn(time));
    }

    #region Update Stats

    private void UpdateHealthUI(float amount)
    {
        _healthFill.fillAmount = amount / _playerMaxHealth;
        if (_healthFill.fillAmount < 0.161f)
        {
            _healthFill.color = Color.red;
        }
        else
        {
            _healthFill.color = Color.white;
        }
    }

    public void UpdateComboUI(int combo, bool active)
    {
        _comboText.text = $"{combo} X";
        _comboText.gameObject.SetActive(active);
    }

    public void UpdateKillScoreUI(int score)
    {
        _killscoreText.text = $"{score}";
    }

    private void UpdateRankIcons(int rank)
    {
        for (int i = 0; i < rank; i++)
        {
            _rankIcons[i].SetActive(true);
        }
        for (int i = rank + 1; i < _rankIcons.Length; i++)
        {
            _rankIcons[i].SetActive(false);
        }
    }

    #endregion

    #region Escape Options Panel

    public void ActivateOptionsPanel()
    {
        Time.timeScale = Time.timeScale > 0 ? 0 : 1f;
        _escapeOptionsPanel.SetActive(!_escapeOptionsPanel.activeInHierarchy);
    }

    public void SetMusicVol(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    public void SetSFXVol(float volume)
    {
        AudioManager.Instance.SetSEFXVolume(volume);
    }

    public void SetBrightness(float bright)
    {
        _mainLight.intensity = bright;
        LoadingData._gameBrightness = bright;
    }

    #endregion

    #region Death Panel

    private void CanPlayerRespawn()
    {
        foreach (var point in _levelCheckpoints)
        {
            if(!point.CanRespawn)
            {
                _canRespawn = false;
            }
            else
            {
                _canRespawn = true;
                break;
            }
        }
    }

    public void ActivateDeathPanel(int respawnLives)
    {
        CanPlayerRespawn();
        _playerDeathPanel.SetActive(true);
        _deathTotalScoreText.text = $"Total Score: {ScoreManager.Instance.GetTotalScore()}";
        _respawnLivesText.text = $"Respawn Lives Remaining: {respawnLives}";
        if (respawnLives > 0 && _canRespawn)
        {
            _respawnButton.SetActive(true);
        }
        else
        {
            _deathRestartLevelButton.SetActive(true);
        }

    }

    public void DeactivateDeathPanel()
    {
        _respawnButton.SetActive(false);
        _deathRestartLevelButton.SetActive(false);
        _playerDeathPanel.SetActive(false);
    }

    #endregion

    #region Level End Panel

    public void ActivateLevelEndPanel(string level)
    {
        _nextLevelToLoad = level;
        _levelEndPanel.SetActive(true);
        _highestComboText.text = $"{ScoreManager.Instance.GetHighestCombo()} X";
        _levelKillScoreText.text = $"{ScoreManager.Instance.GetKillScore()}";
        _totalScoreText.text = $"{ScoreManager.Instance.GetTotalScore()}";
    }

    #endregion

    #region Buttons

    public void RespawnButton()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        AudioManager.Instance.PlayMultiSFX(_buttonSFXs);
        yield  return _fader.FadeOut(1f);
        GameManager.Instance.RespawnPlayer();
    }

    public void RestartLevelButton()
    {
        StartCoroutine(RestartRoutine());
    }

    IEnumerator RestartRoutine()
    {
        AudioManager.Instance.PlayMultiSFX(_buttonSFXs);
        Time.timeScale = 1f;
        yield return _fader.FadeOut(1f);
        LevelLoadingManager.Instance.RestartLevel();
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelRoutine());
    }

    IEnumerator LoadNextLevelRoutine()
    {
        AudioManager.Instance.PlayMultiSFX(_buttonSFXs);
        Time.timeScale = 1f;
        yield return _fader.FadeOut(1f);
        LevelLoadingManager.Instance.LoadNextLevel(_nextLevelToLoad);
    }

    public void MainMenuButton(int index)
    {
        StartCoroutine(MainMenuRoutine());
    }

    IEnumerator MainMenuRoutine()
    {
        AudioManager.Instance.PlayMultiSFX(_buttonSFXs);
        Time.timeScale = 1f;
        yield return _fader.FadeOut(1f);
        LevelLoadingManager.Instance.LoadNextLevel("Main Menu");
    }

    public void ResumeGameButton()
    {
        AudioManager.Instance.PlayMultiSFX(_buttonSFXs);
        _escapeOptionsPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitButton()
    {
        AudioManager.Instance.PlayMultiSFX(_buttonSFXs);
        Application.Quit();
    }

    #endregion
}
