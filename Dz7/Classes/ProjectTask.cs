namespace dz8lat
{
 
        public class ProjectTask
        {
            public string Description { get; }
            public DateTime Deadline { get; }
            public Employee Initiator { get; }
            public Employee? Executor { get; private set; }
            public TaskStatus Status { get; private set; }

            public TaskReport? Report { get; private set; }
            public Project ParentProject { get; }

            public ProjectTask(string description, DateTime deadline, Employee initiator, Employee executor, Project project)
            {
                Description = description;
                Deadline = deadline;
                Initiator = initiator;
                Executor = executor;
                ParentProject = project;
                Status = TaskStatus.Assigned;
            }

            public void TakeInWork(Employee employee)
            {
                EnsureProjectInExecution();
                if (Executor != employee)
                    throw new InvalidOperationException("Взять задачу в работу может только назначенный исполнитель.");
                if (Status != TaskStatus.Assigned)
                    throw new InvalidOperationException("Задачу можно взять в работу только из статуса 'Назначена'.");

                Status = TaskStatus.InProgress;
                Console.WriteLine($"[Задача] '{Description}' взята в работу ({employee.Name}).");
            }


            public void Delegate(Employee currentEmployee, Employee newExecutor)
            {
                EnsureProjectInExecution();
                if (Executor != currentEmployee)
                    throw new InvalidOperationException("Делегировать задачу может только текущий исполнитель.");
                if (Status != TaskStatus.Assigned && Status != TaskStatus.InProgress)
                    throw new InvalidOperationException("Нельзя делегировать задачу на проверке или выполненную.");

                Executor = newExecutor;
                Status = TaskStatus.Assigned;
                Console.WriteLine($"[Задача] '{Description}' делегирована: {currentEmployee.Name} -> {newExecutor.Name}. Статус: Назначена.");
            }
            public void Reject(Employee currentEmployee)
            {
                EnsureProjectInExecution();
                if (Executor != currentEmployee)
                    throw new InvalidOperationException("Отклонить задачу может только текущий исполнитель.");

                Console.WriteLine($"[Задача] '{Description}' отклонена исполнителем {currentEmployee.Name}.");
                Executor = null;
                Status = TaskStatus.Assigned;
            }

            public void Reassign(Employee initiator, Employee newExecutor)
            {
                if (Initiator != initiator)
                    throw new InvalidOperationException("Переназначить задачу может только её инициатор.");
                if (Executor != null)
                    throw new InvalidOperationException("Задача уже имеет исполнителя.");

                Executor = newExecutor;
                Status = TaskStatus.Assigned;
                Console.WriteLine($"[Задача] '{Description}' переназначена тимлидом на {newExecutor.Name}.");
            }


            public void SubmitReport(Employee executor, string reportText)
            {
                EnsureProjectInExecution();
                if (Executor != executor)
                    throw new InvalidOperationException("Сдать отчёт может только текущий исполнитель.");
                if (Status != TaskStatus.InProgress)
                    throw new InvalidOperationException("Отчёт можно отправить только для задачи 'В работе'.");


                if (Report == null)
                {
                    Report = new TaskReport(reportText, executor);
                }
                else
                {
                    Report.Text = reportText;
                    Report.CompletionDate = DateTime.Now;
                }

                Status = TaskStatus.InReview;
                Console.WriteLine($"[Отчёт] {executor.Name} отправил отчёт по задаче '{Description}' на проверку.");
            }

            public void ApproveReport(Employee initiator)
            {
                if (Initiator != initiator)
                    throw new InvalidOperationException("Утвердить отчёт может только инициатор задачи.");
                if (Status != TaskStatus.InReview || Report == null)
                    throw new InvalidOperationException("Задача не находится на проверке или отсутствует отчёт.");

                Status = TaskStatus.Completed;
                Console.WriteLine($"[Отчёт] Инициатор {initiator.Name} УТВЕРДИЛ задачу '{Description}'. Статус: Выполнена.");

                ParentProject.CheckCompletion();
            }

            public void ReturnForRevision(Employee initiator, string comment)
            {
                if (Initiator != initiator)
                    throw new InvalidOperationException("Вернуть на доработку может только инициатор задачи.");
                if (Status != TaskStatus.InReview)
                    throw new InvalidOperationException("Вернуть можно только задачу со статусом 'На проверке'.");

                Status = TaskStatus.InProgress;
                Console.WriteLine($"[Отчёт] Инициатор {initiator.Name} ВЕРНУЛ задачу '{Description}' на доработку. Причина: {comment}");
            }

            private void EnsureProjectInExecution()
            {
                if (ParentProject.Status != ProjectStatus.Execution)
                    throw new InvalidOperationException("Действия с задачами доступны только когда проект в статусе 'Исполнение'.");
            }
        }

    }

