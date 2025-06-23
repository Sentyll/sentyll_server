using System.Reflection;

namespace Sentyll.Domain.Common.Extensions;

public static class AssemblyExtensions
{

    public static List<Type> GetTypesFromAssembly<T>(this Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(t =>
                typeof(T).IsAssignableFrom(t) &&
                !t.IsInterface && !t.IsAbstract
            ).ToList();
    }
    
    public static List<T> ConstructTypesFromAssembly<T>(this Assembly assembly)
    {
        return assembly.GetTypesFromAssembly<T>()
            .Select(type => (T)Activator.CreateInstance(type)!)
            .ToList();
    }
}