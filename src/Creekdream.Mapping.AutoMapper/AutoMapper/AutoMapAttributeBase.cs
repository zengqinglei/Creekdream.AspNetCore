using AutoMapper;
using System;

namespace Creekdream.Mapping.AutoMapper
{
    /// <summary>
    /// Mapping relationships need to inherit abstract base classes
    /// </summary>
    public abstract class AutoMapAttributeBase : Attribute
    {
        /// <summary>
        /// Target type
        /// </summary>
        public Type[] TargetTypes { get; private set; }

        /// <inheritdoc />
        protected AutoMapAttributeBase(params Type[] targetTypes)
        {
            TargetTypes = targetTypes;
        }

        /// <summary>
        /// Create a specified mapping
        /// </summary>
        protected void CreateMap(IMapperConfigurationExpression configuration, Type type, MemberList memberList)
        {
            if (TargetTypes == null)
            {
                return;
            }

            foreach (var targetType in TargetTypes)
            {
                configuration.CreateMap(targetType, type, memberList);
            }
        }

        /// <summary>
        /// Create mappings
        /// </summary>
        public abstract void CreateMap(IMapperConfigurationExpression configuration, Type type);
    }
}

