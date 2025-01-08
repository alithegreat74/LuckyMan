package LuckyManExtensions.GameZone.Leaderboard;

import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import com.smartfoxserver.v2.extensions.ExtensionLogLevel;

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
        try{
            ISFSArray queryResult = dbManager.executeQuery(query,new Object[]{});
            response.putBool("success", true);
            response.putSFSArray("users",queryResult);
            send("getLeaderboard",response,user);

        }
        catch(SQLException e){
            trace(ExtensionLogLevel.WARN, "SQL Failed: " + e.toString());
            response.putBool("success", false);
            response.putUtfString("users",e.getMessage());
        }


    }
}
