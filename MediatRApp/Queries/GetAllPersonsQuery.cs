using MediatR;
using MediatRApp.DTO;
using System.Collections.Generic;

namespace MediatRApp.Queries
{
    public class GetAllPersonsQuery : IRequest<List<PersonResponseDto>>
    {
    }
}