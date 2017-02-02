using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using ChinookSystem.BLL;
using Chinook.UI;
using Chinook.Data.Entities;
#endregion

public partial class SamplePages_CRUDReview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SelectedTitle.Text = "";
    }

    protected void Search_Click(object sender, EventArgs e)
    {
        //clear out the old album information on the Maintain tab
        Clear_Click(sender, e);

        if(string.IsNullOrEmpty(SearchArg.Text))
        {
            //(message string)
            MessageUserControl1.ShowInfo("Enter an album title or part of the title");
        }
        else
        {
            //do a look up of the data in the database via the controller
            //all actions that are external to the webpage should be done in a try/catch
            //  for friendly error handling
            //We will use MessageUserControl to handle the error messages for this semester
            MessageUserControl1.TryRun(()=>
            {
                //coding block I wish MessageUserControll to try and run checking for any errors, catching the errors,
                //and display said error(s) for me in its error panel.
                //what is leave for me to do: simply the logic for the event

                //standard lookup
                AlbumController sysmgr = new AlbumController();
                List<Album> albumlist = sysmgr.Albums_GetbyTitle(SearchArg.Text);
                if(albumlist.Count==0)
                {
                    //(title string, message string)
                    MessageUserControl1.ShowInfo("Search Results", "No data for album title or partial title" + SearchArg.Text);
                    AlbumList.DataSource = null;
                    AlbumList.DataBind();
                }
                else
                {
                    MessageUserControl1.ShowInfo("Search Results", "Select the desired album for maintenance");
                    AlbumList.DataSource = albumlist;
                    AlbumList.DataBind();
                }
            });
        }
    }

   

    protected void AlbumList_SelectedIndexChanged(object sender, EventArgs e)
    {
       

    }

    protected void AddAlbum_Click(object sender, EventArgs e)
    {
       
       
    }
    protected void UpdateAlbum_Click(object sender, EventArgs e)
    {
       
    }
    protected void DeleteAlbum_Click(object sender, EventArgs e)
    {
       
    }
    protected void Clear_Click(object sender, EventArgs e)
    {
        AlbumID.Text = "";
        AlbumTitle.Text = "";
        AlbumReleaseYear.Text = "";
        AlbumReleaseLabel.Text = "";
        ArtistList.SelectedIndex = 0;
    }

    protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
    {
        MessageUserControl.HandleDataBoundException(e);
    }

}