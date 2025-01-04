using Model;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Application
{
    namespace Match
    {
        public class MatchUI : MonoBehaviour
        {
            [Header("Player Side")]
            [SerializeField] private TextMeshProUGUI _playerUsername;
            [SerializeField] private Slider _playerScoreSlider;
            [SerializeField] private TextMeshProUGUI _playerScoreText;
            [Header("Opponent Side")]
            [SerializeField] private TextMeshProUGUI _opponentUsername;
            [SerializeField] private Slider _opponentScoreSlider;
            [SerializeField] private TextMeshProUGUI _opponentScoreText;
            [Header("Common")]
            [SerializeField] private Button _playButton;
            [SerializeField] private TextMeshProUGUI _playPrompt;
            [SerializeField] private TextMeshProUGUI _gameResultPrompt;

            
            public void UpdateUI(UserMatchVariables playerVariables, UserMatchVariables opponentVariables, bool playersTurn)
            {
                _playerScoreSlider.value = playerVariables.CurrentScore;
                _playerScoreText.text = playerVariables.CurrentScore.ToString();
                _opponentScoreSlider.value = opponentVariables.CurrentScore;
                _opponentScoreText.text = opponentVariables.CurrentScore.ToString();
                if (CheckGameWin(playerVariables, opponentVariables))
                {
                    _playButton.gameObject.SetActive(false);
                    _playPrompt.text = "";
                    return;
                }
                _playButton.interactable = playersTurn;
                _playPrompt.text = playersTurn ? "Your Turn" : "Opponents Turn";
            }

            public void InitializeUI(string playerUsername, string opponentUsername, UnityAction buttonClickAction, bool playersTurn)
            {
                _playerUsername.text = playerUsername;
                _opponentUsername.text = opponentUsername;
                _playerScoreSlider.value = 0;
                _opponentScoreSlider.value = 0;
                _playerScoreText.text = "0";
                _opponentScoreText.text = "0";
                _playButton.onClick.AddListener(buttonClickAction);
                _playButton.interactable = playersTurn;
                _playPrompt.text = playersTurn ? "Your Turn" : "Opponents Turn";
                _gameResultPrompt.text = "";
            }

            public bool CheckGameWin(UserMatchVariables playerVariables, UserMatchVariables opponentVariables)
            {
                if (playerVariables.CurrentScore >= 30)
                {
                    _gameResultPrompt.text = "You won the game +50xp ";
                    return true;
                }
                if(opponentVariables.CurrentScore>= 30)
                {
                    _gameResultPrompt.text = "You lost the game";
                    return true;
                }
                return false;
            }
        }
    }
}
