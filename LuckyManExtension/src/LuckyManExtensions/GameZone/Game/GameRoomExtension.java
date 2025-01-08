package LuckyManExtensions.GameZone.Game;

import LuckyManExtensions.GameZone.DiceRolling.UserVariableChanged;
import LuckyManExtensions.GameZone.DiceRolling.DiceRollHandler;
import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class GameRoomExtension extends SFSExtension {
    @Override
    public void init() {
        addEventHandler(SFSEventType.USER_LEAVE_ROOM, UserLeftGame.class);
        addEventHandler(SFSEventType.USER_VARIABLES_UPDATE, UserVariableChanged.class);
        addRequestHandler("diceRoll", DiceRollHandler.class);
    }
    @Override
    public void destroy() {
        super.destroy();
    }
}
