package LuckyManExtensions.GameZone.Leaderboard;

import LuckyManExtensions.utilities.ScopedDatabaseConnection;
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import com.smartfoxserver.v2.extensions.ExtensionLogLevel;

import java.sql.ResultSet;
import java.sql.SQLException;

public class LeaderboardHandler extends BaseClientRequestHandler {

    @Override
    public void handleClientRequest(User user, ISFSObject isfsObject) {
        IDBManager dbManager = getParentExtension().getParentZone().getDBManager();
        ISFSObject response = new SFSObject();
        String query = "SELECT id, username, xp\n" +
                "FROM users\n" +
                "ORDER BY xp DESC\n" +
                "LIMIT 10;\n";
        try(ScopedDatabaseConnection connection =new ScopedDatabaseConnection(dbManager,query))
        {
            ResultSet resultSet = connection.executeQuery();
            ISFSArray queryResult = getLeaderboard(resultSet);
            response.putSFSArray("users", queryResult);
            response.putBool("success", true);
            send("getLeaderboard",response,user);
        }
        catch (SQLException e) {
            trace(ExtensionLogLevel.WARN, "SQL Failed: " + e.getMessage());
            response.putBool("success", false);
            response.putUtfString("errorMessage",e.getMessage());
        }
    }
    private ISFSArray getLeaderboard(ResultSet resultSet) throws SQLException {
        ISFSArray userInfoArray = new SFSArray();
        while(resultSet.next())
        {
            ISFSObject userInfo = new SFSObject();
            userInfo.putInt("id",resultSet.getInt("id"));
            userInfo.putUtfString("username",resultSet.getString("username"));
            userInfo.putInt("xp",resultSet.getInt("xp"));
            userInfoArray.addSFSObject(userInfo);
        }

        return userInfoArray;
    }
}
