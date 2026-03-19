using MediatR;
using PersonManager.Commands;
using PersonManager.Repositories;

namespace PersonManager.Handlers
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
            var result = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (!result.Success || result.Data == null) return false;
            await _repository.DeleteAsync(request.Id, cancellationToken);
            return true;
        }
    }
}