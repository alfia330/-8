//проект
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManager
{
    enum ProjectStatus { Проект, Исполнение, Закрыт }
    enum TaskStatus { Назначена, ВРаботе, НаПроверке, Выполнена }

    class Person
    {
        public string Name;
        public Person(string name) { Name = name; }
    }

    class Report
    {
        public string Text; public DateTime Date; public Person Executor;
        public Report(string text, Person executor) { Text = text; Date = DateTime.Now; Executor = executor; }
    }

    class TaskItem
    {
        public string Description; public DateTime Deadline;
        public Person Initiator; public Person Executor;
        public TaskStatus Status = TaskStatus.Назначена;
        public Report Report; 
        public Project Project;

        public void TakeInWork(Person p)
        {
            if (Status != TaskStatus.Назначена || p != Executor) throw new Exception("Взять в работу может только исполнитель назначенной задачи.");
            Status = TaskStatus.ВРаботе;
        }

        public void DelegateTo(Person p, Person newExecutor)
        {
            if (Status != TaskStatus.Назначена || p != Executor) throw new Exception("Делегировать можно только свою назначенную задачу.");
            Executor = newExecutor; 
        }

        public void Reject(Person p)
        {
            if (Status != TaskStatus.Назначена || p != Executor) throw new Exception("Отклонить можно только свою назначенную задачу.");
            Executor = null; 
        }

        public void Assign(Person lead, Person executor)
        {
            if (lead != Initiator || Executor != null) throw new Exception("Назначить задачу может только тимлид.");
            Executor = executor;
        }

        public void SubmitReport(Person p, string text)
        {
            if (Status != TaskStatus.ВРаботе || p != Executor) throw new Exception("Отчёт сдаёт исполнитель задачи в работе.");
            if (Report != null) throw new Exception("Отчёт по задаче уже создан.");
            Report = new Report(text, p);
            Status = TaskStatus.НаПроверке;
        }

        public void ApproveReport(Person initiator)
        {
            if (Status != TaskStatus.НаПроверке || initiator != Initiator) throw new Exception("Утвердить отчёт может только инициатор.");
            Status = TaskStatus.Выполнена;
        }

        public void ReturnForRework(Person initiator)
        {
            if (Status != TaskStatus.НаПроверке || initiator != Initiator) throw new Exception("Вернуть на доработку может только инициатор.");
            Status = TaskStatus.ВРаботе;
        }
    }

    class Project
    {
        public string Description; public DateTime Deadline;
        public Person Initiator; public Person TeamLead;
        public List<TaskItem> Tasks = new List<TaskItem>();
        public ProjectStatus Status = ProjectStatus.Проект;

        public Project(string description, DateTime deadline, Person initiator, Person teamLead)
        { Description = description; Deadline = deadline; Initiator = initiator; TeamLead = teamLead; }

        public TaskItem CreateTask(string desc, DateTime deadline, Person initiator, Person executor)
        {
            if (Status != ProjectStatus.Проект) throw new Exception("Задачи создаются только в статусе «Проект».");
            if (initiator != TeamLead) throw new Exception("Задачи назначает только тимлид.");
            var t = new TaskItem { Description = desc, Deadline = deadline, Initiator = initiator, Executor = executor, Project = this };
            Tasks.Add(t);
            return t;
        }

        public void RemoveTask(TaskItem t)
        {
            if (t.Executor != null) throw new Exception("Удалить можно только отклонённую задачу (без исполнителя).");
            Tasks.Remove(t);
        }

        public void StartExecution()
        {
            if (Status != ProjectStatus.Проект) throw new Exception("Перевести в исполнение можно только проект.");
            Status = ProjectStatus.Исполнение;
        }

        public void TryClose()
        {
            if (Status == ProjectStatus.Исполнение && Tasks.Count > 0 && Tasks.All(t => t.Status == TaskStatus.Выполнена))
                Status = ProjectStatus.Закрыт;
        }
    }

    class Program
    {
        static void Main()
        {
            var team = new List<Person>();
            for (int i = 1; i <= 10; i++) team.Add(new Person("Сотрудник" + i));

            var customer = new Person("Заказчик ООО Ромашка");
            var lead = team[0];

            var project = new Project("Разработка CRM", DateTime.Now.AddDays(30), customer, lead);

            
            for (int i = 0; i < 10; i++)
                project.CreateTask("Задача " + (i + 1), DateTime.Now.AddDays(5 + i), lead, team[i]);

            project.StartExecution(); 

            foreach (var t in project.Tasks) t.TakeInWork(t.Executor);

            foreach (var t in project.Tasks) t.SubmitReport(t.Executor, "Отчёт по \"" + t.Description + "\": работа выполнена.");

        
            foreach (var t in project.Tasks) t.ApproveReport(t.Initiator);

            project.TryClose(); 

            Console.WriteLine("Проект: " + project.Description + " | Статус: " + project.Status);
            foreach (var t in project.Tasks)
                Console.WriteLine($"{t.Description} | исполнитель: {t.Executor.Name} | статус: {t.Status} | отчёт: \"{t.Report.Text}\" ({t.Report.Date:d})");
        }
    }
}
