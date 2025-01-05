package LuckyManExtensions.EventListeners;
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
        String password = (String)isfsEvent.getParameter(SFSEventParam.LOGIN_PASSWORD);
        ISession session = (ISession) isfsEvent.getParameter(SFSEventParam.SESSION);
        String query = "Select id from users where username = ? and password = ?";
        IDBManager dbManager = getParentExtension().getParentZone().getDBManager();
        try{
            ISFSArray queryResult = dbManager.executeQuery(query,new Object[]{username,password});

            if(queryResult.size()<=0)
            {
                SFSErrorData errData = new SFSErrorData(SFSErrorCode.LOGIN_BAD_PASSWORD);
                throw new SFSLoginException("wrong username or password" , errData);
            }
            int userID = queryResult.getInt(0);
            session.setProperty("dbID", userID);
        }
        catch (Exception e){
            trace("Database Error" + e.getMessage());
        }
    }
}
