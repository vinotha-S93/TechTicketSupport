using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TECHSUPPORTTICKET_MANAGEMENTSYSTEM.Models;

namespace TECHSUPPORTTICKET_MANAGEMENTSYSTEM.Repository
{
    public class TicketRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Devconnection"].ConnectionString;

        public List<Ticket> GetAllTickets()
        {
            List<Ticket> list = new List<Ticket>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetAllTickets", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        list.Add(new Ticket
                        {
                            TicketID = Convert.ToInt32(rdr["TicketID"]),
                            CustomerID = Convert.ToInt32(rdr["CustomerID"]),
                            Subject = rdr["Subject"].ToString(),
                            Description = rdr["Description"].ToString(),
                            Priority = rdr["Priority"].ToString(),
                            Status = rdr["Status"].ToString(),
                            AssignedTo = rdr["AssignedTo"] != DBNull.Value ? Convert.ToInt32(rdr["AssignedTo"]) : (int?)null,
                            CreatedAt = Convert.ToDateTime(rdr["CreatedAt"]),
                            UpdatedAt = rdr["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(rdr["UpdatedAt"]) : (DateTime?)null
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetAllTickets error: " + ex.Message);
            }

            return list;
        }

        public bool InsertTicket(Ticket ticket)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertTicket", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CustomerID", ticket.CustomerID);
                    cmd.Parameters.AddWithValue("@Subject", ticket.Subject ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description", ticket.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Priority", ticket.Priority ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", ticket.Status ?? "Open");
                    cmd.Parameters.AddWithValue("@AssignedTo", DBNull.Value);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    System.Diagnostics.Debug.WriteLine("Rows affected: " + rows);
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("InsertTicket error: " + ex.Message);
                return false;
            }
        }
    }
}
