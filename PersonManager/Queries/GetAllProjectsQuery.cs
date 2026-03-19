using MediatR;
using PersonManager.Domain;
using System.Collections.Generic;

namespace PersonManager.Queries
{
    public class GetAllProjectsQuery : IRequest<List<Project>>
    {
    }
}
