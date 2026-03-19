using MediatR;
using PersonManager.DTO;
using System.Collections.Generic;

namespace PersonManager.Queries
{
    public class GetAllPersonsQuery : IRequest<List<PersonResponseDto>>
    {
    }
}