using MediatR;

namespace Backend.Features.PDF.GeneratePDFReportTaskChanges
{
   
        public record Command(long ProjectId) : IRequest<Response>
        {

        }

        public record Response
        {
            public byte[] PdfContent { get; init; }
            public string FileName { get; init; }
        }

}
