using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Input2
{
    public static float hor = float.NaN;
    public static bool? jump = null;
    public static bool? crouch = null;

    public static float GetAxis(string s)
    {
        return s == "Horizontal" ? float.IsNaN(hor) ? Input.GetAxis(s) : hor : 0;
    }
    public static bool GetButton(string s)
    {
        return s == "Jump" ? jump == null ? Input.GetButton(s) : jump.Value :
             s == "Crouch" ? crouch == null ? Input.GetButton(s) : crouch.Value :
            false;
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
