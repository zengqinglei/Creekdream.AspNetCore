using System;
using AutoMapper;

namespace Creekdream.AutoMapper
{
    /// <summary>
    /// Mark as a two-way mapping
    /// </summary>
    public class AutoMapAttribute : AutoMapAttributeBase
    {
        /// <summary>
        /// Create a two-way entity mapping relationship
        /// </summary>
        public override void CreateMap(IMapperConfigurationExpression configuration, Type type)
        {
            CreateMap(configuration, type, MemberList.Destination);
            CreateMap(configuration, type, MemberList.Source);
        }
    }
}

