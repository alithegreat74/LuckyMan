package LuckyManExtensions.GameZone.Login;
import com.smartfoxserver.bitswarm.service.ISimpleService;
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

import java.sql.Connection;
import java.sql.PreparedStatement;
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
            Connection connection = getParentExtension().getParentZone().getDBManager().getConnection();
            ResultSet resultSet = getUserFromDatabase(connection, username);
            checkResult(resultSet);
            String password = getUserPassword(resultSet);
            checkInputPassword(session, cryptedPassword,password);
            int id = resultSet.getInt("id");
            session.setProperty("dbID", id);
            connection.close();
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
    private ResultSet getUserFromDatabase(Connection connection,String username) throws SQLException
    {
        PreparedStatement statement =
                connection.prepareStatement("Select id, password from users where username=?");
        statement.setString(1, username);
        return statement.executeQuery();
    }
    private void checkResult(ResultSet resultSet) throws SQLException,SFSLoginException {
        if (!resultSet.next())
        {
            SFSErrorData errData = new SFSErrorData(SFSErrorCode.LOGIN_BAD_USERNAME);
            throw new SFSLoginException("Bad user name: ", errData);
        }
    }
    private String getUserPassword(ResultSet resultSet) throws SQLException {
        return (String)resultSet.getString("password");
    }
    private void checkInputPassword(ISession session, String cryptedPassword, String password) throws SFSLoginException {
        if(!getApi().checkSecurePassword(session, password,cryptedPassword)){
            SFSErrorData errData = new SFSErrorData(SFSErrorCode.LOGIN_BAD_PASSWORD);
            errData.addParameter(cryptedPassword);
            throw new SFSLoginException("Bad password: " + cryptedPassword, errData);
        }
    }
}
