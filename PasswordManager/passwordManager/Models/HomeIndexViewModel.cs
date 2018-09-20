using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using passwordManager.Models;

namespace passwordManager.Models {
    public class HomeIndexViewModel {

        public int UserId { set; get; }
        public int ShowItemNumber { set; get; }
        public int ItemStart { set; get; }
        public int NewPwCount { set; get; }
        public int PageCount { set; get; }
        //public List<ShowList> ShowNewPassword = new List<ShowList> { };
        public List<ShowList> ShowNewPassword;

        PWDBEntities db = new PWDBEntities();

        public void ComputPageCount() {
            var query = from o in db.NewPasswords
                        where o.UserId == UserId
                        select o.NewPasswordId;
            List<int> idList = query.ToList();
            PageCount = (idList.Count % ShowItemNumber == 0) ? 
                (idList.Count / ShowItemNumber) : 
                (idList.Count / ShowItemNumber + 1);
        }



        public void PickUpNewPassword() {
            var query = from o in db.NewPasswords
                        where o.UserId == UserId
                        select o;
            List<NewPassword> npwList = query.ToList();
            NewPwCount = npwList.Count;

            ShowNewPassword = new List<ShowList> {};

            for (int i = ItemStart; i< (ItemStart+ShowItemNumber); i++) {
                if (i >= NewPwCount) break;

                NewPassword npw = npwList[i];

                ShowList obj = new ShowList();

                obj.CountId = i+1;
                obj.NewPasswordId = npw.NewPasswordId;
                obj.UserId = npw.UserId;
                obj.SourceName = npw.SourceName;
                obj.SourceAccount = npw.SourceAccount;
                obj.PasswordText = npw.PasswordText;
                obj.StateText = npw.StateText;

                ShowNewPassword.Add(obj);
            }
        }
    }

    public class ShowList {
        public int CountId { set; get; }
        public int NewPasswordId { set; get; }
        public int UserId { set; get; }
        public string SourceName { set; get; }
        public string SourceAccount { set; get; }
        public string PasswordText { set; get; }
        public string StateText { set; get; }
    }
}