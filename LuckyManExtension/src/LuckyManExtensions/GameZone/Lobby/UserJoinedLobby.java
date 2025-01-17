package LuckyManExtensions.GameZone.Lobby;
import com.smartfoxserver.v2.api.CreateRoomSettings;
import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.variables.RoomVariable;
import com.smartfoxserver.v2.entities.variables.SFSRoomVariable;
import com.smartfoxserver.v2.exceptions.SFSCreateRoomException;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.exceptions.SFSJoinRoomException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

import java.util.List;
import java.util.UUID;

public class UserJoinedLobby extends BaseServerEventHandler {

    @Override
    public void handleServerEvent(ISFSEvent isfsEvent) throws SFSException {
        List<User> userLists = getParentExtension().getParentRoom().getUserList();
        if(userLists.size() <= 1)
            return;
        User user = (User) isfsEvent.getParameter(SFSEventParam.USER);
        userLists.remove(user);
        CreateRoomSettings cfg = getRoomSettings(user);
        try
        {
            trace("Creating room");
            Room newGameRoom = createRoom(cfg);
            addUserToRoom(user, newGameRoom);
            addUserToRoom(userLists.get(0), newGameRoom);
        }
        catch (SFSCreateRoomException e)
        {
            trace("Failed to create room " + e.getMessage());
        }
    }
    private Room createRoom(CreateRoomSettings cfg) throws SFSCreateRoomException
    {
        return getApi().createRoom(getParentExtension().getParentZone(), cfg, null);
    }
    private void addUserToRoom(User user, Room room) throws SFSJoinRoomException {
        //leave the old room and go to the new room
        getApi().joinRoom(user, room);
        getApi().leaveRoom(user, getParentExtension().getParentRoom());
    }
    private CreateRoomSettings getRoomSettings(User startingUser)
    {
        CreateRoomSettings cfg = new CreateRoomSettings();
        cfg.setName("GameRoom_"+ UUID.randomUUID().toString().replace("-", "").substring(0, 10));
        cfg.setGame(true);
        cfg.setMaxUsers(2);
        cfg.setExtension(new CreateRoomSettings.RoomExtensionSettings("LuckyMan","LuckyManExtensions.GameZone.Game.GameRoomExtension"));
        RoomVariable startingUseVar = new SFSRoomVariable("startingUser", startingUser.getId());
        cfg.setRoomVariables(List.of(startingUseVar));
        return cfg;
    }
}
