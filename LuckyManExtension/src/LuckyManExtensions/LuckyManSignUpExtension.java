package LuckyManExtensions;

import LuckyManExtensions.Handlers.SignUpHandler;
import com.smartfoxserver.v2.components.signup.PasswordMode;
import com.smartfoxserver.v2.components.signup.SignUpAssistantComponent;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class LuckyManSignUpExtension extends SFSExtension {
    @Override
    public void init() {
        SignUpHandler signupHandler = new SignUpHandler();
        addRequestHandler(SignUpAssistantComponent.COMMAND_PREFIX, signupHandler.getSignUpAssistant());
    }
    @Override
    public void destroy() {
        super.destroy();
    }
}