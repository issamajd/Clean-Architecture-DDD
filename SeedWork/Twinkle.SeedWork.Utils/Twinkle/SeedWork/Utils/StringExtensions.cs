using System.Collections;

namespace Twinkle.SeedWork.Utils;

public static class StringExtensions
{
    public static string BindObjectProperties(this string str, object? obj)
    {
        if (obj == null) return str;
        foreach (var item in ExtractParams(str))
        {
            str = str.Replace("{" + item + "}", obj.GetPropValue(item)?.ToString());
        }

        return str;
    }


    private static object? GetPropValue(this object? obj, string name)
    {
        foreach (var part in name.Split('.'))
        {
            if (obj == null)
            {
                return null;
            }

            if (obj.IsNonStringEnumerable())
            {
                var toEnumerable = (IEnumerable)obj;
                var iterator = toEnumerable.GetEnumerator();
                if (!iterator.MoveNext())
                {
                    return null;
                }

                obj = iterator.Current;
            }

            var type = obj!.GetType();
            var info = type.GetProperty(part);
            if (info == null)
            {
                return null;
            }

            obj = info.GetValue(obj, null);
        }

        return obj;
    }

    private static IEnumerable<string> ExtractParams(string str)
    {
        var split = str.Split('{', '}');
        for (var i = 1; i < split.Length; i += 2)
            yield return split[i];
    }

    private static bool IsNonStringEnumerable(this object? instance)
    {
        return instance != null && instance.GetType().IsNonStringEnumerable();
    }

    private static bool IsNonStringEnumerable(this Type type)
    {
        return type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);
    }
}