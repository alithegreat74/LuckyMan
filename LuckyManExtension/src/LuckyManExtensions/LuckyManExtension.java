package LuckyManExtensions;
import LuckyManExtensions.Handlers.LeaderboardHandler;
import LuckyManExtensions.Handlers.LoginHandler;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class LuckyManExtension extends SFSExtension {
    private  LoginHandler loginHandler;
    @Override
    public void init() {
        loginHandler = new LoginHandler(this);
        addRequestHandler("getLeaderboard", LeaderboardHandler.class);
    }
    @Override
    public void destroy() {
        super.destroy();
    }
}
