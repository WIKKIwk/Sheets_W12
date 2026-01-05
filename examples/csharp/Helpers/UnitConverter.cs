namespace W12CSheets.Client.Helpers;

/// <summary>
/// Unit conversion helper utilities
/// </summary>
public static class UnitConverter
{
    // Temperature
    public static double CelsiusToFahrenheit(double celsius) => (celsius * 9/5) + 32;
    public static double FahrenheitToCelsius(double fahrenheit) => (fahrenheit - 32) * 5/9;
    public static double CelsiusToKelvin(double celsius) => celsius + 273.15;
    public static double KelvinToCelsius(double kelvin) => kelvin - 273.15;

    // Length
    public static double MetersToFeet(double meters) => meters * 3.28084;
    public static double FeetToMeters(double feet) => feet / 3.28084;
    public static double MilesToKilometers(double miles) => miles * 1.60934;
    public static double KilometersToMiles(double kilometers) => kilometers / 1.60934;
    public static double InchesToCentimeters(double inches) => inches * 2.54;
    public static double CentimetersToInches(double centimeters) => centimeters / 2.54;

    // Weight
    public static double KilogramsToPounds(double kilograms) => kilograms * 2.20462;
    public static double PoundsToKilograms(double pounds) => pounds / 2.20462;
    public static double OuncesToGrams(double ounces) => ounces * 28.3495;
    public static double GramsToOunces(double grams) => grams / 28.3495;

    // Volume
    public static double LitersToGallons(double liters) => liters * 0.264172;
    public static double GallonsToLiters(double gallons) => gallons / 0.264172;
    public static double MillilitersToFluidOunces(double ml) => ml * 0.033814;
    public static double FluidOuncesToMilliliters(double oz) => oz / 0.033814;

    // Data
    public static double BytesToKilobytes(double bytes) => bytes / 1024;
    public static double KilobytesToMegabytes(double kb) => kb / 1024;
    public static double MegabytesToGigabytes(double mb) => mb / 1024;
    public static double GigabytesToTerabytes(double gb) => gb / 1024;

    // Speed
    public static double MphToKmh(double mph) => mph * 1.60934;
    public static double KmhToMph(double kmh) => kmh / 1.60934;

    // Time
    public static double HoursToMinutes(double hours) => hours * 60;
    public static double MinutesToHours(double minutes) => minutes / 60;
    public static double DaysToHours(double days) => days * 24;
    public static double HoursToDays(double hours) => hours / 24;
}
