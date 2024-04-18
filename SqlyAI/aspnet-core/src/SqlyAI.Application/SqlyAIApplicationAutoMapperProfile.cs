using AutoMapper;
using SqlyAI.Books;

namespace SqlyAI;

public class SqlyAIApplicationAutoMapperProfile : Profile
{
    public SqlyAIApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();

    }
}
