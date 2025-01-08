package LuckyManExtensions.GameZone.DiceRolling;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import java.util.List;
public class DiceRollHandler extends BaseClientRequestHandler {
    @Override
    public void handleClientRequest(User user, ISFSObject isfsObject) {
        //Get the number of the dice and send it to all of the users in the room
        List<User> roomUsers = getParentExtension().getParentRoom().getUserList();
        send("diceRoll",isfsObject,roomUsers);
    }
}
