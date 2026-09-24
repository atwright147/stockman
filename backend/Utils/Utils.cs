namespace StockMan.Utils;

public static class AppUtils
{
  public static bool ToBool(bool? value, bool defaultValue = false)
  {
    return value ?? defaultValue;
  }

  public static T Coalesce<T>(T? newValue, T oldValue)
  {
    return newValue ?? oldValue;
  }
}
