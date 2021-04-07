using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Input2
{
    public static float hor = float.NaN;
    public static float vert = float.NaN;
    public static bool? jump = null;
    public static bool? crouch = null;
    public static bool? hit = null;
    public static bool? overrideFlip = null;

    public static float GetAxis(string s)
    {
        return 
            s == "Horizontal" && !float.IsNaN(hor) ? hor : 
            s == "Vertical" && !float.IsNaN(vert) ? vert : 
            Input.GetAxis(s);
    }

    public static bool GetButton(string s)
    {
        return s == "Jump" && jump != null ? jump.Value :
             s == "Fire1" && hit != null ? hit.Value :
             s == "Fire2" && crouch != null ? crouch.Value :
            Input.GetButton(s);
    }
    public static bool GetButtonDown(string s)
    {
        return s == "Jump" && jump != null ? jump.Value :
             s == "Fire1" && hit != null ? hit.Value :
             s == "Fire2" && crouch != null ? crouch.Value :
            Input.GetButtonDown(s);
    }

    public static bool GetButtonUp(string s)
    {
        return s == "Jump" && jump != null ? !jump.Value :
             s == "Fire1" && hit != null ? !hit.Value :
             s == "Fire2" && crouch != null ? !crouch.Value :
            Input.GetButtonUp(s);
    }
}

class JoyInputController
{
    internal static float m_forward;
    internal static bool m_jump;
    internal static bool m_crouch;
    internal static bool m_attackPrimary;
    internal static bool m_attackSecondary;
}
public static class InputManager
{

    public static float Forward()
    {
        float kb = Input2.GetAxis("Horizontal");

        kb += JoyInputController.m_forward;
        kb = Mathf.Clamp(kb, -1f, 1f);

        return kb;
    }

    public static bool Jump()
    {
        bool kb = Input2.GetButton("Jump") || JoyInputController.m_jump;
        //private joy

        return kb;
    }

    public static bool Crouch()
    {
        bool kb = Input2.GetButton("Fire1") || JoyInputController.m_crouch;
        //private joy

        return kb;
    }

    public static bool AttackPrimary()
    {
        bool kb = Input2.GetButton("Fire2") || JoyInputController.m_attackPrimary;
        //private joy

        return kb;
    }

    public static bool AttackSecondary()
    {
        bool kb = Input2.GetButton("Fire3") || JoyInputController.m_attackSecondary;
        //private joy

        return kb;
    }

    public static bool JumpPressForGrab()
    {
        bool kb = Input2.GetButton("Jump") || JoyInputController.m_jump;

        return kb;
    }
}
