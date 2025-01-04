using Model;
using UnityEngine;
using UnityEngine.UI;

namespace Application
{
    namespace Match
    {
        public class Dice : MonoBehaviour
        {
            //In case we don't want to give each dice side the same chance
            [SerializeField] private DiceSideModel _diceModel;
            private Image _diceImage => GetComponentInChildren<Image>();
            public int RollDice()
            {
                int index = RandomNumberGenerator.RandomNumberGenerator.GenerateNumber(_diceModel._diceSideChance);
                _diceImage.sprite = _diceModel.Sprites[index];
                return index + 1;
            }
        }
    }

}