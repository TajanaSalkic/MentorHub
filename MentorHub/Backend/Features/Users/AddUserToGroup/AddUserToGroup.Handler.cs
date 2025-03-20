using Backend.Database;
using Backend.Models;
using MediatR;

namespace Backend.Features.Users.AddUserToGroup
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
            var group_user = new Group_User
            {
                Group_ID = request.GroupId,
                User_ID = request.UserId,
                Mentor = request.Mentor
            };

            _context.Group_Users.Add(group_user);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Id = group_user.Id,
                GroupId = group_user.Group_ID,
                UserId = group_user.User_ID,
                Mentor = group_user.Mentor
            };
        }

    }
}
