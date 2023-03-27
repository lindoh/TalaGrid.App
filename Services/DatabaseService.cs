using TalaGrid.Models;
using Microsoft.Data.SqlClient;
//using Microsoft.Maui.Animations;
using System.Data;



//    TO DO:
// Formalize SqlExceptions to match what caused the error
// Add Comments

namespace TalaGrid.Services
{
    public class DatabaseService
    {
        #region Class Properties
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;
        AlertService alerts;
        #endregion

        #region Default Constructor
        public DatabaseService()
        {
            try
            {
                //SQL Authentication Connection String
                 string DataConnection = "Server=tcp:farecost-server.database.windows.net,1433;Initial Catalog=GreenWayAfrica_DB;" +
                    "Persist Security Info=False;User ID=CloudSAb2b69ffa;Password=P@55Code;MultipleActiveResultSets=False;" +
                    "Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                

               /* 
                * string DataConnection = "Server=LAPTOP-J3M5FNUA;Database=GreenWayData;User ID=AdminSql;Password=P@55Code"; 
                */

                sqlConnection = new SqlConnection(DataConnection);
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
            }
            catch (StackOverflowException ex)
            {

                alerts.ShowAlertAsync("Exception Error", ex.ToString());
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            alerts = new AlertService();

        }

        #endregion

        #region SaveData Method for Collector
        /// <summary>
        /// Save Collector's Data in the Collector's Database Table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool SaveData(Users user)
        {
            bool isSaved = false;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "AddCollector";

                sqlCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", user.LastName);
                sqlCommand.Parameters.AddWithValue("@IdNumber", user.IdNumber);
                sqlCommand.Parameters.AddWithValue("@Gender", user.Gender);
                sqlCommand.Parameters.AddWithValue("@HighestQlfn", user.HighestQlfn); ;
                sqlCommand.Parameters.AddWithValue("@IncomeRange", user.IncomeRange);
                sqlCommand.Parameters.AddWithValue("@Email", user.Email);
                sqlCommand.Parameters.AddWithValue("@CellNumber", user.CellNumber);
                sqlCommand.Parameters.AddWithValue("@StreetAddress", user.StreetAddress);
                sqlCommand.Parameters.AddWithValue("@Suburb", user.Suburb);
                sqlCommand.Parameters.AddWithValue("@City", user.City);
                sqlCommand.Parameters.AddWithValue("@Province", user.Province);
                sqlCommand.Parameters.AddWithValue("@Country", user.Country);

                //Open Sql database connection
                sqlConnection.Open();

                //If affected number of rows is > 0, then data is saved successfully
                int NoOfRowsAffected = sqlCommand.ExecuteNonQuery();
                isSaved = NoOfRowsAffected > 0;
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }


            return isSaved;
        }

        #endregion

        #region SaveData Method for Admin
        /// <summary>
        /// Save Collector's Data in the Collector's Database Table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool SaveAdminData(Users user)
        {
            bool isSaved = false;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "AddAdmin";

                sqlCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", user.LastName);
                sqlCommand.Parameters.AddWithValue("@IdNumber", user.IdNumber);
                sqlCommand.Parameters.AddWithValue("@Gender", user.Gender);
                sqlCommand.Parameters.AddWithValue("@HighestQlfn", user.HighestQlfn); ;
                sqlCommand.Parameters.AddWithValue("@IncomeRange", user.IncomeRange);
                sqlCommand.Parameters.AddWithValue("@Email", user.Email);
                sqlCommand.Parameters.AddWithValue("@CellNumber", user.CellNumber);
                sqlCommand.Parameters.AddWithValue("@StreetAddress", user.StreetAddress);
                sqlCommand.Parameters.AddWithValue("@Suburb", user.Suburb);
                sqlCommand.Parameters.AddWithValue("@City", user.City);
                sqlCommand.Parameters.AddWithValue("@Province", user.Province);
                sqlCommand.Parameters.AddWithValue("@Country", user.Country);

                //Open Sql database connection
                sqlConnection.Open();

                //If affected number of rows is > 0, then data is saved successfully
                int NoOfRowsAffected = sqlCommand.ExecuteNonQuery();
                isSaved = NoOfRowsAffected > 0;
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }


            return isSaved;
        }

        #endregion

