package LuckyManExtensions.GameZone.Login;
import com.smartfoxserver.bitswarm.sessions.ISession;
import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.exceptions.SFSErrorCode;
import com.smartfoxserver.v2.exceptions.SFSErrorData;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.exceptions.SFSLoginException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

public class LoginHandler extends BaseServerEventHandler {

    @Override
    public void handleServerEvent(ISFSEvent isfsEvent) throws SFSException {
        String username = (String)isfsEvent.getParameter(SFSEventParam.LOGIN_NAME);
        String cryptedPass = (String)isfsEvent.getParameter(SFSEventParam.LOGIN_PASSWORD);
        ISession session = (ISession) isfsEvent.getParameter(SFSEventParam.SESSION);
        String query = "SELECT id, password FROM users WHERE username = ?";
        IDBManager dbManager = getParentExtension().getParentZone().getDBManager();

        try{
            ISFSArray queryResult = dbManager.executeQuery(query,new Object[]{username});
            if(queryResult.size()<=0)
            {
                SFSErrorData errData = new SFSErrorData(SFSErrorCode.LOGIN_BAD_USERNAME);
                throw new SFSLoginException("wrong username or password" , errData);
            }
            String storedHashedPassword = queryResult.getSFSObject(0).getUtfString("password");
            if(!getApi().checkSecurePassword(session,storedHashedPassword,cryptedPass)){
                SFSErrorData data = new SFSErrorData(SFSErrorCode.LOGIN_BAD_PASSWORD);
                data.addParameter(username);
                throw new SFSLoginException("Login failed for user: "  + username, data);
            }
            int userID = queryResult.getSFSObject(0).getInt("id");
            session.setProperty("dbID", userID);
        }
        catch (Exception e){
            trace("Database Error " + e.getMessage());
            SFSErrorData errData = new SFSErrorData(SFSErrorCode.LOGIN_BAD_PASSWORD);
            throw new SFSLoginException("wrong username or password");
        }
    }
}
