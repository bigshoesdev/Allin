using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.Data.SqlClient;

/// <summary>
/// Summary description for DBConnection
/// </summary>
public class DBManager
{
    public DBManager()
	{
		
	}

    string GetMConnectionString()
    {
        String connectionString =
               ConfigurationManager.ConnectionStrings["MConnectionString"].ConnectionString;
        return connectionString;
    }
    string GetPokerConnectionString()
    {
        String connectionString =
               ConfigurationManager.ConnectionStrings["PokerConnectionString"].ConnectionString;
        return connectionString;
    }
    string GetBadukiConnectionString()
    {
        String connectionString =
               ConfigurationManager.ConnectionStrings["BadukiConnectionString"].ConnectionString;
        return connectionString;
    }
    string GetMatgoConnectionString()
    {
        String connectionString =
               ConfigurationManager.ConnectionStrings["MatgoConnectionString"].ConnectionString;
        return connectionString;
    }

    string GetAllinConnectionString()
    {
        String connectionString =
               ConfigurationManager.ConnectionStrings["AllinConnectionString"].ConnectionString;
        return connectionString;
    }

    string GetAllinSocketString()
    {
        String connectionString =
               ConfigurationManager.ConnectionStrings["AllinSocketString"].ConnectionString;
        return connectionString;
    }


    public string GetMDBName()
    {
        String connectionString =
               ConfigurationManager.ConnectionStrings["MConnectionString"].ConnectionString;
        SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder(connectionString);
        return scsb.InitialCatalog;
    }

    public DataSet RunMSelectQuery(SqlCommand sqlQuery)
    {
        SqlConnection DBConnection = new SqlConnection(GetMConnectionString());

        SqlDataAdapter dbAdapter = new SqlDataAdapter();

        dbAdapter.SelectCommand = sqlQuery;

        sqlQuery.Connection = DBConnection;

        DataSet resultsDataSet = new DataSet();

        //try
        //{
            dbAdapter.Fill(resultsDataSet);
        //}
        //catch(Exception)
        //{
        //}
        return resultsDataSet;
    }
    public int RunMQuery(SqlCommand sqlQuery)
    {
        SqlConnection DBConnection = new SqlConnection(GetMConnectionString());

        sqlQuery.Connection = DBConnection;

        try
        {
            DBConnection.Open();
            return sqlQuery.ExecuteNonQuery();
        }
        finally
        {
            if (DBConnection.State == ConnectionState.Open)
            {
                DBConnection.Close();
            }
        }
        return -1;
    }

    public DataSet RunPokerSelectQuery(SqlCommand sqlQuery)
    {
        SqlConnection DBConnection = new SqlConnection(GetPokerConnectionString());

        SqlDataAdapter dbAdapter = new SqlDataAdapter();

        dbAdapter.SelectCommand = sqlQuery;

        sqlQuery.Connection = DBConnection;

        DataSet resultsDataSet = new DataSet();

        try
        {
            dbAdapter.Fill(resultsDataSet);
        }
        catch(Exception)
        {
        }
        return resultsDataSet;
    }
    public int RunPokerQuery(SqlCommand sqlQuery)
    {
        SqlConnection DBConnection = new SqlConnection(GetPokerConnectionString());

        sqlQuery.Connection = DBConnection;

        try
        {
            DBConnection.Open();
            return sqlQuery.ExecuteNonQuery();
        }
        finally
        {
            if (DBConnection.State == ConnectionState.Open)
            {
                DBConnection.Close();
            }
        }
        return -1;
    }

    public DataSet RunAllinSelectQuery(SqlCommand sqlQuery)
    {
        SqlConnection DBConnection = new SqlConnection(GetAllinConnectionString());

        SqlDataAdapter dbAdapter = new SqlDataAdapter();

        dbAdapter.SelectCommand = sqlQuery;

        sqlQuery.Connection = DBConnection;

        DataSet resultsDataSet = new DataSet();

        try
        {
            dbAdapter.Fill(resultsDataSet);
        }
        catch (Exception)
        {
        }
        return resultsDataSet;
    }

    public int RunAllinQuery(SqlCommand sqlQuery)
    {
        SqlConnection DBConnection = new SqlConnection(GetAllinConnectionString());

        sqlQuery.Connection = DBConnection;

        try
        {
            DBConnection.Open();
            return sqlQuery.ExecuteNonQuery();
        }
        finally
        {
            if (DBConnection.State == ConnectionState.Open)
            {
                DBConnection.Close();
            }
        }
        return -1;
    }

    public DataSet RunBadukiSelectQuery(SqlCommand sqlQuery)
    {
        SqlConnection DBConnection = new SqlConnection(GetBadukiConnectionString());

        SqlDataAdapter dbAdapter = new SqlDataAdapter();

        dbAdapter.SelectCommand = sqlQuery;

        sqlQuery.Connection = DBConnection;

        DataSet resultsDataSet = new DataSet();

        //try
        {
            dbAdapter.Fill(resultsDataSet);
        }
        //catch(Exception)
        {
        }
        return resultsDataSet;
    }
    public int RunBadukiQuery(SqlCommand sqlQuery)
    {
        SqlConnection DBConnection = new SqlConnection(GetBadukiConnectionString());

        sqlQuery.Connection = DBConnection;

        try
        {
            DBConnection.Open();
            return sqlQuery.ExecuteNonQuery();
        }
        finally
        {
            if (DBConnection.State == ConnectionState.Open)
            {
                DBConnection.Close();
            }
        }
        return -1;
    }

    public DataSet RunMatgoSelectQuery(SqlCommand sqlQuery)
    {
        SqlConnection DBConnection = new SqlConnection(GetMatgoConnectionString());

        SqlDataAdapter dbAdapter = new SqlDataAdapter();

        dbAdapter.SelectCommand = sqlQuery;

        sqlQuery.Connection = DBConnection;

        DataSet resultsDataSet = new DataSet();

        try
        {
            dbAdapter.Fill(resultsDataSet);
        }
        catch(Exception)
        {
        }
        return resultsDataSet;
    }
    public int RunMatgoQuery(SqlCommand sqlQuery)
    {
        SqlConnection DBConnection = new SqlConnection(GetMatgoConnectionString());

        sqlQuery.Connection = DBConnection;

        try
        {
            DBConnection.Open();
            return sqlQuery.ExecuteNonQuery();
        }
        finally
        {
            if (DBConnection.State == ConnectionState.Open)
            {
                DBConnection.Close();
            }
        }
        return -1;
    }
}