        #region SaveData Method for Admin Logins
        /// <summary>
        /// A Method to Save Admin Login Details
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public bool SaveLogins(Login login)
        {
            bool isSaved = false;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "AddAdminLogins";

                sqlCommand.Parameters.AddWithValue("@Username", login.Username);
                sqlCommand.Parameters.AddWithValue("@Password", login.Password);
                sqlCommand.Parameters.AddWithValue("@AdminId", login.AdminId);

                //Open Sql Connection
                sqlConnection.Open();

                //If affected number of rows is > 0, then data is saved successfully
                int NoOfRowsAffected = sqlCommand.ExecuteNonQuery();
                isSaved = NoOfRowsAffected > 0;
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return isSaved;
        }
        #endregion

        #region Search Admin Logins in the Database
        /// <summary>
        /// Search the Admin Login details to find a match of
        /// Username and Password and Read the Data including 
        /// the AdminId
        /// </summary>
        /// <param name="UserLogin"></param>
        /// <returns></returns>
        public void SearchLogins(Login UserLogin)
        {

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "SearchAdminLogins";

                sqlCommand.Parameters.AddWithValue("@Username", UserLogin.Username);
                sqlCommand.Parameters.AddWithValue("@Password", UserLogin.Password);

                //Open Sql Connection
                sqlConnection.Open();

                var sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    UserLogin.IsLoggedIn = true;

                    while (sqlDataReader.Read())
                    {
                        UserLogin.Username = sqlDataReader.GetString(1);
                        UserLogin.Password = sqlDataReader.GetString(2);
                        UserLogin.AdminId = sqlDataReader.GetInt32(3);
                    }
                    sqlDataReader.Close();
                }
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        #endregion

        #region Search Admin Username to Avoid Duplications
        public int SearchAdminUsername(string username)
        {
            int id = 0;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "SearchAdminUsername";

                sqlCommand.Parameters.AddWithValue("@Username", username);

                //Open Sql Connection
                sqlConnection.Open();

                var sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {

                    while (sqlDataReader.Read())
                    {
                        id = sqlDataReader.GetInt32(0);
                    }
                    sqlDataReader.Close();
                }
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return id;
        }

        #endregion

