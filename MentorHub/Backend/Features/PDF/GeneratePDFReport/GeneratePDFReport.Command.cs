using MediatR;

namespace Backend.Features.PDF.GeneratePDFReport
{
    public record Command(long UserId) : IRequest<Response>
    {

    }

    public record Response
    {
        public byte[] PdfContent { get; init; }
        public string FileName { get; init; }
    }
}
