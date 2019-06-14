using System;

namespace Creekdream.SimpleDemo.Interceptors
{
    /// <summary>
    /// Audit log attribute
    /// </summary>
    public class AuditLogAttribute : Attribute
    {
        /// <summary>
        /// Is disabled audit log (default: false)
        /// </summary>
        public bool IsDisabled { get; set; }
    }
}
