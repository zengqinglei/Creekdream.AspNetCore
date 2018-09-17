using System;
using AutoMapper;

namespace Creekdream.Mapping.AutoMapper
{
    /// <summary>
    /// Mark target object mapped to this object
    /// </summary>
    public class AutoMapFromAttribute : AutoMapAttributeBase
    {
        /// <inheritdoc />
        public override void CreateMap(IMapperConfigurationExpression configuration, Type type)
        {
            CreateMap(configuration, type, MemberList.Destination);
        }
    }
}

