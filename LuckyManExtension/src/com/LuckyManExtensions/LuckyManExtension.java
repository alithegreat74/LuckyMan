package com.LuckyManExtensions;
import com.smartfoxserver.v2.components.signup.SignUpAssistantComponent;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class LuckyManExtension extends SFSExtension {
    @Override
    public void init() {
        SignUpAssistantComponent signUpAssistant = new SignUpAssistantComponent();
        signUpAssistant.getConfig().isEmailRequired = false;
        addRequestHandler(SignUpAssistantComponent.COMMAND_PREFIX, signUpAssistant);
    }
    @Override
    public void destroy() {
        super.destroy();
    }
}
