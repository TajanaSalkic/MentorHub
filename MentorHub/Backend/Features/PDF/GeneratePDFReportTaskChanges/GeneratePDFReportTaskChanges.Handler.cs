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
            List<TaskChangesDTO> taskChanges)
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
                    projectTable.AddCell("ID");
                    projectTable.AddCell(project.Id.ToString());

                    projectTable.AddCell("Description");
                    projectTable.AddCell(project.Description);

                    projectTable.AddCell("Start Date");
                    projectTable.AddCell(project.StartDate.ToString("yyyy-MM-dd"));

                    projectTable.AddCell("End Date");
                    projectTable.AddCell(project.EndDate.ToString("yyyy-MM-dd"));

                    projectTable.AddCell("Status");
                    projectTable.AddCell(project.Status.ToString());

                    projectTable.AddCell("Points");
                    projectTable.AddCell(project.Points.ToString());

                    document.Add(projectTable);
                    document.Add(new Paragraph("\n"));

                    if (projectTasks.ContainsKey(project.Id) && projectTasks[project.Id].Any())
                    {
                        document.Add(new Paragraph("Tasks:").SetFontSize(14));

                        Table taskTable = new Table(5).UseAllAvailableWidth();
                        taskTable.AddHeaderCell("Task ID");
                        taskTable.AddHeaderCell("Title");
                        taskTable.AddHeaderCell("Status");
                        taskTable.AddHeaderCell("Points");
                        taskTable.AddHeaderCell("Deadline");

                        foreach (var task in projectTasks[project.Id])
                        {
                            taskTable.AddCell(task.Id.ToString());
                            taskTable.AddCell(task.Title);
                            taskTable.AddCell(task.Status.ToString());
                            taskTable.AddCell(task.Points.ToString());
                            taskTable.AddCell(task.EndDate.ToString("yyyy-MM-dd"));
                        }

                        document.Add(taskTable);
                        document.Add(new Paragraph("\n"));

                        var taskIds = projectTasks[project.Id].Select(t => t.Id).ToHashSet();
                        var changesForProject = taskChanges
                            .Where(c => taskIds.Contains(c.TaskID))
                            .OrderBy(c => c.ChangedAt)
                            .ToList();

                        if (changesForProject.Any())
                        {
                            document.Add(new Paragraph("Task Change History:").SetFontSize(14));

                            Table changeTable = new Table(8).UseAllAvailableWidth();
                            changeTable.AddHeaderCell("ChangeID");
                            changeTable.AddHeaderCell("Task ID");
                            changeTable.AddHeaderCell("Task Title");
                            changeTable.AddHeaderCell("Date");
                            changeTable.AddHeaderCell("Changed Field");
                            changeTable.AddHeaderCell("Old Value");
                            changeTable.AddHeaderCell("New Value");
                            changeTable.AddHeaderCell("Changed By");

                            foreach (var change in changesForProject)
                            {
                                changeTable.AddCell(change.ChangeID.ToString());
                                changeTable.AddCell(change.TaskID.ToString());
                                changeTable.AddCell(change.Title);
                                changeTable.AddCell(change.ChangedAt.ToString("yyyy-MM-dd HH:mm"));
                                changeTable.AddCell(change.FieldChanged);
                                changeTable.AddCell(change.OldValue);
                                changeTable.AddCell(change.NewValue);
                                changeTable.AddCell($"{change.Name} {change.Surname}");
                            }

                            document.Add(changeTable);
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

            if (projectExists == null)
            {
                throw new KeyNotFoundException($"Project with ID {request.ProjectId} not found.");
            }

            var user = projectExists.TaskProjectUsers
                .Where(tpu => tpu.Project_ID == request.ProjectId && tpu.Creator == false)
                .Select(tpu => tpu.User)
                .FirstOrDefault();

            var projects = await _context.Task_Projects
                .Where(tp => tp.Project_ID == request.ProjectId)
                .Include(tp => tp.Project)
                .Select(tp => tp.Project)
                .Distinct()
                .ToListAsync(cancellationToken);

            var projectTasks = new Dictionary<long, IEnumerable<Models.Task>>();
            foreach (var project in projects)
            {
                var tasks = await _context.Task_Projects
                    .Where(tp => tp.Project_ID == project.Id && tp.Task_ID != null && tp.Creator != true)
                    .Include(tp => tp.Task)
                    .Select(tp => tp.Task)
                    .ToListAsync(cancellationToken);

                projectTasks[project.Id] = tasks;
            }

            var taskChanges = await _context.TaskChanges
                .Where(tc => _context.Task_Projects
                    .Where(tp => tp.Project_ID == request.ProjectId)
                    .Select(tp => tp.Task_ID)
                    .Contains(tc.TaskID))
                .OrderBy(tc => tc.ChangedAt)
                .Select(tc => new TaskChangesDTO
                {
                    ChangeID = tc.ChangeID,
                    UserID = tc.UserID,
                    Name = tc.User.Name,
                    Surname = tc.User.Surname,
                    TaskID = tc.TaskID,
                    Title = tc.Task.Title,
                    ChangedAt = tc.ChangedAt,
                    FieldChanged = tc.FieldChanged,
                    OldValue = tc.OldValue,
                    NewValue = tc.NewValue,
                })
                .ToListAsync(cancellationToken);

            var userName = $"{user?.Name} {user?.Surname}";
            var pdfContent = GenerateUserProjectsReport(userName, projects, projectTasks, taskChanges);

            return new Response
            {
                PdfContent = pdfContent,
                FileName = $"User_{userName}_Projects_Report_{DateTime.Now:yyyyMMdd}.pdf"
            };
        }
    }

}
