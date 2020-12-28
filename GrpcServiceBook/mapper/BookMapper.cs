using AutoMapper;
using Google.Protobuf.WellKnownTypes;

namespace GrpcServiceBook.mapper
{
  public  class BookMapper:Profile
    {
        public BookMapper()
        {
            CreateMap<Book, Common.model.Book>().ForMember(dest => dest.Isbn, opt => opt.MapFrom(src => src.Isbn));
            CreateMap<Book, Common.model.Book>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<Book, Common.model.Book>().ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author));
            CreateMap<Book, Common.model.Book>().ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language));
            CreateMap<Book, Common.model.Book>().ForMember(dest => dest.Pages, opt => opt.MapFrom(src => src.Pages));
            CreateMap<Book, Common.model.Book>().ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));

            CreateMap<Book, Common.model.Book>().ForMember(dest => dest.Published, opt => opt.MapFrom(src => src.Published.ToDateTime()));
            CreateMap<Book, Common.model.Book>().ForMember(dest => dest.Genre, opt => opt.MapFrom(src => (Genre)src.Genre));



            CreateMap<Common.model.Book,Book>().ForMember(dest => dest.Isbn, opt => opt.MapFrom(src => src.Isbn));
            CreateMap<Common.model.Book, Book>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap< Common.model.Book,Book>().ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author));
            CreateMap<Common.model.Book, Book>().ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language));
            CreateMap<Common.model.Book, Book>().ForMember(dest => dest.Pages, opt => opt.MapFrom(src => src.Pages));
            CreateMap<Common.model.Book, Book>().ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));

            CreateMap<Common.model.Book, Book>().ForMember(dest => dest.Published, opt => opt.MapFrom(src => src.Published.ToTimestamp()));
            CreateMap<Common.model.Book, Book>().ForMember(dest => dest.Genre, opt => opt.MapFrom(src => (Common.model.Genre)src.Genre));
        }

    }
}
