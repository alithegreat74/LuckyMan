package LuckyManExtensions.GameZone.DiceRolling;

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
        Connection connection = getParentExtension().getParentZone().getDBManager().getConnection();
        PreparedStatement statement = connection.prepareStatement("UPDATE users SET xp = xp + ? WHERE username = ?");
        statement.setInt(1,50);
        statement.setString(2,user.getName());
        statement.executeUpdate();
        connection.close();
    }
}
