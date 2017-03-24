﻿using Orchard.Environment.Shell.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orchard.Environment.Shell
{
    /// <summary>
    /// Represents the minimalistic set of fields stored for each tenant. This
    /// model is obtained from the IShellSettingsManager, which by default reads this
    /// from the App_Data settings.txt files.
    /// </summary>
    public class ShellSettings
    {
        private readonly IDictionary<string, string> _values;
        private TenantState _tenantState = TenantState.Invalid;
        private string[] _features = new string[0];

        public ShellSettings()
        {
            _values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            State = TenantState.Invalid;
        }

        public ShellSettings(ShellSettings settings)
        {
            _values = new Dictionary<string, string>(settings._values, StringComparer.OrdinalIgnoreCase);
            State = settings.State;
        }

        public string this[string key]
        {
            get
            {
                string retVal;
                return _values.TryGetValue(key, out retVal) ? retVal : null;
            }
            set { _values[key] = value; }
        }

        /// <summary>
        /// Gets all keys held by this shell settings.
        /// </summary>
        public IEnumerable<string> Keys { get { return _values.Keys; } }

        /// <summary>
        /// The name of the tenant
        /// </summary>
        public string Name
        {
            get { return this["Name"] ?? ""; }
            set { this["Name"] = value; }
        }

        /// <summary>
        /// The host name of the tenant
        /// </summary>
        public string RequestUrlHost
        {
            get { return this["RequestUrlHost"]; }
            set { this["RequestUrlHost"] = value; }
        }

        /// <summary>
        /// The request url prefix of the tenant
        /// </summary>
        public string RequestUrlPrefix
        {
            get { return this["RequestUrlPrefix"]; }
            set { _values["RequestUrlPrefix"] = value; }
        }

        public string ConnectionString
        {
            get { return this["ConnectionString"]; }
            set { _values["ConnectionString"] = value; }
        }

        public string TablePrefix
        {
            get { return this["TablePrefix"]; }
            set { _values["TablePrefix"] = value; }
        }

        public string DatabaseProvider
        {
            get { return this["DatabaseProvider"]; }
            set { _values["DatabaseProvider"] = value; }
        }

        public string[] Features
        {
            get
            {
                return _features ?? (Features = (_values["Features"] ?? "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                                                                     .Select(t => t.Trim())
                                                                     .ToArray();
            }
            set
            {
                _features = value;
                this["Features"] = string.Join(";", value);
            }
        }

        /// <summary>
        /// The state is which the tenant is
        /// </summary>
        public TenantState State
        {
            get { return _tenantState; }
            set
            {
                _tenantState = value;
                this["State"] = value.ToString();
            }
        }
    }
}
