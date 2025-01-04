package LuckyManExtensions.EventListeners;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

import java.util.List;

public class UserLeftGame extends BaseServerEventHandler {
    @Override
    public void handleServerEvent(ISFSEvent isfsEvent) throws SFSException {
        //if one player left the room, make the other one leave as well
        List<User> users = getParentExtension().getParentRoom().getUserList();
        if(users.isEmpty())
            return;
        getApi().removeRoom(getParentExtension().getParentRoom());
    }
}
