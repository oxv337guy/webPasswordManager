using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace passwordManager.Models {
    public class CreateAccountViewModel {
        public string UserFirstName { set; get; }
        public string UserLastName { set; get; }
        public string UserEmail { set; get; }
        public string AccountName { set; get; }
        public string AccountPassword { set; get; }
        public string AccountPassword2 { set; get; }
        public string EncryptPassword { set; get; }

        public string ErrorMessageAccountNameExist { set; get; }
        public string ErrorMessagePasswordDiff { set; get; }
        public string ErrorMessageEmailFormat { set; get; }

        PWDBEntities db = new PWDBEntities();

        public bool IsAccountNameExist() {
            var query = from o in db.MemberUsers
                        select o.AccountName;
            List<string> AccountNameList = query.ToList();
            bool result = AccountNameList.Exists(x => x == AccountName);
            if (result) {
                ErrorMessageAccountNameExist = "Account is exist! Change please.";
            }
            return result;
        }

        public bool IsInputPwIdentity() {
            bool result = (AccountPassword == AccountPassword2) ? true : false;
            if (!result) { ErrorMessagePasswordDiff = "Typing Passwords are difference!"; }
            return result;
        }

        public void EncryptPw() {
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] source = Encoding.Default.GetBytes(AccountPassword);
            byte[] crypto = sha256.ComputeHash(source);
            EncryptPassword = Convert.ToBase64String(crypto);
        }

    }
}