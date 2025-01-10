package LuckyManExtensions.GameZone.Login;
import LuckyManExtensions.utilities.Pair;
import LuckyManExtensions.utilities.ScopedDatabaseConnection;
import com.smartfoxserver.bitswarm.sessions.ISession;
import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.exceptions.SFSErrorCode;
import com.smartfoxserver.v2.exceptions.SFSErrorData;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.exceptions.SFSLoginException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;
import java.sql.ResultSet;
import java.sql.SQLException;

public class LoginHandler extends BaseServerEventHandler {

    @Override
    public void handleServerEvent(ISFSEvent isfsEvent) throws SFSException {
        trace("Checking login");
        try {
            String username = getUsername(isfsEvent);
            String cryptedPassword = getCryptedPassword(isfsEvent);
            ISession session = getSession(isfsEvent);
            IDBManager idbManager = getParentExtension().getParentZone().getDBManager();
            Pair<Integer,String> userInfo = getUserInfoFromDatabase(idbManager,username);
            checkInputPassword(session, cryptedPassword,userInfo.value);
            session.setProperty("dbID", userInfo.key);
        }
        catch (Exception e) {
            trace(e.getMessage());
            throw new SFSLoginException(e.getMessage());
        }

    }
    private String getUsername(ISFSEvent isfsEvent) {
        return (String)isfsEvent.getParameter(SFSEventParam.LOGIN_NAME);
    }
    private String getCryptedPassword(ISFSEvent isfsEvent) {
        return (String)isfsEvent.getParameter(SFSEventParam.LOGIN_PASSWORD);
    }
    private ISession getSession(ISFSEvent isfsEvent) {
        return (ISession) isfsEvent.getParameter(SFSEventParam.SESSION);
    }
    private Pair<Integer,String> getUserInfoFromDatabase(IDBManager dbManager, String username) throws SQLException, SFSException
    {
        try(ScopedDatabaseConnection connection
                    = new ScopedDatabaseConnection(dbManager,"Select id,password from users where username=?")) {
            connection.setString(1, username);
            ResultSet result = connection.executeQuery();
            checkResult(result);
            String password = result.getString("password");
            int id = result.getInt("id");
            return new Pair<Integer,String>(id,password);
        }

    }
    private void checkResult(ResultSet resultSet) throws SQLException,SFSLoginException {
        if (!resultSet.next())
        {
            SFSErrorData errData = new SFSErrorData(SFSErrorCode.LOGIN_BAD_USERNAME);
            throw new SFSLoginException("Bad user name: ", errData);
        }
    }

    private void checkInputPassword(ISession session, String cryptedPassword, String password) throws SFSLoginException {
        if(!getApi().checkSecurePassword(session, password,cryptedPassword)){
            SFSErrorData errData = new SFSErrorData(SFSErrorCode.LOGIN_BAD_PASSWORD);
            errData.addParameter(cryptedPassword);
            throw new SFSLoginException("Bad password: " + cryptedPassword, errData);
        }
    }
}
