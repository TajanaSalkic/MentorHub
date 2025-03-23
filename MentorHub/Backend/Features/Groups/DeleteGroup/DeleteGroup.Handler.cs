using Backend.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Features.Groups.DeleteGroup
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var group = await _context.Group
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            var group_user = await _context.Group_Users.Where(p => p.Group_ID == request.Id).ToListAsync(cancellationToken);

            if (!group_user.IsNullOrEmpty())
            {
                _context.Group_Users.RemoveRange(group_user);
                await _context.SaveChangesAsync(cancellationToken);
            }

            if (group == null)
            {
                return new Response
                {
                    Success = false,
                    Message = $"Group with ID {request.Id} not found."
                };
            }

            

            _context.Group.Remove(group);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Success = true,
                Message = $"Group with ID {request.Id} has been deleted."
            };
        }
    }
}
