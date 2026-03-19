using MediatR;

namespace MediatRApp.Commands
{
    public class DeletePersonCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}