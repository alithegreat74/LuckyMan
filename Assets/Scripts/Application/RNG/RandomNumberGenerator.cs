using System.Linq;
using UnityEngine;

namespace Application
{
    namespace RandomNumberGenerator
    {
        public class RandomNumberGenerator
        {
            public static int GenerateNumber(int min, int max)
            {
                return Random.Range(min, max);
            }

            [Tooltip("This function will get a series of chance values and chooses an index based on the chances given")]
            public static int GenerateNumber(int[] weightedChances)
            {
                int sumOfWeights = weightedChances.Sum();
                int random = Random.Range(0, sumOfWeights);

                int cumulative = 0;
                for (int i = 0; i < weightedChances.Length; i++)
                {
                    cumulative += weightedChances[i];
                    if (random < cumulative)
                        return i;
                }

                throw new System.Exception("Failed to generate a valid random number");
            }
        }

    }
}