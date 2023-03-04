using System.Reflection;
using Utils.Exceptions;

namespace Utils.Reflection;

public static class ReflectionUtils
{
    public static Type? FindType(string fullName) => AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).FirstOrDefault(t => fullName == t.FullName);

    public static ConstructorInfo? GetEmptyConstructor(Type type) =>
        type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .FirstOrDefault(c => c.GetParameters().Length == 0);

    public static ConstructorInfo? GetConstructor(Type type, params Type[] ctorTypes) =>
        type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .FirstOrDefault(c =>
            {
                var infos = c.GetParameters();
                if (ctorTypes.Length != infos.Length) return false;
                for (var index = 0; index < infos.Length; index++)
                {
                    var info = infos[index];
                    if (info.ParameterType != ctorTypes[index])
                        return false;
                }
                return true;
            });

    public static void InjectField(FieldInfo field, object target, object value)
    {
        ValidateField(field);
        field.SetValue(target, value);
    }

    public static void InjectField(string field, object target, object value)
    {
        var info = target.GetType().GetField(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (info is null) throw new ReflectionException("invalid field");
        ValidateField(info);
        info.SetValue(target, value);
    }
    public static bool TryInjectField(string field, object target, object value)
    {
        var info = target.GetType().GetField(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (info is null) return false;
        if (info.IsInitOnly) return false;
        info.SetValue(target, value);
        return true;
    }

    private static void ValidateField(FieldInfo info)
    {
        if (info.IsInitOnly) throw new ReflectionException("field is init only");
    }
    private static void ValidateProperty(PropertyInfo info)
    {
        if (info.SetMethod == null) throw new ReflectionException("property has no set method");
    }

    public static void InjectProperty(PropertyInfo prop, object target, object value)
    {
        prop.SetValue(target, value);
        ValidateProperty(prop);
    }

    public static void InjectProperty(string prop, object target, object value)
    {
        var info = target.GetType().GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (info is null) throw new ReflectionException("invalid field");
        ValidateProperty(info);
        info.SetValue(target, value);
    }
    public static bool TryInjectProperty(string prop, object target, object value)
    {
        var info = target.GetType().GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (info is null) return false;
        if (info.SetMethod == null) return false;
        info.SetValue(target, value);
        return true;
    }

    public static bool TryGetAttribute<T>(Type type, out T attribute) where T : Attribute
    {
        attribute = type.GetCustomAttributes().FirstOrDefault(a => a is T) as T;
        return attribute is not null;
    }

    public static MethodInfo? GetMethod(Type type, string name, ReflectionType types = ReflectionType.Instance | ReflectionType.Static, params Type[] parameters) => TryGetMethod(type, name, out var method, types, parameters) ? method : null;

    public static bool TryGetMethod(Type type, string name, out MethodInfo info, ReflectionType types = ReflectionType.Instance | ReflectionType.Static, params Type[] parameters)
    {
        var args = BindingFlags.Public | BindingFlags.NonPublic;
        if (types.HasFlag(ReflectionType.Instance))
            args |= BindingFlags.Instance;
        if (types.HasFlag(ReflectionType.Static))
            args |= BindingFlags.Static;

        var baseType = type;
        while (baseType != typeof(object) && baseType != null)
        {
            if (baseType.GetMethod(name, args) is
                { } method && method.GetParameters().AllEquals(parameters, (p, t) => p.ParameterType == t))
            {
                info = method;
                return true;
            }
            baseType = baseType.BaseType;
        }
        info = null;
        return false;
    }
}