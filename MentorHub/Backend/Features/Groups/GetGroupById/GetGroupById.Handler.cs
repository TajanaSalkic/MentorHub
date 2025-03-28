using Backend.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Features.Groups.GetGroupById
{
    public class Handler : IRequestHandler<Command, Response>
    {

        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {

            var group = _context.Group
                .Where(x => x.Id == request.Id).FirstOrDefault();

            return new Response
            {
                Group = group
            };
        }
    }
}
