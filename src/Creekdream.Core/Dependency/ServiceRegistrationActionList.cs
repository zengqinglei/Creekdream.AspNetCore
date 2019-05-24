using System;
using System.Collections.Generic;

namespace Creekdream.Dependency
{
    /// <summary>
    /// Service registration event collection
    /// </summary>
    public class ServiceRegistrationActionList : List<Action<IOnServiceRegistredContext>>
    {

    }
}
