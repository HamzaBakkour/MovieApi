using AutoMapper;
using MovieApi.Models.Dtos;
using MovieApi.Models.Entities;

namespace MovieApi.Data;

public class MapperProfile : Profile
{

    public MapperProfile() {

        CreateMap<Movie, MovieDto>();
        CreateMap<Movie, MovieCreateDto>().ReverseMap();

        CreateMap<MovieDetailes, MovieDetailesCreateDto>().ReverseMap();

        CreateMap<Movie, MovieUpdateDto>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Detailes, opt => opt.Ignore())
            .ForMember(dest => dest.Reviews, opt => opt.Ignore())
            .ForMember(dest => dest.Actors, opt => opt.Ignore())
            .ForMember(dest => dest.Genres, opt => opt.Ignore());
        CreateMap<Movie, MovieAllDetailsDto>()
            .AfterMap((src, dest) =>
            {
                if (src.Actors == null || src.Actors.Count == 0)
                    dest.Actors = null;

                if (src.Genres == null || src.Genres.Count == 0)
                    dest.Genres = null;

                if (src.Reviews == null || src.Reviews.Count == 0)
                    dest.Reviews = null;
            });

        CreateMap<Actor, ActorDto>();
        CreateMap<Genre, GenreDto>();
        CreateMap<Review, ReviewDto>();
        CreateMap<MovieDetailes, MovieDetailesDto>();
    }

}





