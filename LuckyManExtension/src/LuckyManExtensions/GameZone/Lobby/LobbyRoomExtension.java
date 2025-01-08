package LuckyManExtensions.GameZone.Lobby;

import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.extensions.SFSExtension;
public class LobbyRoomExtension extends SFSExtension {
    @Override
    public void init() {
        addEventHandler(SFSEventType.USER_JOIN_ROOM,UserJoinedLobby.class);
    }
    @Override
    public void destroy() {
        super.destroy();
    }
}
