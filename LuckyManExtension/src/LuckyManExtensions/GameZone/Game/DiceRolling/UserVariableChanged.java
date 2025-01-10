package LuckyManExtensions.GameZone.Game.DiceRolling;

import LuckyManExtensions.utilities.ScopedDatabaseConnection;
import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;

public class UserVariableChanged extends BaseServerEventHandler {
    @Override
    public void handleServerEvent(ISFSEvent isfsEvent) throws SFSException {
        User user = (User) isfsEvent.getParameter(SFSEventParam.USER);
        if(user.getVariable("currentScore").getIntValue()<30)
            return;
        try{
            updateDatabase(user);
            trace(String.format("Player %s won the game", user.getName()));
        }
        catch(SQLException e)
        {
            trace(e.getMessage());
        }
    }
    private void updateDatabase(User user) throws SQLException
    {
        IDBManager dbManager = getParentExtension().getParentZone().getDBManager();
        try(ScopedDatabaseConnection connection =
                    new ScopedDatabaseConnection(dbManager,"UPDATE users SET xp = xp + ? WHERE username = ?"))
        {
            connection.setInt(1,50);
            connection.setString(2,user.getName());
            connection.executeUpdate();
        }

    }
}
