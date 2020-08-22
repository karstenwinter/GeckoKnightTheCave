﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        float kb = Input.GetAxis("Horizontal");

        kb += JoyInputController.m_forward;
        kb = Mathf.Clamp(kb, -1f, 1f);

        return kb;
    }

    public static bool Jump()
    {
        bool kb = Input.GetButton("Jump") || JoyInputController.m_jump;
        //private joy

        return kb;
    }

    public static bool Crouch()
    {
        bool kb = Input.GetButton("Fire1") || JoyInputController.m_crouch;
        //private joy

        return kb;
    }

    public static bool AttackPrimary()
    {
        bool kb = Input.GetButtonDown("Fire2") || JoyInputController.m_attackPrimary;
        //private joy

        return kb;
    }

    public static bool AttackSecondary()
    {
        bool kb = Input.GetButtonDown("Fire3") || JoyInputController.m_attackSecondary;
        //private joy

        return kb;
    }

    public static bool JumpPressForGrab()
    {
        bool kb = Input.GetButton("Jump") || JoyInputController.m_jump;

        return kb;
    }
}
