using SchoolMS.Common;
using SchoolMS.Model;
using SchoolMS.Model.Common;
using SchoolMS.Repository.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMS.Repository
{
    public class StudentRepository : IStudentRepository
    {
        public static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SchoolMS;Integrated Security=True";

        public async Task<List<StudentModelDTO>> GetAllStudents(Paging paging, Sorting sorting, StudentFilter studentFilter)
        {
            try
            {

                SqlConnection connection = new SqlConnection(connectionString);
                using (connection)
                {   StringBuilder queryString = new StringBuilder();

                    SqlCommand commmand = new SqlCommand();
                    queryString.AppendLine("select * from Student where 1=1");           
                    if (studentFilter != null)
                    {
                        if (studentFilter.SchoolId != Guid.Empty)
                        {
                            queryString.AppendLine("and SchoolId = @schoolId ");
                            commmand.Parameters.AddWithValue("@schoolId", studentFilter.SchoolId);

                        }
                        if(studentFilter.Name != null)
                        {
                            queryString.AppendLine("and FirstName like '%'+@name+'%' or LastName like '%'+@name+'%' ");
                            commmand.Parameters.AddWithValue("@name", studentFilter.Name);
                        }
                        if(studentFilter.AverageFrom != null)
                        {
                            queryString.AppendLine("and Average >= @averageFrom ");
                            commmand.Parameters.AddWithValue("@averageFrom", studentFilter.AverageFrom);
                        }
                        if (studentFilter.AverageTo != null)
                        {
                            queryString.AppendLine("and Average <= @averageTo ");
                            commmand.Parameters.AddWithValue("@averageTo", studentFilter.AverageTo);
                        }
                        if (studentFilter.Average != null)
                        {
                            queryString.AppendLine("and Average = @average ");
                            commmand.Parameters.AddWithValue("@average", studentFilter.Average);
                        }
                        if(studentFilter.DOBFrom != null)
                        {
                            if(studentFilter.DOBTo != null)
                            {
                                queryString.AppendLine("and DOB between @dobFrom and @dobTo ");                               
                            }
                            queryString.AppendLine("and DOB between @dobFrom and '1-1-2999' ");
                            commmand.Parameters.AddWithValue("@dobFrom", studentFilter.DOBFrom);
                        }
                        if (studentFilter.DOBTo != null)
                        {
                            queryString.AppendLine("and DOB between '1-1-1900' and @dobTo ");
                            commmand.Parameters.AddWithValue("@dobTo", studentFilter.DOBTo);
                        }
                    }
                    if (sorting != null)
                    {
                        queryString.AppendLine($"order by {sorting.OrderBy} {sorting.SortOrder} ");
                    }
                    if(paging != null)
                    {
                        queryString.AppendLine("offset (@pageNumber - 1) * @pageSize rows fetch next @pageSize rows only" );
                        commmand.Parameters.AddWithValue("@pageNumber", paging.PageNumber);
                        commmand.Parameters.AddWithValue("@pageSize", paging.PageSize);
                    }
                    commmand.Connection = connection;
                    commmand.CommandText = queryString.ToString();

                    connection.Open();
                    SqlDataReader reader = await commmand.ExecuteReaderAsync();

                    List<StudentModelDTO> students = new List<StudentModelDTO>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentModelDTO student = new StudentModelDTO();
                            student.Id = reader.GetGuid(0);
                            student.FirstName = reader.GetString(1);
                            student.LastName = reader.GetString(2);
                            student.DOB = reader.GetDateTime(3);
                            student.Address = reader.GetString(4);
                            student.SchoolId = reader.GetGuid(5);
                            student.Average = reader.IsDBNull(6) ? student.Average : reader.GetDecimal(6);

                            students.Add(student);
                        }
                        reader.Close();
                        return students;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<StudentModelDTO> GetStudent(Guid id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                using (connection)
                {
                    SqlCommand command = new SqlCommand("select * from Student where Id=@id", connection);
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    StudentModelDTO student = new StudentModelDTO();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        student.Id = reader.GetGuid(0);
                        student.FirstName = reader.GetString(1);
                        student.LastName = reader.GetString(2);
                        student.DOB = reader.GetDateTime(3);
                        student.Address = reader.GetString(4);
                        student.SchoolId = reader.GetGuid(5);
                        student.Average = reader.IsDBNull(6) ? student.Average : reader.GetDecimal(6);

                        reader.Close();
                        return student;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> AddNewStudent(StudentModelDTO student)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                using (connection)
                {
                    SqlCommand command = new SqlCommand("insert into Student values(@id, @firstName, @lastName, @dob, @address, @schoolId, @average)", connection);
                    command.Parameters.AddWithValue("@id", Guid.NewGuid());
                    command.Parameters.AddWithValue("@firstName", student.FirstName);
                    command.Parameters.AddWithValue("@lastName", student.LastName);
                    command.Parameters.AddWithValue("@dob", student.DOB);
                    command.Parameters.AddWithValue("@address", student.Address);
                    command.Parameters.AddWithValue("@schoolId", student.SchoolId);
                    command.Parameters.AddWithValue("@average", (decimal?)student.Average ?? Convert.DBNull  );

                    connection.Open();
                    int numberOfAffectedRows = await command.ExecuteNonQueryAsync();
                    if (numberOfAffectedRows > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EditStudent(Guid id, StudentModelDTO student)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                using (connection)
                {

                    SqlCommand commandEdit = new SqlCommand("update Student set FirstName=@firstName, LastName=@lastName, DOB=@dob, Address=@address, SchoolId=@schoolId, Average=@average where Id=@id", connection);
                    commandEdit.Parameters.AddWithValue("@id", id);
                    commandEdit.Parameters.AddWithValue("@firstName", student.FirstName);
                    commandEdit.Parameters.AddWithValue("lastName", student.LastName);
                    commandEdit.Parameters.AddWithValue("@dob", student.DOB);
                    commandEdit.Parameters.AddWithValue("@address", student.Address);
                    commandEdit.Parameters.AddWithValue("@schoolId", student.SchoolId);
                    commandEdit.Parameters.AddWithValue("@average", (decimal)student.Average == default ? Convert.DBNull : (decimal)student.Average );

                    connection.Open();
                    int numberOfAffectedRows = await commandEdit.ExecuteNonQueryAsync();
                    if (numberOfAffectedRows > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }     
        }

        public async Task<bool> DeleteStudent(Guid id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                using (connection)
                {
                    SqlCommand command = new SqlCommand("delete from Student where Id=@id", connection);
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    int numberOfAffectedRows = await command.ExecuteNonQueryAsync();
                    if (numberOfAffectedRows > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
