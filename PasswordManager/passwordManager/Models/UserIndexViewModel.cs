using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using passwordManager.Models;
using System.Security.Cryptography;
using System.Text;

namespace passwordManager.Models
{
    
    public class UserIndexViewModel
    {
        public int UserId { set; get; }
        public string UserAccount { set; get; }
        public string UserPassword { set; get; }
        public string MessageWrongAccount { set; get; }
        public string MessageWrongPassword { set; get; }

        public void InitialViewModel() {
            UserId = 0;
            //UserAccount = "";
            //UserPassword = "";
            MessageWrongAccount = "";
            MessageWrongPassword = "";
        }

        PWDBEntities db = new PWDBEntities();
        MemberUser dbUserInfo;

        public bool IsAccountValid()
        {
            if (string.IsNullOrEmpty(UserAccount))
            {
                MessageWrongAccount = "Account can not be empty!";
                return false;
            }

            var query = from o in db.MemberUsers
                        where o.AccountName == UserAccount
                        select o;
            try
            {
                dbUserInfo = query.Single();
                if (dbUserInfo == null)
                {
                    MessageWrongAccount = "Wromg Account!";
                    return false;
                }
            }
            catch (System.InvalidOperationException ex)
            {
                MessageWrongAccount = "Wromg Account!";
                return false;
            }
            if (dbUserInfo == null)
            {
                MessageWrongAccount = "Wromg Account!";
                return false;
            }

            return true;
        }


        public bool IsPasswordValid() {

            if (string.IsNullOrEmpty(UserPassword))
            {
                MessageWrongPassword = "Password can not be empty!";
                return false;
            }

            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] source = Encoding.Default.GetBytes(UserPassword);
            byte[] crypto = sha256.ComputeHash(source);
            string result = Convert.ToBase64String(crypto);

            if (result != dbUserInfo.AccountPassword)
            {
                MessageWrongPassword = "Wrong Password!";
                return false;
            }

            UserId = dbUserInfo.UserId;
            return true;
        }



    }
}