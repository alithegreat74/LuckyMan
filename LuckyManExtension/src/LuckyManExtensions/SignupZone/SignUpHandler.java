package LuckyManExtensions.SignupZone;
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;


public class SignUpHandler extends BaseClientRequestHandler {

    @Override
    public void handleClientRequest(User user, ISFSObject isfsObject) {
        ISFSObject response = SFSObject.newInstance();
        try{
            String username = isfsObject.getUtfString("username");
            String password = isfsObject.getUtfString("password");
            updateDatabase(username, password);
            response.putBool("success", true);
        }
        catch(SQLException e){
            trace("Database Error" + e.getMessage());
            response.putBool("success", false);
            response.putUtfString("error", e.getMessage());
        }
        catch(Exception e){
            trace(e.getMessage());
            response.putBool("success", false);
            response.putUtfString("error", e.getMessage());
        }
        send("signUp",response,user);

    }
    private void updateDatabase(String username, String password) throws SQLException {
        IDBManager dbManager = getParentExtension().getParentZone().getDBManager();
        Connection connection = dbManager.getConnection();
        PreparedStatement statement =
                connection.prepareStatement("Insert Into users Values(NULL, ?, ?, 0);");
        statement.setString(1, username);
        statement.setString(2, password);
        statement.executeUpdate();
        connection.close();
    }
}
