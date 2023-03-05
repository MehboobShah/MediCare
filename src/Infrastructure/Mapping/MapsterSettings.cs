using Mapster;
using MediCare.Application.Ontology;
using MediCare.Domain.Ontology;

namespace MediCare.Infrastructure.Mapping;

public class MapsterSettings
{
    public static void Configure()
    {
        // here we will define the type conversion / Custom-mapping
        // More details at https://github.com/MapsterMapper/Mapster/wiki/Custom-mapping

        // This one is actually not necessary as it's mapped by convention
         TypeAdapterConfig<Keyword, KeywordDto>.NewConfig().Map(dest => dest.Synonyms, src => src.Dictionaries.Select(d => d.Name));
    }
}