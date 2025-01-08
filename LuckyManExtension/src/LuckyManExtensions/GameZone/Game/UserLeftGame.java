package LuckyManExtensions.GameZone.Game;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

import java.util.List;

public class UserLeftGame extends BaseServerEventHandler {
    @Override
    public void handleServerEvent(ISFSEvent isfsEvent) throws SFSException {
        //if one player left the room, make the other one leave as well
        Room currentRoom = getParentExtension().getParentRoom();
        List<User> users = currentRoom.getUserList();
        if( !users.isEmpty() || !currentRoom.isGame())
            return;
        getApi().removeRoom(currentRoom);
    }
}