        #region GetAll User's Data 
        /// <summary>
        /// Get all data that matches a given name, i.e., Firstname
        /// </summary>
        /// <returns>List of users that matches Firstname</returns>
        public List<Users> Search(string name, string selectedUser)
        {
            List<Users> usersList = new();
            string userToSearch = "SearchCollector";

            //Update the userToSearch variable to select the correct 
            //Stored Procedure for the search
            if (selectedUser == "Admin")
                userToSearch = "SearchAdmin";
            else if (selectedUser == "Collector")
                userToSearch = "SearchCollector";

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = userToSearch;
                sqlCommand.Parameters.AddWithValue("@FirstName", name);

                sqlConnection.Open();
                var sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    Users user;

                    while (sqlDataReader.Read())
                    {
                        user = new Users();
                        user.Id = sqlDataReader.GetInt32(0);
                        user.FirstName = sqlDataReader.GetString(1);
                        user.LastName = sqlDataReader.GetString(2);
                        user.IdNumber = sqlDataReader.GetString(3);
                        user.Gender = sqlDataReader.GetString(4);
                        user.HighestQlfn = sqlDataReader.GetString(5);
                        user.IncomeRange = sqlDataReader.GetString(6);
                        user.Email = sqlDataReader.GetString(7);
                        user.CellNumber = sqlDataReader.GetString(8);
                        user.StreetAddress = sqlDataReader.GetString(9);
                        user.Suburb = sqlDataReader.GetString(10);
                        user.City = sqlDataReader.GetString(11);
                        user.Province = sqlDataReader.GetString(12);
                        user.Country = sqlDataReader.GetString(13);

                        usersList.Add(user);
                    }
                    sqlDataReader.Close();

                }
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return usersList;
        }
        #endregion

        #region Search AdminId using IdNumber
        public Users SearchAdmin(string IdNumber)
        {
            Users user = new Users();

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "SearchAdminWithIdNum";
                sqlCommand.Parameters.AddWithValue("@IdNumber", IdNumber);

                sqlConnection.Open();
                var sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        user.Id = sqlDataReader.GetInt32(0);
                        user.FirstName = sqlDataReader.GetString(1);
                        user.LastName = sqlDataReader.GetString(2);
                        user.Email = sqlDataReader.GetString(3);
                    }
                    sqlDataReader.Close();

                }
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return user;
        }

        #endregion

        #region Update User's Data
        public bool Update(Users user, string selectedUser)
        {
            bool isUpdated = false;
            string userToUpdate = "UpdateCollector";
            string userId = "@CollectorId";

            //Update the userToUpdate variable to select the correct 
            //Stored Procedure for the updatec function
            if (selectedUser == "Admin")
            {
                userToUpdate = "UpdateAdmin";
                userId = "@AdminId";
            }
            else if (selectedUser == "Collector")
            {
                userToUpdate = "UpdateCollector";
                userId = "@CollectorId";
            }

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = userToUpdate;

                sqlCommand.Parameters.AddWithValue(userId, user.Id);
                sqlCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", user.LastName);
                sqlCommand.Parameters.AddWithValue("@IdNumber", user.IdNumber);
                sqlCommand.Parameters.AddWithValue("@Gender", user.Gender);
                sqlCommand.Parameters.AddWithValue("@HighestQlfn", user.HighestQlfn); ;
                sqlCommand.Parameters.AddWithValue("@IncomeRange", user.IncomeRange);
                sqlCommand.Parameters.AddWithValue("@Email", user.Email);
                sqlCommand.Parameters.AddWithValue("@CellNumber", user.CellNumber);
                sqlCommand.Parameters.AddWithValue("@StreetAddress", user.StreetAddress);
                sqlCommand.Parameters.AddWithValue("@Suburb", user.Suburb);
                sqlCommand.Parameters.AddWithValue("@City", user.City);
                sqlCommand.Parameters.AddWithValue("@Province", user.Province);
                sqlCommand.Parameters.AddWithValue("@Country", user.Country);

                //Open Sql database connection
                sqlConnection.Open();

                //If affected number of rows is > 0, then data is updated successfully
                int NoOfRowsAffected = sqlCommand.ExecuteNonQuery();
                isUpdated = NoOfRowsAffected > 0;
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return isUpdated;
        }

        #endregion

        #region Update Admin Password
        public bool UpdatePassword(int AdminId, string Password)
        {
            bool isUpdated = false;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "UpdatePassword";

                sqlCommand.Parameters.AddWithValue("@AdminId", AdminId);
                sqlCommand.Parameters.AddWithValue("@Password", Password);

                //Open Sql database connection
                sqlConnection.Open();

                //If affected number of rows is > 0, then data is updated successfully
                int NoOfRowsAffected = sqlCommand.ExecuteNonQuery();
                isUpdated = NoOfRowsAffected > 0;
            }
            catch (SqlException ex)
            {
                alerts.ShowAlert("Error", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return isUpdated;
        }

        #endregion

        #region Delete User's Data
        public bool Delete(int id, string selectedUser)
        {
            bool isDeleted = false;
            string userToDelete = "DeleteCollector";
            string userId = "@CollectorId";

            //Update the userToUpdate variable to select the correct 
            //Stored Procedure for the updatec function
            if (selectedUser == "Admin")
            {
                userToDelete = "DeleteAdmin";
                userId = "@AdminId";
            }
            else if (selectedUser == "Collector")
            {
                userToDelete = "DeleteCollector";
                userId = "@CollectorId";
            }

            try
            {
                sqlCommand.Parameters.Clear();      //Clear Parameters
                sqlCommand.CommandText = userToDelete;

                sqlCommand.Parameters.AddWithValue(userId, id);

                //Open Sql database connection
                sqlConnection.Open();

                //If number of rows affected > 0 then the data is deleted succesfully
                int noOfRowsAffected = sqlCommand.ExecuteNonQuery();
                isDeleted = noOfRowsAffected > 0;
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return isDeleted;
        }

        #endregion

        #region Insert and Update User Banking Details

        /// <summary>
        /// The method searches for the Collector's banking details
        /// Using the provided CollectorId
        /// </summary>
        /// <param name="CollectorId"></param>
        /// <returns>A List with the User's banking details</returns>
        public Banking SearchBanking(int CollectorId)
        {
            Banking banker = new Banking();

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "SearchBankingDetails";
                sqlCommand.Parameters.AddWithValue("@CollectorId", CollectorId);

                sqlConnection.Open();
                var sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {

                    while (sqlDataReader.Read())
                    {
                        banker.BankDetailsId = sqlDataReader.GetInt32(0);
                        banker.BankName = sqlDataReader.GetString(1);
                        banker.BranchName = sqlDataReader.GetString(2);
                        banker.BranchCode = sqlDataReader.GetString(3);
                        banker.AccountType = sqlDataReader.GetString(4);
                        banker.AccountNumber = sqlDataReader.GetString(5);
                    }
                    sqlDataReader.Close();
                }
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return banker;
        }

        /// <summary>
        /// To save the banking the details of the Collector 
        /// for the first time
        /// </summary>
        /// <param name="banker"></param>
        /// <param name="user"></param>
        /// <returns>Return True if successful, else False</returns>
        public bool NewBankingDetails(Banking banker, Users user)
        {
            bool isSaved = false;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "NewBankingDetails";

                sqlCommand.Parameters.AddWithValue("@BankName", banker.BankName);
                sqlCommand.Parameters.AddWithValue("@BranchName", banker.BranchName);
                sqlCommand.Parameters.AddWithValue("@BranchCode", banker.BranchCode);
                sqlCommand.Parameters.AddWithValue("@AccType", banker.AccountType);
                sqlCommand.Parameters.AddWithValue("@AccNumber", banker.AccountNumber);
                sqlCommand.Parameters.AddWithValue("@CollectorId", user.Id);

                //Open Sql connection
                sqlConnection.Open();

                //If number of rows affected is > 0, data is saved successfully
                int NoOfRowsAffected = sqlCommand.ExecuteNonQuery();
                isSaved = NoOfRowsAffected > 0;

            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return isSaved;
        }

        /// <summary>
        /// To update existing Collector's banking details
        /// </summary>
        /// <param name="banker"></param>
        /// <returns>Return true if successful, else false</returns>
        public bool UpdateBankingDetails(Banking banker)
        {
            bool isUpdated = false;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "UpdateBankingDetails";

                sqlCommand.Parameters.AddWithValue("@BankDetailsId", banker.BankDetailsId);
                sqlCommand.Parameters.AddWithValue("@BankName", banker.BankName);
                sqlCommand.Parameters.AddWithValue("@BranchName", banker.BranchName);
                sqlCommand.Parameters.AddWithValue("@BranchCode", banker.BranchCode);
                sqlCommand.Parameters.AddWithValue("@AccType", banker.AccountType);
                sqlCommand.Parameters.AddWithValue("@AccNumber", banker.AccountNumber);

                //Open Sql connection
                sqlConnection.Open();

                //If the number of rows affected > 0, then the data is succesfully updated
                int NoOfRowsAffected = sqlCommand.ExecuteNonQuery();
                isUpdated = NoOfRowsAffected > 0;
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return isUpdated;
        }

        #endregion

        #region Get The List of Bottles
        public List<BottleDataSource> GetBottleList()
        {
            List<BottleDataSource> bottleList = new List<BottleDataSource>();

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "GetBottles";

                sqlConnection.Open();

                var sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    BottleDataSource bottle;

                    while (sqlDataReader.Read())
                    {
                        bottle = new BottleDataSource();

                        bottle.BottleDataSourceId = sqlDataReader.GetInt32(0);
                        bottle.BottleName = sqlDataReader.GetString(1);
                        bottle.Size = sqlDataReader.GetString(2);

                        bottleList.Add(bottle);

                    }
                    sqlDataReader.Close();
                }
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }


            return bottleList;
        }

        #endregion

        #region Get The List of Other Waste Material
        public List<WasteMaterial> GetOtherWasteList()
        {
            List<WasteMaterial> otherWasteList = new List<WasteMaterial>();

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "GetOtherWasteList";

                sqlConnection.Open();

                var sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    WasteMaterial wasteMaterial;

                    while (sqlDataReader.Read())
                    {
                        wasteMaterial = new WasteMaterial();

                        wasteMaterial.MaterialName = sqlDataReader.GetString(1);
                        otherWasteList.Add(wasteMaterial);
                    }
                    sqlDataReader.Close();
                }
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }


            return otherWasteList;
        }

        #endregion

        #region Save BuyBackCentre Details
        /// <summary>
        /// Save the Data for the BuyBackCentre with the Associated Admin details
        /// Admin ID is sufficient in this case
        /// </summary>
        /// <param name="buyBackCentre"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool SaveBBCData(BuyBackCentre buyBackCentre, Users user)
        {
            bool isSaved = false;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "NewBBCDetails";

                sqlCommand.Parameters.AddWithValue("@BBCName", buyBackCentre.BuyBackCentreName);
                sqlCommand.Parameters.AddWithValue("@StreetAddress", buyBackCentre.StreetAddress);
                sqlCommand.Parameters.AddWithValue("@Suburb", buyBackCentre.Suburb);
                sqlCommand.Parameters.AddWithValue("@City", buyBackCentre.City);
                sqlCommand.Parameters.AddWithValue("@Province", buyBackCentre.Province);
                sqlCommand.Parameters.AddWithValue("@Country", buyBackCentre.Country);
                sqlCommand.Parameters.AddWithValue("@AdminId", user.Id);

                //Open Sql database connection
                sqlConnection.Open();

                //If affected number of rows is > 0, then data is saved successfully
                int NoOfRowsAffected = sqlCommand.ExecuteNonQuery();
                isSaved = NoOfRowsAffected > 0;

            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }


            return isSaved;
        }

        #endregion

        #region Search BuyBackCentre Details
        public BuyBackCentre SearchBBC(int AdminId)
        {
            BuyBackCentre buyBackCentre = new BuyBackCentre();

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "SearchBBC";
                sqlCommand.Parameters.AddWithValue("@AdminId", AdminId);

                sqlConnection.Open();
                var sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        buyBackCentre.BBCId = sqlDataReader.GetInt32(0);
                        buyBackCentre.BuyBackCentreName = sqlDataReader.GetString(1);
                        buyBackCentre.AdminId = sqlDataReader.GetInt32(2);
                        buyBackCentre.StreetAddress = sqlDataReader.GetString(3);
                        buyBackCentre.Suburb = sqlDataReader.GetString(4);
                        buyBackCentre.City = sqlDataReader.GetString(5);
                        buyBackCentre.Province = sqlDataReader.GetString(6);
                        buyBackCentre.Country = sqlDataReader.GetString(7);
                    }
                    sqlDataReader.Close();
                }
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return buyBackCentre;
        }

        #endregion

        #region Update BuyBackCentre Details
        public bool UpdateBBC(BuyBackCentre buyBackCentre)
        {
            bool isUpdated = false;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "UpdateBBCDetails";

                sqlCommand.Parameters.AddWithValue("@BBCId", buyBackCentre.BBCId);
                sqlCommand.Parameters.AddWithValue("@BBCName", buyBackCentre.BuyBackCentreName);
                sqlCommand.Parameters.AddWithValue("@StreetAddress", buyBackCentre.StreetAddress);
                sqlCommand.Parameters.AddWithValue("@Suburb", buyBackCentre.Suburb);
                sqlCommand.Parameters.AddWithValue("@City", buyBackCentre.City);
                sqlCommand.Parameters.AddWithValue("@Province", buyBackCentre.Province);
                sqlCommand.Parameters.AddWithValue("@Country", buyBackCentre.Country);

                //Open Sql database connection
                sqlConnection.Open();

                //If affected number of rows is > 0, then data is updated successfully
                int NoOfRowsAffected = sqlCommand.ExecuteNonQuery();
                isUpdated = NoOfRowsAffected > 0;
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return isUpdated;
        }

        #endregion

        #region Save Captured Bottle Data
        public bool CaptureBottles(Bottles bottle)
        {
            bool isSaved = false;

            try
            {
                //Open Sql connection
                sqlConnection.Open();

                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "CaptureBottles";

                sqlCommand.Parameters.AddWithValue("Quantity", bottle.Quantity);
                sqlCommand.Parameters.AddWithValue("BottleDataSourceId", bottle.BottleDataSourceId);
                sqlCommand.Parameters.AddWithValue("CollectorId", bottle.CollectorId);
                sqlCommand.Parameters.AddWithValue("BBCId", bottle.BBCId);
                sqlCommand.Parameters.AddWithValue("Amount", bottle.Amount);
                sqlCommand.Parameters.AddWithValue("AdminId", bottle.AdminId);

                //If affected number of rows > 0, then the save operation is successful
                int NoOfRowsAffected = sqlCommand.ExecuteNonQuery();
                isSaved = NoOfRowsAffected > 0;

            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return isSaved;
        }
        #endregion

        #region Save Captured Other Waste Material Data
        public bool CaptureOtherWaste(OtherWaste otherWaste)
        {
            bool isSaved = false;

            try
            {
                //Open Sql connection
                sqlConnection.Open();

                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "CaptureOtherWaste";

                sqlCommand.Parameters.AddWithValue("MaterialName", otherWaste.MaterialName);
                sqlCommand.Parameters.AddWithValue("Size", otherWaste.Size);
                sqlCommand.Parameters.AddWithValue("Price", otherWaste.Price);
                sqlCommand.Parameters.AddWithValue("CollectorId", otherWaste.CollectorId);
                sqlCommand.Parameters.AddWithValue("BBCId", otherWaste.BBCId);
                sqlCommand.Parameters.AddWithValue("Amount", otherWaste.Amount);
                sqlCommand.Parameters.AddWithValue("AdminId", otherWaste.AdminId);

                //If affected number of rows > 0, then the save operation is successful
                int NoOfRowsAffected = sqlCommand.ExecuteNonQuery();
                isSaved = NoOfRowsAffected > 0;

            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return isSaved;
        }
        #endregion

        #region Get Bottle Id
        public int GetBottleId(Bottles bottle)
        {
            int id = 0;

            try
            {

                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "GetBottleId";

                sqlCommand.Parameters.AddWithValue("Quantity", bottle.Quantity);
                sqlCommand.Parameters.AddWithValue("BottleDataSourceId", bottle.BottleDataSourceId);
                sqlCommand.Parameters.AddWithValue("CollectorId", bottle.CollectorId);
                sqlCommand.Parameters.AddWithValue("BBCId", bottle.BBCId);
                sqlCommand.Parameters.AddWithValue("Amount", bottle.Amount);
                sqlCommand.Parameters.AddWithValue("AdminId", bottle.AdminId);

                sqlConnection.Open();

                var sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        id = sqlDataReader.GetInt32(0);
                    }
                }
                sqlDataReader.Close();


            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return id;
        }
        #endregion

        #region Get Other Waste Id
        public int GetOtherWasteId(OtherWaste otherWaste)
        {
            int id = 0;

            try
            {

                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "GetOtherWasteId";

                sqlCommand.Parameters.AddWithValue("MaterialName", otherWaste.MaterialName);
                sqlCommand.Parameters.AddWithValue("Size", otherWaste.Size);
                sqlCommand.Parameters.AddWithValue("Price", otherWaste.Price);
                sqlCommand.Parameters.AddWithValue("CollectorId", otherWaste.CollectorId);
                sqlCommand.Parameters.AddWithValue("BBCId", otherWaste.BBCId);
                sqlCommand.Parameters.AddWithValue("Amount", otherWaste.Amount);
                sqlCommand.Parameters.AddWithValue("AdminId", otherWaste.AdminId);

                sqlConnection.Open();

                var sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        id = sqlDataReader.GetInt32(0);
                    }
                }
                sqlDataReader.Close();


            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return id;
        }

        #endregion

        #region Save Transaction Record
        public bool TransRecord(Transaction transaction)
        {
            bool isSaved = false;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "TransRecord";

                sqlCommand.Parameters.AddWithValue("TransactionType", transaction.TransactionType);
                sqlCommand.Parameters.AddWithValue("TransactionDateTime", transaction.LocalDate);
                sqlCommand.Parameters.AddWithValue("WasteMaterialId", transaction.WasteMaterialId);
                sqlCommand.Parameters.AddWithValue("BankDetailsId", transaction.BankDetailsId);
                sqlCommand.Parameters.AddWithValue("Signature", transaction.Signature);
                sqlCommand.Parameters.AddWithValue("WasteMaterialType", transaction.WasteMaterialType);
                sqlCommand.Parameters.AddWithValue("AdminId", transaction.AdminId);
                sqlCommand.Parameters.AddWithValue("CollectorId", transaction.CollectorId);



                sqlConnection.Open();

                //If affected number of rows > 0, then the save operation is successful
                int NoOfRowsAffected = sqlCommand.ExecuteNonQuery();
                isSaved = NoOfRowsAffected > 0;
            }
            catch (SqlException ex)
            {

                alerts.ShowAlert("Error!", ex.Message);
            }
            catch (System.ArgumentException ex)
            {
                alerts.ShowAlert("Error!", ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return isSaved;
        }

        #endregion



    }
}
