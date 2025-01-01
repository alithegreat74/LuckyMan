package LuckyManExtensions.Handlers;

import com.smartfoxserver.v2.components.login.LoginAssistantComponent;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class LoginHandler {
    private final LoginAssistantComponent loginAssistantComponent;
    public LoginHandler(SFSExtension extension){
        loginAssistantComponent = new LoginAssistantComponent(extension);
    }
    public LoginAssistantComponent getLoginAssistantComponent()
    {
        return loginAssistantComponent;
    }
}
