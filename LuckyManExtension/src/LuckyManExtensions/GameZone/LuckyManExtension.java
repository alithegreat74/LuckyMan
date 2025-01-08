package LuckyManExtensions.GameZone;
import LuckyManExtensions.GameZone.Leaderboard.LeaderboardHandler;
import LuckyManExtensions.GameZone.Login.LoginHandler;
import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class LuckyManExtension extends SFSExtension {
    @Override
    public void init() {
        addEventHandler(SFSEventType.USER_LOGIN, LoginHandler.class);
        addRequestHandler("getLeaderboard", LeaderboardHandler.class);
    }
    @Override
    public void destroy() {
        super.destroy();
    }
}
