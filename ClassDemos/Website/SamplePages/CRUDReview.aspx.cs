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
        //coming from tab Find
        GridViewRow agvrow = AlbumList.Rows[AlbumList.SelectedIndex];
        string albumid = (agvrow.FindControl("AlbumID") as Label).Text;
        string albumtitle = (agvrow.FindControl("Title") as Label).Text;
        string albumyear = (agvrow.FindControl("Year") as Label).Text;
        string albumlabel = (agvrow.FindControl("AlbumLabel") as Label).Text;
        string artistid = (agvrow.FindControl("ArtistID") as Label).Text;

        //displayint on tab find
        SelectedTitle.Text = albumtitle + "release in " + albumyear + " by " + albumlabel;
        //filling controls on tab Maintain
        AlbumID.Text = albumid;
        AlbumTitle.Text = albumtitle;
        ArtistList.SelectedValue = artistid;
        AlbumReleaseYear.Text = albumyear;
        AlbumReleaseLabel.Text = albumlabel;


    }

    protected void AddAlbum_Click(object sender, EventArgs e)
    {
        //retest the validation of the incoming data via the Validation Controls
       if (IsValid)
        {
            //any other business rules
            MessageUserControl2.TryRun(() =>
            {
                AlbumController sysmgr = new AlbumController();
                Album newalbum = new Album();
                newalbum.Title = AlbumTitle.Text;
                newalbum.ArtistId = int.Parse(ArtistList.SelectedValue);
                newalbum.ReleaseYear = int.Parse(AlbumReleaseYear.Text);
                newalbum.ReleaseLabel = string.IsNullOrEmpty(AlbumReleaseLabel.Text) ? null : AlbumReleaseYear.Text;

                sysmgr.Albums_Add(newalbum);
            },"Add Album", "Album has been successfully added to the database.");
        }
       
    }
    protected void UpdateAlbum_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            //any other business rules
            if (string.IsNullOrEmpty(AlbumID.Text))
            {
                MessageUserControl2.ShowInfo("Missing Data", "Missing Album Id. Use Find to locate the album you wish to maintain.");
            }
            else
            {
                int albumid = 0;
                if(int.TryParse(AlbumID.Text, out albumid))
                {
                    MessageUserControl2.TryRun(() =>
                    {
                        AlbumController sysmgr = new AlbumController();
                        Album newalbum = new Album();
                        newalbum.AlbumId = albumid;
                        newalbum.Title = AlbumTitle.Text;
                        newalbum.ArtistId = int.Parse(ArtistList.SelectedValue);
                        newalbum.ReleaseYear = int.Parse(AlbumReleaseYear.Text);
                        newalbum.ReleaseLabel = string.IsNullOrEmpty(AlbumReleaseLabel.Text) ? null : AlbumReleaseYear.Text;

                        sysmgr.Albums_Update(newalbum);
                    }, "Update Album", "Album has been successfully update on the database.");
                }
                else
                {
                    MessageUserControl2.ShowInfo("Invalid Data", "Album Id. Use Find to locate the album you wish to maintain.");
                }               
            }           
        }
    }
    protected void DeleteAlbum_Click(object sender, EventArgs e)
    {
       
            //any other business rules
            if (string.IsNullOrEmpty(AlbumID.Text))
            {
                MessageUserControl2.ShowInfo("Missing Data", "Missing Album Id. Use Find to locate the album you wish to maintain.");
            }
            else
            {
                int albumid = 0;
                if (int.TryParse(AlbumID.Text, out albumid))
                {
                    MessageUserControl2.TryRun(() =>
                    {
                        AlbumController sysmgr = new AlbumController();
                        
                  
                        sysmgr.Albums_Delete(albumid);
                    }, "Delete Album", "Album has been successfully delete from the database.");
                }
                else
                {
                    MessageUserControl2.ShowInfo("Invalid Data", "Album Id. Use Find to locate the album you wish to maintain.");
                }
            }
        
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