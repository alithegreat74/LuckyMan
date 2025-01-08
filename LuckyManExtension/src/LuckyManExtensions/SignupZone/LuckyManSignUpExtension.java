package LuckyManExtensions.SignupZone;
import com.smartfoxserver.v2.extensions.SFSExtension;
public class LuckyManSignUpExtension extends SFSExtension {
    @Override
    public void init() {
        addRequestHandler("signUp", SignUpHandler.class);
    }
    @Override
    public void destroy() {
        super.destroy();
    }
}