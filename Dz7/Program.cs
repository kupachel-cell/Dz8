using System;
using System.Collections.Generic;

namespace dz8lat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Ex1Test();
        }

        static void Ex1Test()
        {
            var customer = new Employee("Александр (Заказчик)", "Product Owner");
            var teamLead = new Employee("Елена (Тимлид)", "Team Lead");

            var team = new List<Employee>
            {
                new Employee("Иван", "Backend Developer"),
                new Employee("Мария", "Frontend Developer"),
                new Employee("Дмитрий", "QA Engineer"),
                new Employee("Анна", "UI/UX Designer"),
                new Employee("Сергей", "DevOps"),
                new Employee("Ольга", "System Analyst"),
                new Employee("Павел", "Backend Developer"),
                new Employee("Наталья", "Frontend Developer"),
                new Employee("Артем", "Mobile Developer"),
                new Employee("Виктория", "QA Automation")
            };

            var project = new Project("Разработка корпоративного портала", DateTime.Now.AddMonths(2), customer, teamLead);

            var tasks = new List<ProjectTask>();
            for (int i = 0; i < team.Count; i++)
            {
                var task = project.AddTask(
                    teamLead,
                    $"Модуль #{i + 1} ({team[i].Role})",
                    DateTime.Now.AddDays(14),
                    team[i]
                );
                tasks.Add(task);
            }

           
            var extraTask = project.AddTask(teamLead, "Интеграция с устаревшим API", DateTime.Now.AddDays(5), team[2]);


            project.StartExecution(teamLead);

            tasks[0].Delegate(team[0], team[6]);

            extraTask.Reject(team[2]);
            project.RemoveTask(teamLead, extraTask);

            tasks[1].TakeInWork(team[1]);
            tasks[1].SubmitReport(team[1], "Сверстала главную страницу.");
            tasks[1].ReturnForRevision(teamLead, "Не адаптировано под мобильные экраны.");
            tasks[1].SubmitReport(team[1], "Добавила адаптивную верстку для смартфонов.");
            tasks[1].ApproveReport(teamLead);

            for (int i = 0; i < tasks.Count; i++)
            {
                if (i == 1)
                {
                    continue; 
                }

                Employee currentExecutor = tasks[i].Executor!;
                tasks[i].TakeInWork(currentExecutor);
                tasks[i].SubmitReport(currentExecutor, $"Работа по '{tasks[i].Description}' полностью завершена.");
                tasks[i].ApproveReport(teamLead);
            }

            int completedCount = 0;
            foreach (ProjectTask task in project.Tasks)
            {
                if (task.Status == TaskStatus.Completed)
                {
                    completedCount++;
                }
            }

            Console.WriteLine($"\nИтоговый статус проекта: {project.Status}");
            Console.WriteLine($"Всего выполнено задач: {completedCount} из {project.Tasks.Count}");
        }
    }
}
