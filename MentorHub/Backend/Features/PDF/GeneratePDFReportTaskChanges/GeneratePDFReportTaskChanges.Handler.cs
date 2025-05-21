using Backend.Database;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using FluentValidation;

namespace Backend.Features.PDF.GeneratePDFReportTaskChanges
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

        public byte[] GenerateUserProjectsReport(
            string userName,
            IEnumerable<Project> projects,
            Dictionary<long, IEnumerable<Models.Task>> projectTasks,
            Dictionary<long, IEnumerable<Models.TaskChanges>> taskChanges)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(memoryStream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph($"Project Report for {userName}")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20));

                document.Add(new Paragraph($"Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12));

                document.Add(new Paragraph("\n"));

                foreach (var project in projects)
                {
                    document.Add(new Paragraph($"Project: {project.Title}").SetFontSize(16));

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

                            document.Add(taskTable);

                            if (taskChanges.ContainsKey(task.Id) && taskChanges[task.Id].Any())
                            {
                                document.Add(new Paragraph("Task Change History:").SetFontSize(12));

                                Table changeTable = new Table(4).UseAllAvailableWidth();
                                changeTable.AddHeaderCell(new Cell().Add(new Paragraph("Date")));
                                changeTable.AddHeaderCell(new Cell().Add(new Paragraph("Changed Field")));
                                changeTable.AddHeaderCell(new Cell().Add(new Paragraph("Old Value")));
                                changeTable.AddHeaderCell(new Cell().Add(new Paragraph("New Value")));

                                foreach (var change in taskChanges[task.Id])
                                {
                                    changeTable.AddCell(new Cell().Add(new Paragraph(change.ChangedAt.ToString("yyyy-MM-dd HH:mm"))));
                                    changeTable.AddCell(new Cell().Add(new Paragraph(change.FieldChanged)));
                                    changeTable.AddCell(new Cell().Add(new Paragraph(change.OldValue)));
                                    changeTable.AddCell(new Cell().Add(new Paragraph(change.NewValue)));
                                }

                                document.Add(changeTable);
                            }
                        }
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

            var projectExists = await _context.Projects
    .Include(p => p.TaskProjectUsers)
        .ThenInclude(tpu => tpu.User) 
    .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

            var user = projectExists?.TaskProjectUsers
                .Where(tpu => tpu.Project_ID== request.ProjectId && tpu.Creator==false) 
                .Select(tpu => tpu.User)
                .FirstOrDefault();

            if (projectExists == null)
            {
                throw new KeyNotFoundException($"User with ID {request.ProjectId} not found.");
            }

            var projects = await _context.Task_Projects
                .Where(tp => tp.Project_ID == request.ProjectId)
                .Include(tp => tp.Project)
                .Select(tp => tp.Project)
                .Distinct()
                .ToListAsync(cancellationToken);

            var projectTasks = new Dictionary<long, IEnumerable<Models.Task>>();
            var taskChanges = new Dictionary<long, IEnumerable<Models.TaskChanges>>();

            foreach (var project in projects)
            {
                var tasks = await _context.Task_Projects
                    .Where(tp => tp.Project_ID == project.Id && tp.Task_ID != null && tp.Creator != true)
                    .Include(tp => tp.Task)
                    .Select(tp => tp.Task)
                    .ToListAsync(cancellationToken);

                projectTasks[project.Id] = tasks;

                foreach (var task in tasks)
                {
                    var changes = await _context.TaskChanges
                        .Where(tc => tc.TaskID == task.Id)
                        .OrderBy(tc => tc.ChangedAt)
                        .ToListAsync(cancellationToken);

                    taskChanges[task.Id] = changes;
                }
            }

            var userName = $"{user.Name} {user.Surname}";
            var pdfContent = GenerateUserProjectsReport(userName, projects, projectTasks, taskChanges);

            return new Response
            {
                PdfContent = pdfContent,
                FileName = $"User_{userName}_Projects_Report_{DateTime.Now:yyyyMMdd}.pdf"
            };
        }
    }
}
