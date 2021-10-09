using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using SyncFrameworkApp.Controls.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIT.Data.Sync.Client;

namespace SyncFrameworkApp.Controls
{
    public partial class SfaDataTable
    {
        [Parameter]
        public OrmContext OrmContext { get; set; }
        public int DeltaCount { get; set; }
        
        async void RefreshData()
        {
            this.DeltaCount = await OrmContext.DeltaStore.GetDeltaCountAsync(await OrmContext.DeltaStore.GetLastPushedDeltaAsync(default), new System.Threading.CancellationToken());
           
            this.StateHasChanged();
        }
        async void Pull(MouseEventArgs args)
        {
            await OrmContext.PullAsync();
            RefreshData();

        }
        async void Push(MouseEventArgs args)
        {
            await OrmContext.PushAsync();
            RefreshData();
        }
        async void InitDatabase(MouseEventArgs args)
        {
           
            RefreshData();
        }

        [Parameter]
        public List<User> Users { get; set; }

        public RadzenDataGrid<User> grid;

        public RadzenDataGrid<UserContact> gridDetail;

        void RowRender(RowRenderEventArgs<User> args)
        {

        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if(firstRender)
                RefreshData();
        }

        #region CrudHeader
        void InsertRow()
        {
            var Instance = new User();
            this.OrmContext.Add(Instance);
            grid.InsertRow(Instance);
            RefreshData();
        }

        async Task SaveRow(User user)
        {
            await grid.UpdateRow(user);
            await this.OrmContext.SaveChangesAsync();
            RefreshData();
        }


        void EditRow(User user)
        {
            grid.EditRow(user);
        }

        


        void CancelEdit(User user)
        {
            grid.CancelEditRow(user);

        }

        void DeleteRow(User user)
        {
            if (Users.Contains(user))
            {
                Users.Remove(user);
                grid.Reload();
            }
            else
            {
                grid.CancelEditRow(user);
            }
        }

        #endregion


        void InsertDetailRow()
        {
            gridDetail.InsertRow(new UserContact());
        }

        void EditDetailRow(UserContact userContact)
        {

            gridDetail.EditRow(userContact);

        }

        void SaveDetailRow(UserContact userContact, Guid Id)
        {
            var user = Users.Where(a => a.Id == Id).FirstOrDefault();

            user.Contacts.Add(userContact);

            foreach (var userData in Users.ToList())
            {
                if (userData.Id == user.Id)
                {
                    userData.Contacts = user.Contacts;
                }

            }

            gridDetail.UpdateRow(userContact);

        }


        void DeleteDetailRow(UserContact userContact, Guid Id)
        {
            var user = Users.Where(a => a.Id == Id).FirstOrDefault();

            foreach (var userData in Users.ToList())
            {
                if (userData.Id == user.Id)
                {
                    userData.Contacts.Remove(userContact);
                }

            }
            gridDetail.Reload();
        }

        void CancelDetailEdit(UserContact userContact)
        {
            gridDetail.CancelEditRow(userContact);

        }
    }
}
