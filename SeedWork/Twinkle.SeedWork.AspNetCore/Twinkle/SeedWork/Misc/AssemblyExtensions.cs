using System.Reflection;

namespace Twinkle.SeedWork.Misc;

public static class AssemblyExtensions
{
    public static IEnumerable<AssemblyName> GetReferencingAssemblies(this Assembly assembly)
    {
        //get all assemblies that have been loaded
        var reachableAssemblies = AppDomain.CurrentDomain
            .GetAssemblies().Select(domainAssembly => domainAssembly.GetName())
            .Distinct()
            .ToList();

        int assembliesDiscoveredCount;
        do
        {
            var newAssemblies = new List<AssemblyName>();
            assembliesDiscoveredCount = reachableAssemblies.Count;
            reachableAssemblies.ForEach(reachableAssembly =>
            {
                var newReferencedAssemblies =
                    Assembly.Load(reachableAssembly).GetReferencedAssemblies()
                        .Where(referencedAssembly => reachableAssemblies.All(anyAssembly => //check that is not included before
                                                         anyAssembly.FullName != referencedAssembly.FullName) &&
                                                     newAssemblies.All(newAssembly =>
                                                         newAssembly.FullName != referencedAssembly.FullName));
                newAssemblies.AddRange(newReferencedAssemblies);
            });
            reachableAssemblies.AddRange(newAssemblies);
        } while (assembliesDiscoveredCount < reachableAssemblies.Count);

        return reachableAssemblies.Where(reachableAssembly =>
            Assembly.Load(reachableAssembly).GetReferencedAssemblies()
                .Any(referenced => referenced.FullName == assembly.FullName));
    }
}