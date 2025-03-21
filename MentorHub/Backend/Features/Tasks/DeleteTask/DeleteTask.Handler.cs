﻿using Backend.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Features.Tasks.DeleteTask
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
            var task = await _context.Tasks
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            var project_users_tasks = await _context.Task_Projects
                .Where(p => p.Task_ID == request.Id).ToListAsync(cancellationToken);

            if (!project_users_tasks.IsNullOrEmpty())
            {
                _context.Task_Projects.RemoveRange(project_users_tasks);
                await _context.SaveChangesAsync(cancellationToken);

            }


            if (task == null)
            {
                return new Response
                {
                    Success = false,
                    Message = $"Project with ID {request.Id} not found."
                };
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Success = true,
                Message = $"Project with ID {request.Id} has been deleted."
            };
        }
    }
}
