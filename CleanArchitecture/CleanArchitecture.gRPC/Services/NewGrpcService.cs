using CleanArchitecture.Application.Features.New.Commands.CreateNew;
using CleanArchitecture.Application.Features.New.Commands.DeleteNew;
using CleanArchitecture.Application.Features.New.Commands.UpdateNew;
using CleanArchitecture.Application.Features.New.Queries.GetAllNew;
using CleanArchitecture.Application.Features.New.Queries.GetNewById;
using CleanArchitecture.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace CleanArchitecture.Grpc.Services
{
    public class NewGrpcService : NewService.NewServiceBase
    {
        private readonly ISender _mediator;

        public NewGrpcService(ISender mediator)
        {
            _mediator = mediator;
        }

        public override async Task<GetAllNewsResponse> GetAllNews(GetAllNewsRequest request, ServerCallContext context)
        {
            var news = await _mediator.Send(new GetAllNewQuery());

            var response = new GetAllNewsResponse();
            response.Items.AddRange(news.Select(n => new NewMessage
            {
                NewsId = n.NewsId,
                Title = n.Title,
                Slug = n.Slug ?? string.Empty,
                Summary = n.Summary ?? string.Empty,
                Content = n.Content ?? string.Empty
            }));

            return response;
        }

        public override async Task<GetNewByIdResponse> GetNewById(GetNewByIdRequest request, ServerCallContext context)
        {
            var newsItem = await _mediator.Send(new GetNewByIdQuery { NewId = request.Id });

            if (newsItem == null)
                return new GetNewByIdResponse { Found = false };

            return new GetNewByIdResponse
            {
                Found = true,
                New = new NewMessage
                {
                    NewsId = newsItem.NewsId,
                    Title = newsItem.Title,
                    Slug = newsItem.Slug ?? string.Empty,
                    Summary = newsItem.Summary ?? string.Empty,
                    Content = newsItem.Content ?? string.Empty
                }
            };
        }

        public override async Task<CreateNewResponse> CreateNew(CreateNewRequest request, ServerCallContext context)
        {
            var savedNew = await _mediator.Send(new CreateNewCommand
            (
                 request.Title,
                 request.Slug,
                 request.Summary,
                 request.Content
            ));

            return new CreateNewResponse { Id = savedNew.NewsId };
        }

        public override async Task<UpdateNewResponse> UpdateNew(UpdateNewRequest request, ServerCallContext context)
        {
            var updatedNew = await _mediator.Send(new UpdateNewCommand
            (
                 request.Id,
                 request.Title,
                 request.Slug,
                 request.Summary,
                 request.Content
            ));

            return new UpdateNewResponse { Id = updatedNew.NewsId };
        }

        public override async Task<DeleteNewByIdResponse> DeleteNewById(DeleteNewByIdRequest request, ServerCallContext context)
        {
            try
            {
                await _mediator.Send(new DeleteNewCommand { NewId = request.Id });
                return new DeleteNewByIdResponse { Success = true };
            }
            catch (Exception)
            {
                return new DeleteNewByIdResponse { Success = false };
            }
        }
    }
}