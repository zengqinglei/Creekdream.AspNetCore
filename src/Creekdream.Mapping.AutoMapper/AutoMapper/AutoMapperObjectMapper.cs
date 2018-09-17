using AutoMapper;

namespace Creekdream.Mapping.AutoMapper
{
    /// <summary>
    /// Use Automapper mapping
    /// </summary>
    public class AutoMapperObjectMapper : IObjectMapper
    {
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public AutoMapperObjectMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <inheritdoc />
        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        /// <inheritdoc />
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }
    }
}
