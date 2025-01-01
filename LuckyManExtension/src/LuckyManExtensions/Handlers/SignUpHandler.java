package LuckyManExtensions.Handlers;
import com.smartfoxserver.v2.components.signup.PasswordMode;
import com.smartfoxserver.v2.components.signup.SignUpAssistantComponent;
import com.smartfoxserver.v2.extensions.SFSExtension;

import java.util.List;

public class SignUpHandler {
    private final SignUpAssistantComponent signUpAssistant;
    public SignUpHandler()
    {
        signUpAssistant = new SignUpAssistantComponent();
        signUpAssistant.getConfig().isEmailRequired = false;
        //signUpAssistant.getConfig().passwordMode = PasswordMode.MD5;
        signUpAssistant.getConfig().extraFields = List.of("score");
        signUpAssistant.getConfig().preProcessPlugin = (user, isfsObject, signUpConfiguration) -> {
            //Set the default value as 0

            if(isfsObject.getInt("score") ==null)
                isfsObject.putInt("score", 0);
        };
    }
    public SignUpAssistantComponent getSignUpAssistant()
    {
        return signUpAssistant;
    }
}
