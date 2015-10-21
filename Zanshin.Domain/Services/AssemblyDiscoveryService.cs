namespace Zanshin.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Zanshin.Domain.Exceptions;
    using Zanshin.Domain.Extensions;
    using Zanshin.Domain.Helpers.Interfaces;
    using Zanshin.Domain.Services.Interfaces;

    /// <summary>
    /// Gets a collection of Assemblies in the specified bin directory that 
    /// are not on the ignored assemblies list.
    /// </summary>
    public sealed class AssemblyDiscoveryService : IAssemblyDiscoveryService
    {
        private readonly string binDirectory;

        /// <summary>
        /// is the dependency discovery service.
        /// </summary>
        /// <param name="configurationWrapper">The configuration wrapper.</param>
        /// <exception cref="AppDomainUnloadedException">The operation is attempted on an unloaded application domain. </exception>
        public AssemblyDiscoveryService(IConfigurationWrapper configurationWrapper)
        {
            this.AssemblyList = new HashSet<Assembly>();
            this.binDirectory = AppDomain.CurrentDomain.BaseDirectory + (configurationWrapper.AppSettings["DependencyFolder"] ?? "bin");
        }

        /// <summary>
        /// Generates the dependency list.
        /// </summary>
        /// <exception cref="ApplicationException">Could not find assembly path for  + name</exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public void GenerateDependencyList()
        {
            // Generate an assembly list.
            var assemblyList = this.GetBaseDirectoryAssemblyList().Where(x => !IgnoredAssemblies.Instance.IsMatch(x));

            foreach (var assembly in assemblyList)
            {
                this.LoadAssemblyFromPath(assembly);
            }
        }

        /// <summary>
        /// Gets the base directory assembly list.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.IO.DirectoryNotFoundException"></exception>
        /// <exception cref="ArgumentNullException">directory</exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        private IEnumerable<string> GetBaseDirectoryAssemblyList()
        {

            if (!Directory.Exists(this.binDirectory))
            {
                throw new DirectoryNotFoundException(string.Format(Common.DirectoryNotFound, this.binDirectory));
            }

            return Directory.GetFiles(this.binDirectory, "*.dll");
        }

        /// <summary>
        /// Loads the assembly from path.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="ParameterNullException">name</exception>
        /// <exception cref="System.ApplicationException">Could not find assembly path for  + name</exception>
        private void LoadAssemblyFromPath(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ParameterNullException("name");
            }

            // pass true in to ensure that the string value returned is the full path 
            // which is required to load an assembly
            string path = name.FindPath(true);

            if (string.IsNullOrEmpty(path))
            {
                throw new ApplicationException("Could not find assembly path for " + name);
            }
            this.AssemblyList.Add(Assembly.LoadFrom(path));
        }

        /// <summary>
        /// Gets or sets the assembly list.
        /// </summary>
        /// <value>
        /// The assembly list.
        /// </value>
        public HashSet<Assembly> AssemblyList { get; set; }
    }
}