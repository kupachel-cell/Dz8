namespace dz8lat
{
    
        public class Project
        {
            public string Description { get; }
            public DateTime Deadline { get; }
            public Employee Customer { get; }
            public Employee TeamLead { get; }
            public ProjectStatus Status { get; private set; }

            private readonly List<ProjectTask> _tasks = new List<ProjectTask>();
            public IReadOnlyList<ProjectTask> Tasks => _tasks.AsReadOnly();

            public Project(string description, DateTime deadline, Employee customer, Employee teamLead)
            {
                Description = description;
                Deadline = deadline;
                Customer = customer;
                TeamLead = teamLead;
                Status = ProjectStatus.Project;
                Console.WriteLine($"=== Создан проект: '{Description}' (Статус: Проект) ===");
            }

            public ProjectTask AddTask(Employee creator, string description, DateTime deadline, Employee executor)
            {
                if (creator != TeamLead)
                    throw new UnauthorizedAccessException("Создавать задачи по проекту может только ответственный тимлид.");
                if (Status != ProjectStatus.Project)
                    throw new InvalidOperationException("Создавать задачи можно только когда проект в статусе 'Проект'.");

                var task = new ProjectTask(description, deadline, TeamLead, executor, this);
                _tasks.Add(task);
                Console.WriteLine($"[Планирование] Добавлена задача '{description}' -> Исполнитель: {executor.Name}");
                return task;
            }

            public void RemoveTask(Employee initiator, ProjectTask task)
            {
                if (initiator != TeamLead)
                    throw new UnauthorizedAccessException("Удалять задачи может только тимлид.");
                if (task.Executor != null)
                    throw new InvalidOperationException("Удалить можно только отклоненную задачу без исполнителя.");

                _tasks.Remove(task);
                Console.WriteLine($"[Проект] Задача '{task.Description}' удалена из проекта.");
            }

            public void StartExecution(Employee initiator)
            {
                if (initiator != TeamLead)
                    throw new UnauthorizedAccessException("Запустить проект может только тимлид.");
                if (_tasks.Count == 0)
                    throw new InvalidOperationException("Нельзя запустить проект без задач.");
                if (Status != ProjectStatus.Project)
                    throw new InvalidOperationException("Проект уже запущен или закрыт.");

                Status = ProjectStatus.Execution;
                Console.WriteLine($"\n=== Проект '{Description}' переведен в статус 'Исполнение' ===\n");
            }

            public void CheckCompletion()
            {
                if (Status == ProjectStatus.Execution && _tasks.All(t => t.Status == TaskStatus.Completed))
                {
                    Status = ProjectStatus.Closed;
                    Console.WriteLine($"\n=== ВСЕ ЗАДАЧИ ВЫПОЛНЕНЫ! Проект '{Description}' переведен в статус 'Закрыт' ===");
                }
            }
        }
    }
