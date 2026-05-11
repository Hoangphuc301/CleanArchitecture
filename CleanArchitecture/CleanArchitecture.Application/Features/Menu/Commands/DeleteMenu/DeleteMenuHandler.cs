

using AutoMapper;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.Menu.Commands.DeleteMenu
{
    public class DeleteMenuHandler : IRequestHandler<DeleteMenuCommand, int>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;   

        public DeleteMenuHandler(IMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteMenuCommand request, CancellationToken cancellationToken)
        {
            var result = await _menuRepository.DeleteAsync(request.MenuId);
            return result;
        }
    }
}
