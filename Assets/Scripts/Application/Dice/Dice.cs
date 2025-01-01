using Model;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Application
{
    public class Dice : MonoBehaviour
    {
        [SerializeField] private DiceSideModel _diceModel;
        //In case we don't want to give each dice side the same chance
        private Image _diceImage => GetComponentInChildren<Image>();
        
        private void Start()
        {
            RollDice();
        }
        public void RollDice()
        {
            int index = RandomNumberGenerator.RandomNumberGenerator.GenerateNumber(_diceModel._diceSideChance);
            Debug.Log(index);
            _diceImage.sprite = _diceModel.Sprites[index];
        }
    }

}