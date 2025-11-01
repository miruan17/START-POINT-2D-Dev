using UnityEngine;

public enum HotkeyType
{
    A, B, C, D, E, F, G, H, I, J,
    K, L, M, N, O, P, Q, R, S, T,
    U, V, W, X, Y, Z,
    LeftControl, RightControl,
    LeftAlt, RightAlt,
    LeftShift, RightShift
}

public static class HotkeyTypeExtensions
{
    public static KeyCode ToKeyCode(this HotkeyType type)
    {
        return type switch
        {
            HotkeyType.A => KeyCode.A,
            HotkeyType.B => KeyCode.B,
            HotkeyType.C => KeyCode.C,
            HotkeyType.D => KeyCode.D,
            HotkeyType.E => KeyCode.E,
            HotkeyType.F => KeyCode.F,
            HotkeyType.G => KeyCode.G,
            HotkeyType.H => KeyCode.H,
            HotkeyType.I => KeyCode.I,
            HotkeyType.J => KeyCode.J,
            HotkeyType.K => KeyCode.K,
            HotkeyType.L => KeyCode.L,
            HotkeyType.M => KeyCode.M,
            HotkeyType.N => KeyCode.N,
            HotkeyType.O => KeyCode.O,
            HotkeyType.P => KeyCode.P,
            HotkeyType.Q => KeyCode.Q,
            HotkeyType.R => KeyCode.R,
            HotkeyType.S => KeyCode.S,
            HotkeyType.T => KeyCode.T,
            HotkeyType.U => KeyCode.U,
            HotkeyType.V => KeyCode.V,
            HotkeyType.W => KeyCode.W,
            HotkeyType.X => KeyCode.X,
            HotkeyType.Y => KeyCode.Y,
            HotkeyType.Z => KeyCode.Z,
            HotkeyType.LeftControl => KeyCode.LeftControl,
            HotkeyType.RightControl => KeyCode.RightControl,
            HotkeyType.LeftAlt => KeyCode.LeftAlt,
            HotkeyType.RightAlt => KeyCode.RightAlt,
            HotkeyType.LeftShift => KeyCode.LeftShift,
            HotkeyType.RightShift => KeyCode.RightShift,
            _ => KeyCode.None
        };
    }
}
