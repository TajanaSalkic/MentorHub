using Backend.Models;
using MediatR;

namespace Backend.Features.Users.GetAllUsers
{
    // dodati da se proslijedi id ulogovanog mentora koji dodjeljuje zadatak pa da se po njegovoj grupi uzmu studenti
    public record Command : IRequest<Response>;


    public record Response
    {
        public IEnumerable<User> Users { get; init; }
    }
}
