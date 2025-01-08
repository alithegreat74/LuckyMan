package LuckyManExtensions.SignupZone;
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;


public class SignUpHandler extends BaseClientRequestHandler {

    @Override
    public void handleClientRequest(User user, ISFSObject isfsObject) {
        ISFSObject response = new SFSObject();
        String username = "";
        String password = "";
        try{
            username = isfsObject.getUtfString("username");
            password = isfsObject.getUtfString("password");
        }
        catch(Exception e){
            trace("invalid input error");
            response.putBool("success",false);
            response.putUtfString("errorMessage","invalid input of username/password");
            send("signUp",response,user);
            return;
        }
        String databaseInsert = "Insert Into users Values(NULL, ?, ?, 0);";
        IDBManager dbManager = getParentExtension().getParentZone().getDBManager();

        try{
            dbManager.executeInsert(databaseInsert,new Object[]{username,password});
            response.putBool("success",true);
            response.putUtfString("message","User created successfully");
            send("signUp",response,user);

        }
        catch(Exception e){
            trace("Database Error",e);
            response.putBool("success",false);
            response.putUtfString("errorMessage","failed to create user");
            send("signUp",response,user);
        }

    }
}
