using System;
using System.Collections.Generic;
using Npgsql;

namespace PracticalWork2._3
{
    // содержит методы для отправки запросов
    public static class DatabaseRequests
    {
        // отправляет запрос на добавление пользователя
        public static void AddUserQuery(string id, string login, string password)
        {
            var querySql = $"insert into \"ToDo\".\"User\"(id, login, password) values ({id}, '{login}', '{password}')";
            using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
            cmd.ExecuteNonQuery();
        }

        // отправляет запрос на получение всех id пользователей для проверки
        public static List<string> CheckIdQuery()
        {
            var querySql = $"SELECT id FROM \"ToDo\".\"User\"";
            using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
            using var reader = cmd.ExecuteReader();

            List<string> id = new List<string>();

            while (reader.Read())
            {
                id.Add(reader[0].ToString());
            }

            return id;
        }

        // отправляет запрос на удаление пользователя
        public static void DeleteUserQuery(string id)
        {
            var querySql = $"delete from \"ToDo\".\"Task\" where \"user ID\" = {id};" +
                           $"delete from \"ToDo\".\"User\" where id = {id};";
            using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
            cmd.ExecuteNonQuery();
        }

        // отправляет запрос на получение id пользователя
        public static string GetUserQuery(string login, string password)
        {
            var querySql = $"select id from \"ToDo\".\"User\" where login = '{login}' and password = '{password}'";
            using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
            using var reader = cmd.ExecuteReader();

            string userId = "";

            while (reader.Read())
            {
                userId = reader[0].ToString();
            }

            return userId;
        }

        // отправляет запрос на добавление новой задачи
        public static void AddTaskQuery(string userId, string title, string description, DateTime dueDate)
        {
            var querySql =
                $"INSERT INTO \"ToDo\".\"Task\"(\"user ID\", title, description, creationdate, duedate) VALUES ('{userId}', '{title}', '{description}', current_date, '{dueDate}')";
            using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
            cmd.ExecuteNonQuery();
        }

        // отправляет запрос на удаление задачи
        public static void DeleteTaskQuery(string id, string userId)
        {
            var querySql =
                $"DELETE FROM \"ToDo\".\"Task\" WHERE id = '{id}' and \"user ID\" = {userId}";
            using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
            cmd.ExecuteNonQuery();
        }

        // отправляет запрос на редактирование задачи
        public static void EditTaskQuery(string id, string title, string description, DateTime duedate)
        {
            var querySql =
                $"update \"ToDo\".\"Task\" set title = '{title}', description = '{description}', duedate = '{duedate}' where id = {id}";
            using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
            cmd.ExecuteNonQuery();
        }

        // отправляет запрос на получение списка задач на указанную дату
        public static void GetTasksByDate(string userId, DateTime date)
        {
            var querySql =
                $"SELECT id, title, description, creationdate, duedate FROM \"ToDo\".\"Task\" where \"user ID\" = {userId} and duedate = '{date}'";
            using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(
                    $"ID: {reader[0]}\tНазвание: {reader[1]}\tОписание: {reader[2]}\tДата создания: {reader[3]}\tДата окончания: {reader[4]}");
            }
        }

        // отправляет запрос на получение списка всех задач
        public static void GetTasksQuery(string userId)
        {
            var querySql =
                $"SELECT id, title, description, creationdate, duedate FROM \"ToDo\".\"Task\" where \"user ID\" = {userId}";
            using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(
                    $"ID: {reader[0]}\tНазвание: {reader[1]}\tОписание: {reader[2]}\tДата создания: {reader[3]}\tДата окончания: {reader[4]}");
            }
        }

        // отправляет запрос на получение списка предстоящих задач
        public static void GetUpcomingTasksQuery(string userId)
        {
            var querySql =
                $"SELECT id, title, description, creationdate, duedate FROM \"ToDo\".\"Task\" where \"user ID\" = {userId} and duedate > current_date";
            using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(
                    $"ID: {reader[0]}\tНазвание: {reader[1]}\tОписание: {reader[2]}\tДата создания: {reader[3]}\tДата окончания: {reader[4]}");
            }
        }

        // отправляет запрос на получение списка выполненных задач
        public static void GetCompletedTasksQuery(string userId)
        {
            var querySql =
                $"SELECT id, title, description, creationdate, duedate FROM \"ToDo\".\"Task\" where \"user ID\" = {userId} and duedate < current_date";
            using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(
                    $"ID: {reader[0]}\tНазвание: {reader[1]}\tОписание: {reader[2]}\tДата создания: {reader[3]}\tДата окончания: {reader[4]}");
            }
        }
    }
}