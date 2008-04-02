using System.Collections.Generic;

namespace ShoutcastIntegration
{
    public class ConfigurationService : IConfigurationService
    {
        #region IConfigurationService Members

        public List<string> Feeds { get; set; }

        #endregion
    }
}