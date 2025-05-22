using Backend.Database;
using Backend.Models;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Backend.Features.PDF.GeneratePDFReport
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Command> _validator;


        public Handler(ApplicationDbContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }


        public byte[] GenerateUserProjectsReport(string userName, IEnumerable<Project> projects, Dictionary<long, IEnumerable<Models.Task>> projectTasks)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(memoryStream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph($"Project Report for {userName}")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20));
                document.Add(new Paragraph($"Generated on {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12));
                document.Add(new Paragraph("\n"));

                foreach (var project in projects)
                {
                    document.Add(new Paragraph($"Project: {project.Title}")
                        .SetFontSize(16));

                    Table projectTable = new Table(2).UseAllAvailableWidth();

                    projectTable.AddCell(new Cell().Add(new Paragraph("ID")));
                    projectTable.AddCell(new Cell().Add(new Paragraph(project.Id.ToString())));
                    projectTable.AddCell(new Cell().Add(new Paragraph("Description")));
                    projectTable.AddCell(new Cell().Add(new Paragraph(project.Description)));
                    projectTable.AddCell(new Cell().Add(new Paragraph("Start Date")));
                    projectTable.AddCell(new Cell().Add(new Paragraph(project.StartDate.ToString("yyyy-MM-dd"))));
                    projectTable.AddCell(new Cell().Add(new Paragraph("End Date")));
                    projectTable.AddCell(new Cell().Add(new Paragraph(project.EndDate.ToString("yyyy-MM-dd"))));
                    projectTable.AddCell(new Cell().Add(new Paragraph("Status")));
                    projectTable.AddCell(new Cell().Add(new Paragraph(project.Status.ToString())));
                    projectTable.AddCell(new Cell().Add(new Paragraph("Points")));
                    projectTable.AddCell(new Cell().Add(new Paragraph(project.Points.ToString())));

                    document.Add(projectTable);
                    document.Add(new Paragraph("\n"));

                    if (projectTasks.ContainsKey(project.Id) && projectTasks[project.Id].Any())
                    {
                        document.Add(new Paragraph("Tasks:").SetFontSize(14));
                        Table taskTable = new Table(5).UseAllAvailableWidth();
                        taskTable.AddHeaderCell(new Cell().Add(new Paragraph("Task ID")));
                        taskTable.AddHeaderCell(new Cell().Add(new Paragraph("Title")));
                        taskTable.AddHeaderCell(new Cell().Add(new Paragraph("Status")));
                        taskTable.AddHeaderCell(new Cell().Add(new Paragraph("Points")));
                        taskTable.AddHeaderCell(new Cell().Add(new Paragraph("Deadline")));

                        foreach (var task in projectTasks[project.Id])
                        {
                            taskTable.AddCell(new Cell().Add(new Paragraph(task.Id.ToString())));
                            taskTable.AddCell(new Cell().Add(new Paragraph(task.Title)));
                            taskTable.AddCell(new Cell().Add(new Paragraph(task.Status.ToString())));
                            taskTable.AddCell(new Cell().Add(new Paragraph(task.Points.ToString())));
                            taskTable.AddCell(new Cell().Add(new Paragraph(task.EndDate.ToString("yyyy-MM-dd"))));
                        }

                        document.Add(taskTable);
                    }
                    else
                    {
                        document.Add(new Paragraph("No tasks for this project."));
                    }

                    document.Add(new Paragraph("\n"));
                }

                document.Close();
                return memoryStream.ToArray();
            }
        }
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {request.UserId} not found.");
            }

            var projects = await _context.Task_Projects
                .Where(tp => tp.User_ID == request.UserId)
                .Include(tp => tp.Project)
                .Select(tp => tp.Project)
                .Distinct()
                .ToListAsync(cancellationToken);

            var projectTasks = new Dictionary<long, IEnumerable<Models.Task>>();

            foreach (var project in projects)
            {

                var tasks = await _context.Task_Projects
                    .Where(tp => tp.Project_ID == project.Id && tp.Task_ID != null)
                    .Include(tp => tp.Task)
                    .Select(tp => tp.Task)
                    .ToListAsync(cancellationToken);

                projectTasks.Add(project.Id, tasks);
            }

            var userName = $"{user.Name} {user.Surname}";
            var pdfContent = GenerateUserProjectsReport(userName, projects, projectTasks);

            return new Response
            {
                PdfContent = pdfContent,
                FileName = $"User_{userName}_Projects_Report_{DateTime.Now:yyyyMMdd}.pdf"
            };
        }
    }
}
