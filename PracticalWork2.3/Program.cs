using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PracticalWork2._3
{
    class Program
    {
        private static string userId = String.Empty;

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Выберите действие");
                Console.WriteLine("0. Выйти из программы.");
                Console.WriteLine("1. Авторизация.");
                Console.WriteLine("2. Регистрация.");
                Console.WriteLine("3. Удалить пользователя.");
                Console.WriteLine("4. Добавить задачу.");
                Console.WriteLine("5. Удалить задачу.");
                Console.WriteLine("6. Редактировать задачу.");
                Console.WriteLine("7. Задачи на сегодня.");
                Console.WriteLine("8. Задачи на завтра.");
                Console.WriteLine("9. Задачи на неделю.");
                Console.WriteLine("10. Список всех задач.");
                Console.WriteLine("11. Список предстоящих задач.");
                Console.WriteLine("12. Список выполненных задач.");

                int choice = GetChoice();

                switch (choice)
                {
                    case 0:
                    {
                        return;
                    }
                    case 1:
                    {
                        Authorization();
                        break;
                    }
                    case 2:
                    {
                        Registration();
                        break;
                    }
                    case 3:
                    {
                        DeleteUser();
                        break;
                    }
                    case 4:
                    {
                        AddTask();
                        break;
                    }
                    case 5:
                    {
                        RemoveTask();
                        break;
                    }
                    case 6:
                    {
                        EditTask();
                        break;
                    }
                    case 7:
                    {
                        ViewTasksByDate(DateTime.Today);
                        break;
                    }
                    case 8:
                    {
                        ViewTasksByDate(DateTime.Today.AddDays(1));
                        break;
                    }
                    case 9:
                    {
                        ViewTasksByWeek(DateTime.Today.AddDays(7));
                        break;
                    }
                    case 10:
                    {
                        ViewAllTasks();
                        break;
                    }
                    case 11:
                    {
                        ViewUpComingTasks();
                        break;
                    }
                    case 12:
                    {
                        ViewCompletedTasks();
                        break;
                    }
                    default:
                    {
                        Console.WriteLine("Некорректный ввод. Попробуйте снова.\n");
                        break;
                    }
                }
            }
        }

        private static int GetChoice()
        {
            while (true)
            {
                Console.Write("Действие: ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    return choice;
                }

                Console.WriteLine("Некорректный ввод. Попробуйте снова.\n");
            }
        }

        private static void Authorization()
        {
            Console.Write("\nВведите логин: ");
            string loginInput = Console.ReadLine();

            Console.Write("Введите пароль: ");
            string passwordInput = Console.ReadLine();

            string id = DatabaseRequests.GetUserQuery(loginInput, passwordInput);

            if (!string.IsNullOrEmpty(id))
            {
                Console.WriteLine("Авторизация прошла успешно.\n");
                userId = id;
            }
            else
            {
                Console.WriteLine("Неверно введён логин или пароль.\n");
            }
        }

        private static void Registration()
        {
            Console.Write("\nВведите ID: ");
            string idInput = Console.ReadLine();

            List<string> id = DatabaseRequests.CheckIdQuery();

            if (id.Contains(idInput))
            {
                Console.WriteLine("Данный ID занят.\n");
            }
            else
            {
                Console.Write("Введите логин: ");
                string loginInput = Console.ReadLine();

                Console.Write("Введите пароль: ");
                string passwordInput = Console.ReadLine();

                if (string.IsNullOrEmpty(idInput) || string.IsNullOrEmpty(loginInput) ||
                    string.IsNullOrEmpty(passwordInput))
                {
                    Console.WriteLine("Данные введены некорректно.\n");
                }
                else
                {
                    DatabaseRequests.AddUserQuery(idInput, loginInput, passwordInput);

                    Console.WriteLine("Регистрация прошла успешно.\n");
                }
            }
        }

        private static void DeleteUser()
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("Вы уверены, что хотите удалить текущего пользователя?");
                Console.WriteLine("Введите 1 если ДА...");
                string input = Console.ReadLine();
                if (input == "1")
                {
                    DatabaseRequests.DeleteUserQuery(userId);
                    Console.WriteLine("Пользователь успешно удалён.\n");
                    userId = String.Empty;
                }
                else
                {
                    Console.WriteLine("Пользователь не был удалён.\n");
                }
            }
            else
            {
                Console.WriteLine("Перед выполнением действий необходимо авторизоваться.");
                Authorization();
            }
        }

        private static void AddTask()
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("Введите название задачи:");
                string title = Console.ReadLine();

                Console.WriteLine("Введите описание задачи:");
                string description = Console.ReadLine();

                Console.WriteLine("Введите дату завершения (формат: ГГГГ-ММ-ДД):");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime dueDate))
                {
                    DatabaseRequests.AddTaskQuery(userId, title, description, dueDate);

                    Console.WriteLine("Задача успешно добавлена.\n");
                }
                else
                {
                    Console.WriteLine("Некорректная дата. Задача не добавлена.\n");
                }
            }
            else
            {
                Console.WriteLine("Перед выполнением действий необходимо авторизоваться.");
                Authorization();
            }
        }

        private static void RemoveTask()
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Console.Write("Введите номер задачи для удаления: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    DatabaseRequests.DeleteTaskQuery(id.ToString(), userId);

                    Console.WriteLine("Задача успешно удалена.\n");
                }
                else
                {
                    Console.WriteLine("Некорректный ввод.\n");
                }
            }
            else
            {
                Console.WriteLine("Перед выполнением действий необходимо авторизоваться.");
                Authorization();
            }
        }

        private static void EditTask()
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Console.Write("Введите номер задачи для редактирования: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.Write("Введите новое название задачи: ");
                    string title = Console.ReadLine();

                    Console.WriteLine("Введите новое описание задачи:");
                    string description = Console.ReadLine();

                    Console.Write("Введите новую дату завершения задачи (формат: ГГГГ-ММ-ДД): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime duedate))
                    {
                        DatabaseRequests.EditTaskQuery(id.ToString(), title, description, duedate);

                        Console.WriteLine("Задача отредактирована успешно.\n");
                    }
                    else
                    {
                        Console.WriteLine("Некорректная дата.\n");
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный ввод.\n");
                }
            }
            else
            {
                Console.WriteLine("Перед выполнением действий необходимо авторизоваться.");
                Authorization();
            }
        }

        private static void ViewTasksByDate(DateTime date)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("\n##############################################################################\n");

                Console.WriteLine($"Задачи на {date.ToShortDateString()}:");

                DatabaseRequests.GetTasksByDate(userId, date);

                Console.WriteLine("\n##############################################################################\n");
            }
            else
            {
                Console.WriteLine("Перед выполнением действий необходимо авторизоваться.");
                Authorization();
            }
        }

        private static void ViewTasksByWeek(DateTime date)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("\n##############################################################################\n");

                Console.WriteLine("Задачи на неделю:");

                DatabaseRequests.GetTasksByDate(userId, date);

                Console.WriteLine("\n##############################################################################\n");
            }
            else
            {
                Console.WriteLine("Перед выполнением действий необходимо авторизоваться.");
                Authorization();
            }
        }

        private static void ViewAllTasks()
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("\n##############################################################################\n");

                Console.WriteLine("Все задачи:");

                DatabaseRequests.GetTasksQuery(userId);

                Console.WriteLine("\n##############################################################################\n");
            }
            else
            {
                Console.WriteLine("Перед выполнением действий необходимо авторизоваться.");
                Authorization();
            }
        }

        private static void ViewUpComingTasks()
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("\n##############################################################################\n");

                Console.WriteLine("Предстоящие задачи:");

                DatabaseRequests.GetUpcomingTasksQuery(userId);

                Console.WriteLine("\n##############################################################################\n");
            }
            else
            {
                Console.WriteLine("Перед выполнением действий необходимо авторизоваться.");
                Authorization();
            }
        }

        private static void ViewCompletedTasks()
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("\n##############################################################################\n");

                Console.WriteLine("Выполненные задачи:");

                DatabaseRequests.GetCompletedTasksQuery(userId);

                Console.WriteLine("\n##############################################################################\n");
            }
            else
            {
                Console.WriteLine("Перед выполнением действий необходимо авторизоваться.");
                Authorization();
            }
        }
    }
}