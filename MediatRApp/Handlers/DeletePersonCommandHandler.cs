using MediatR;
using MediatRApp.Commands;
using MediatRApp.Repositories;

namespace MediatRApp.Handlers
{
    public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, bool>
    {
        private readonly IPersonRepository _repository;
        public DeletePersonCommandHandler(IPersonRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            var person = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (person == null) return false;
            await _repository.DeleteAsync(request.Id, cancellationToken);
            return true;
        }
    }
}