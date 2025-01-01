using UnityEngine;

namespace Model
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Dice Side Model", menuName = "Dice/Dice Side Model")]
    public class DiceSideModel : ScriptableObject
    {
        public Sprite[] Sprites;
        [Tooltip("Number will determine what will be the chance of this side of dice appearing")]
        public int[] _diceSideChance;
    }
}
