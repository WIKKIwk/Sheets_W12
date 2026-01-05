namespace W12CSheets.Client.Helpers;

/// <summary>
/// Color helper utilities for console output
/// </summary>
public static class ColorHelper
{
    /// <summary>
    /// RGB color structure
    /// </summary>
    public struct RGB
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public RGB(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
    }

    /// <summary>
    /// Convert hex color to RGB
    /// </summary>
    public static RGB HexToRGB(string hex)
    {
        hex = hex.Replace("#", "");
        
        if (hex.Length != 6)
            throw new ArgumentException("Hex color must be 6 characters");

        byte r = Convert.ToByte(hex.Substring(0, 2), 16);
        byte g = Convert.ToByte(hex.Substring(2, 2), 16);
        byte b = Convert.ToByte(hex.Substring(4, 2), 16);

        return new RGB(r, g, b);
    }

    /// <summary>
    /// Convert RGB to hex color
    /// </summary>
    public static string RGBToHex(RGB rgb)
    {
        return $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
    }

    /// <summary>
    /// Get color brightness (0-255)
    /// </summary>
    public static double GetBrightness(RGB rgb)
    {
        return (0.299 * rgb.R + 0.587 * rgb.G + 0.114 * rgb.B);
    }

    /// <summary>
    /// Check if color is dark
    /// </summary>
    public static bool IsDark(RGB rgb)
    {
        return GetBrightness(rgb) < 128;
    }

    /// <summary>
    /// Lighten color by percentage
    /// </summary>
    public static RGB Lighten(RGB rgb, double percentage)
    {
        percentage = Math.Clamp(percentage, 0, 100);
        double factor = percentage / 100.0;

        return new RGB(
            (byte)Math.Min(255, rgb.R + (255 - rgb.R) * factor),
            (byte)Math.Min(255, rgb.G + (255 - rgb.G) * factor),
            (byte)Math.Min(255, rgb.B + (255 - rgb.B) * factor)
        );
    }

    /// <summary>
    /// Darken color by percentage
    /// </summary>
    public static RGB Darken(RGB rgb, double percentage)
    {
        percentage = Math.Clamp(percentage, 0, 100);
        double factor = 1 - (percentage / 100.0);

        return new RGB(
            (byte)(rgb.R * factor),
            (byte)(rgb.G * factor),
            (byte)(rgb.B * factor)
        );
    }

    /// <summary>
    /// Invert color
    /// </summary>
    public static RGB Invert(RGB rgb)
    {
        return new RGB(
            (byte)(255 - rgb.R),
            (byte)(255 - rgb.G),
            (byte)(255 - rgb.B)
        );
    }

    /// <summary>
    /// Generate random color
    /// </summary>
    public static RGB RandomColor()
    {
        var random = new Random();
        return new RGB(
            (byte)random.Next(256),
            (byte)random.Next(256),
            (byte)random.Next(256)
        );
    }

    /// <summary>
    /// Predefined colors
    /// </summary>
    public static class Colors
    {
        public static readonly RGB Red = new RGB(255, 0, 0);
        public static readonly RGB Green = new RGB(0, 255, 0);
        public static readonly RGB Blue = new RGB(0, 0, 255);
        public static readonly RGB Yellow = new RGB(255, 255, 0);
        public static readonly RGB Cyan = new RGB(0, 255, 255);
        public static readonly RGB Magenta = new RGB(255, 0, 255);
        public static readonly RGB Black = new RGB(0, 0, 0);
        public static readonly RGB White = new RGB(255, 255, 255);
        public static readonly RGB Gray = new RGB(128, 128, 128);
    }
}
