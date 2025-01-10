package LuckyManExtensions.utilities;

import com.smartfoxserver.v2.db.IDBManager;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public class ScopedDatabaseConnection implements AutoCloseable{
    private final Connection connection;
    private PreparedStatement preparedStatement;
    public ScopedDatabaseConnection(IDBManager dbManager,String sql) throws SQLException {
        connection = dbManager.getConnection();
        preparedStatement = connection.prepareStatement(sql);
    }
    @Override
    public void close() throws SQLException {
        if(preparedStatement != null)
            preparedStatement.close();
        if(connection != null)
            connection.close();
    }
    public void setString(int index, String value) throws SQLException {
        preparedStatement.setString(index, value);
    }
    public void setInt(int index, int value) throws SQLException {
        preparedStatement.setInt(index, value);
    }
    public void setFloat(int index, float value) throws SQLException {
        preparedStatement.setFloat(index, value);
    }
    public ResultSet executeQuery() throws SQLException {
        ResultSet rs = preparedStatement.executeQuery();
        preparedStatement.clearParameters();
        return rs;
    }
    public int executeUpdate() throws SQLException {
        int rs = preparedStatement.executeUpdate();
        preparedStatement.clearParameters();
        return rs;
    }
}
