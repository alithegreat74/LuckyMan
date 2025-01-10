package LuckyManExtensions.GameZone.Game.DiceRolling;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.entities.variables.UserVariable;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import java.util.List;
import java.util.Random;
public class DiceRollHandler extends BaseClientRequestHandler {
    @Override
    public void handleClientRequest(User user, ISFSObject isfsObject) {
        Random rand = new Random();
        //Generate a random number
        int randomInt = rand.nextInt(5);
        ISFSObject response = new SFSObject();
        try {
            getApi().setUserVariables(user,UpdateUserVariables(user,randomInt));
            response.putBool("success", true);
            response.putInt("diceNumber",randomInt);
        } catch (Exception e) {
            trace("Unable to update user variables");
            response.putBool("success", false);
        }
        send("diceRoll",response, user);
    }
    private List<UserVariable> UpdateUserVariables(User user, int diceNumber)
    {
        int currentScore = 0;
        try{
            currentScore=user.getVariable("currentScore").getIntValue();
        } catch (Exception e) {
            trace("Creating initial variables");
        }
        //Dice numbers are from 1 to 6 but our index is from 0 to 5
        currentScore+=diceNumber+1;
        SFSUserVariable diceVar = new SFSUserVariable("diceNumber", diceNumber);
        SFSUserVariable scoreVar = new SFSUserVariable("currentScore", currentScore);
        return List.of(diceVar, scoreVar);
    }
}
