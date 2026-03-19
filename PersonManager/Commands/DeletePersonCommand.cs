using MediatR;

namespace PersonManager.Commands
{
    public class DeletePersonCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}