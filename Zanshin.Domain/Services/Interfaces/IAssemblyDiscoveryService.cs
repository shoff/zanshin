namespace Zanshin.Domain.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Gets a collection of Assemblies in the specified bin directory that 
    /// are not on the ignored assemblies list.
    /// </summary>
    public interface IAssemblyDiscoveryService
    {
        /// <summary>
        /// Generates the dependency list.
        /// </summary>
        void GenerateDependencyList();

        /// <summary>
        /// Gets or sets the assembly list.
        /// </summary>
        /// <value>
        /// The assembly list.
        /// </value>
        HashSet<Assembly> AssemblyList { get; set; }
    }
}