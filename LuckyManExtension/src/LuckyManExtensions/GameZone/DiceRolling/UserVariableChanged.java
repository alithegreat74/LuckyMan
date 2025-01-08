package LuckyManExtensions.GameZone.DiceRolling;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

public class UserVariableChanged extends BaseServerEventHandler {
    @Override
    public void handleServerEvent(ISFSEvent isfsEvent) throws SFSException {
        User user = (User) isfsEvent.getParameter(SFSEventParam.USER);
        if(user.getVariable("currentScore").getIntValue()<30)
            return;
        trace(String.format("Player %s won the game", user.getName()));
        String query = "UPDATE users SET xp = xp + ? WHERE username = ?";

        try{
            IDBManager dbManager = getParentExtension().getParentZone().getDBManager();

            // Execute the update query
            dbManager.executeUpdate(query, new Object[]{50, user.getName()});

        }
        catch(Exception e){
            trace(e.getMessage());
        }

    }
}
