namespace Zanshin.Domain.Helpers.Interfaces
{
    using System.Collections.Specialized;
    using System.Configuration;

    public interface IConfigurationWrapper
    {
        NameValueCollection AppSettings { get; }
        ConnectionStringSettingsCollection ConnectionStrings { get; }
        object GetSection(string sectionName);
        Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel);
        Configuration OpenMachineConfiguration();
        void RefreshSection(string sectionName);
    }
}