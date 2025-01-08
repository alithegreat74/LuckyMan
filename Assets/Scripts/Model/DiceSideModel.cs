using UnityEngine;

namespace Model
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Dice Side Model", menuName = "Dice/Dice Side Model")]
    public class DiceSideModel : ScriptableObject
    {
        public Sprite[] Sprites;
    }
}
