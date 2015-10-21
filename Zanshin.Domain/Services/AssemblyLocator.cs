namespace Zanshin.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Compilation;

    public sealed class AssemblyLocator
    {
        private HashSet<Assembly> allAssemblies;
        private HashSet<Assembly> binAssemblies;
        private HashSet<Assembly> potentialDependencyAssemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyLocator"/> class.
        /// </summary>
        public AssemblyLocator()
        {
            this.Messages = new List<string>();
        }


        /// <summary>
        /// Loads the assemblies.
        /// </summary>
        public void LoadAssemblies()
        {
            try
            {
                this.allAssemblies = new HashSet<Assembly>(BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList());
            }
            catch (InvalidOperationException ioe)
            {
                this.Messages.Add(ioe.Message);
                this.allAssemblies = new HashSet<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
            }


            IList<Assembly> assemblies = new List<Assembly>();
            IList<Assembly> dependencyAssemblies = new List<Assembly>();

            var binFolder = HttpContext.Current != null
                ? HttpRuntime.AppDomainAppPath + @"bin\" : AppDomain.CurrentDomain.BaseDirectory + @"\";
            var dllFiles = Directory.GetFiles(binFolder, "*.dll", SearchOption.TopDirectoryOnly).ToList();

            foreach (var dllFile in dllFiles)
            {
                var assemblyName = AssemblyName.GetAssemblyName(dllFile);

                var locatedAssembly =
                    this.allAssemblies.FirstOrDefault(a => AssemblyName.ReferenceMatchesDefinition(a.GetName(), assemblyName));

                if (locatedAssembly != null)
                {
                    assemblies.Add(locatedAssembly);
                    if (!IgnoreDllForDependencies(Path.GetFileName(dllFile)))
                    {
                        dependencyAssemblies.Add(locatedAssembly);
                    }
                }
            }

            this.binAssemblies = new HashSet<Assembly>(assemblies);
            this.potentialDependencyAssemblies = new HashSet<Assembly>(dependencyAssemblies);
        }

        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        /// <returns></returns>
        public HashSet<Assembly> AllAssemblies
        {
            get { return this.allAssemblies; }
        }

        /// <summary>
        /// Gets the bin folder assemblies.
        /// </summary>
        /// <returns></returns>
        public HashSet<Assembly> BinFolderAssemblies
        {
           get { return this.binAssemblies; }
        }

        /// <summary>
        /// Gets the potential dependency assemblies.
        /// </summary>
        /// <returns></returns>
        public HashSet<Assembly> PotentialDependencyAssemblies
        {
            get { return this.potentialDependencyAssemblies;}
        }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public List<string> Messages { get; set; }

        private static bool IgnoreDllForDependencies(string fileName)
        {
            var ignoredFileNames = new[]
            {
                "antlr.runtime.dll"
                , "Antlr3.Runtime.dll"
                , "Avalara.AvaTax.Adapter.dll"
                , "AWSSDK.dll"
                , "BookSleeve.dll"
                , "CarlosAg.ExcelXmlWriter.dll"
                , "Castle.Core.dll"
                , "Castle.DynamicProxy2.dll"
                , "Castle.MicroKernel.dll"
                , "CKFinder.dll"
                , "CyberSource.Base.dll"
                , "CyberSource.Base.dll"
                , "CyberSource.Clients.dll"
                , "CyberSource.Clients.XmlSerializers.dll"
                , "CyberSource.WSSecurity.dll"
                , "CybsWSSecurityIOP.dll"
                , "dotless.Core.dll"
                , "Elmah.dll"
                , "Elmah.Mvc.dll"
                , "Enyim.Caching.dll"
                , "eSELECTplus_dotNet_API.dll"
                , "FedEx.dll"
                , "FluentValidation.dll"
                , "FluentValidation.Mvc.dll"
                , "FluorineFx.dll"
                , "GemBox.Spreadsheet.dll"
                , "HttpWebAdapters.dll"
                , "Harbour.RedisSessionStateStore.dll"
                , "Ionic.Zip.dll"
                , "Iesi.Collections.dll"
                , "ImageResizer.dll"
                , "ImageResizer.Mvc.dll"
                , "IOWA.dll"
                , "log4net.dll"
                , "Lucene.Net.dll"
                , "MemcachedProviders.dll"
                , "MiniProfiler.dll"
                , "Newtonsoft.Json.dll"
                , "NHibernate.dll"
                , "Owin.dll"
                , "Payflow_dotNET.dll"
                , "paypal_base.dll"
                , "RazorEngine.dll"
                , "RadInput.Net2.dll"
                , "RemoteActiveDirectory.dll"
                , "RouteDebug.dll"
                //DO NOT INCLUDE THIS ONE! (this is a note to myself), "SecurityProvider_Testing.dll"
                , "ServiceStack.Common.dll"
                , "ServiceStack.Interfaces.dll"
                , "ServiceStack.Redis.dll"
                , "ServiceStack.Text.dll"
                , "SolrNet.dll"
                , "SolrNet.DSL.dll"
                , "Unity.SolrNetIntegration.dll"
                , "Wurfl.Aspnet.Extensions.dll"
                , "Wurfl.dll"
                , "Unity.WebApi.dll"
            };

            return ignoredFileNames.Any(o => EqualsIgnoreCase(o, fileName))
                || fileName.StartsWith("NHibernate.Caches")
                || fileName.StartsWith("Microsoft")
                || fileName.StartsWith("CacheCow")
                || fileName.StartsWith("AutoMapper")
                || fileName.StartsWith("System.");
        }

        private static bool EqualsIgnoreCase(string value, string otherValue)
        {
            return string.Compare(value, otherValue, true, CultureInfo.InvariantCulture) == 0;
        }
    }
}