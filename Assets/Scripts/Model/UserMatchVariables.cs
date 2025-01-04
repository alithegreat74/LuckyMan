using Sfs2X.Entities.Variables;
using System.Collections.Generic;

namespace Model
{
    public struct UserMatchVariables
    {
        public int CurrentScore;
        public int DiceNumber;

        public UserMatchVariables(int currentScore, int lastDiceNumber)
        {
            this.CurrentScore = currentScore;
            this.DiceNumber = currentScore;
        }
        public void FromSFSUserVariable(List<UserVariable> userVariables)
        {
            this.CurrentScore = 0;
            this.DiceNumber = 0;
            foreach (var variable in userVariables)
            {
                switch (variable.Name)
                {
                    case "currentScore":
                        CurrentScore = variable.GetIntValue();
                        break;
                    case "diceNumber":
                        DiceNumber = variable.GetIntValue();
                        break;
                }
            }
        }
        public List<UserVariable> ToSFSUserVariable()
        {
            var userVariables = new List<UserVariable>();
            userVariables.Add(new SFSUserVariable("currentScore", CurrentScore));
            userVariables.Add(new SFSUserVariable("diceNumber", DiceNumber));
            return userVariables;
        }
    }
}